using Microsoft.Win32;
using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Win10Clean
{
    class RegistryUtilities
    {
        public void TakeOwnership(string regPath, RegistryHive registryHive)
        {
            var baseReg = RegistryKey.OpenBaseKey(registryHive, RegistryView.Registry64);
            try {
                /* Get the ID of the current user (aka Amin)
                 */
                WindowsIdentity id = WindowsIdentity.GetCurrent();

                /* Add the TakeOwnership Privilege
                 */
                bool blRc = Natif.MySetPrivilege(Natif.TakeOwnership, true);
                if (!blRc)
                    throw new PrivilegeNotHeldException(Natif.TakeOwnership);

                /* Add the Restore Privilege (must be done to change the owner)
                 */
                blRc = Natif.MySetPrivilege(Natif.Restore, true);
                if (!blRc)
                    throw new PrivilegeNotHeldException(Natif.Restore);

                /* Open a registry which I don't own
                 */
                RegistryKey rkADSnapInsNodesTypes = baseReg.OpenSubKey(regPath, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.TakeOwnership);
                RegistrySecurity regSecTempo = rkADSnapInsNodesTypes.GetAccessControl(AccessControlSections.All);

                /* Get the real owner
                 */
                IdentityReference oldId = regSecTempo.GetOwner(typeof(SecurityIdentifier));
                SecurityIdentifier siTrustedInstaller = new SecurityIdentifier(oldId.ToString());

                /* process user become the owner
                 */
                regSecTempo.SetOwner(id.User);
                rkADSnapInsNodesTypes.SetAccessControl(regSecTempo);

                /* Add the full control
                 */
                RegistryAccessRule regARFullAccess = new RegistryAccessRule(id.User, RegistryRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow);
                regSecTempo.AddAccessRule(regARFullAccess);
                rkADSnapInsNodesTypes.SetAccessControl(regSecTempo);

                /* Do something 
                 */
                rkADSnapInsNodesTypes.CreateSubKey("dummy");
                rkADSnapInsNodesTypes.DeleteSubKey("dummy");
            }
            catch (Exception excpt)
            {
                throw excpt;
            }
        }

        /// <summary>
        /// Renames a subkey of the passed in registry key since 
        /// the Framework totally forgot to include such a handy feature.
        /// </summary>
        /// <param name="regKey">The RegistryKey that contains the subkey 
        /// you want to rename (must be writeable)</param>
        /// <param name="subKeyName">The name of the subkey that you want to rename
        /// </param>
        /// <param name="newSubKeyName">The new name of the RegistryKey</param>
        /// <returns>True if succeeds</returns>
        public bool RenameSubKey(RegistryKey parentKey,
           string subKeyName, string newSubKeyName)
        {
            CopyKey(parentKey, subKeyName, newSubKeyName);
            parentKey.DeleteSubKeyTree(subKeyName);
            return true;
        }


        /// <summary>
        /// Copy a registry key.  The parentKey must be writeable.
        /// </summary>
        /// <param name="parentKey"></param>
        /// <param name="keyNameToCopy"></param>
        /// <param name="newKeyName"></param>
        /// <returns></returns>
        public bool CopyKey(RegistryKey parentKey,
            string keyNameToCopy, string newKeyName)
        {
            //Create new key
            RegistryKey destinationKey = parentKey.CreateSubKey(newKeyName);

            //Open the sourceKey we are copying from
            RegistryKey sourceKey = parentKey.OpenSubKey(keyNameToCopy);

            RecurseCopyKey(sourceKey, destinationKey);

            return true;
        }

        private void RecurseCopyKey(RegistryKey sourceKey, RegistryKey destinationKey)
        {
            //copy all the values
            foreach (string valueName in sourceKey.GetValueNames())
            {
                object objValue = sourceKey.GetValue(valueName);
                RegistryValueKind valKind = sourceKey.GetValueKind(valueName);
                destinationKey.SetValue(valueName, objValue, valKind);
            }

            //For Each subKey 
            //Create a new subKey in destinationKey 
            //Call myself 
            foreach (string sourceSubKeyName in sourceKey.GetSubKeyNames())
            {
                RegistryKey sourceSubKey = sourceKey.OpenSubKey(sourceSubKeyName);
                RegistryKey destSubKey = destinationKey.CreateSubKey(sourceSubKeyName);
                RecurseCopyKey(sourceSubKey, destSubKey);
            }
        }

    }
}

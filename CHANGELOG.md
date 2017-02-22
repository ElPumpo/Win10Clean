# Change Log
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/) 
and this project adheres to [Semantic Versioning](http://semver.org/).

## [Unreleased]
### Added
- Ability to cleanup the context menu for extensions. Like printing, editing (notepad), play song and add to playlist (wmp).
- Uninstalling apps now have correct feedback (sucess, fail).
- Added removing OneDrive from Explorer file dialogs (amd64 only).

### Fixed
- Prompt for uninstalling apps now tell every app to be uninstalled.

## [0.9.0] - 2017-02-04
### Added
- Added a query to check if the start menu ads already has been disabled, and if so - give user choice to re-enable.
- Added a fix for uninstalled apps reinstalling.
- The OneDrive uninstaller now remove any leftovers found in the task scheduler.
- Ability to uninstall multiple apps at once.

### Changed
- Cleanup in code, now using `Using` statements in a attempt in lowering the memory usage.

## [0.8.0] - 2016-11-15
### Fixed
- Fixed tab index yet again.
- The OneDrive should now be sucessfully remove the icon from Explorer, and does no longer remove stuff from the registry.
- Fixed a `Revert7` related error if registry key doesn't exist.

## [0.7.0] - 2016-11-03
### Added
- Added support to uninstall apps installed for all users.
- Replaced all forms with a big one.
- Added console for extended debugging information.
- Executable icon, thanks to https://github.com/Maddoc42/Android-Material-Icon-Generator.

## [0.6.0] - 2016-10-02
### Added
- You can now disable HomeGroup.
- Added tooltips for extended information.
- Added fancy icons by famfamfam, http://famfamfam.com.
- Added a about class with legal stuff.
- Added option to disable home menu ads.
- Pressing enter on your keyboard in the app uninstaller GUI will now act as selecting uninstall.

### Changed
- A lot of actions including modifing the registry has been improved.
- Improved debugging (still room for improvements, expect a debug GUI).
- When the app list for uninstalling Win10 apps is refreshed, it will save the selected app and take you back to whatever you selected - thank my friends for their feedback.
- Improved Defender disabler; now removing startup process and unregistering right-click menu scans.
- Improved the libary remover, the name has been changed to `Revert7` because it will now replicate the Windows 7 File Explorer settings. It now also pins the libary folder, disables quick access filling up with random crap and sets the default dir of explorer to `My PC`.

### Fixed
- Fixed the tabbing index, or whenever you try to scroll the GUI using a keyboard.

## [0.5.0] - 2016-09-23
Initial public release

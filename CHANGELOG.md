# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [2.1.1] - 2023-04-08
### Changed
- Implemented the ability to use the MenuHandler as a singleton through the use of the 'VULPES_MENUS_SINGLETON' scripting define.

## [2.1.0] - 2023-04-06
### Added
- Added MenuStackSnapshot to allow for caching and restoring of particular screen stack states.

## [2.0.2] - 2023-03-10
### Changed
- Wrapped OnValidate in editor only tags for MenuHandler and MenuWidget_Slider.

## [2.0.1] - 2023-01-31
### Changed
- Updated Transitions Package dependency from 1.0.3 to 1.0.4.

## [2.0.0] - 2023-01-27
### Changed
- Simplified a lot of interfaces by removing redundant methods and properties.
- Most classes now have the 'DisallowMultipleComponent' attribute.
- Cleaned up some of the code to make things more concise.
- For 'MenuScreens' the 'TransitionIn' and 'TransitionOut' now take a 'MenuScreenTransitionContext' as an argument instead of a boolean value.
- The 'MenuHandler' no longer requires a 'CanvasGroup' component and instead requires a 'Canvas' component.
- On the 'MenuHandler', the 'Visible' property now directly toggles the 'Canvas' component on and off rather than awkwardly setting the alpha value of the 'CanvasGroup'.
- Removed 'MenuTooltip' logic from 'MenuHandler', that logic is now included in an optional 'MenuTooltipHandler' component which should be added to the same object as the 'MenuHandler' if you are using the 'MenuTooltip' as part of your menus.
- 'MenuHandler' no longer manages 'EventSystem' selections.
- Moved 'Raycast' method from 'MenuHandler' to new optional component 'MenuRaycaster'.
- Separated screen stack logic from 'IMenuHandler' into new 'IMenuStack' interface.
- Renamed 'MenuTooltipDetails' to 'MenuTooltipContent'.
- Updated Transitions Package dependency from 1.0.2 to 1.0.3.

### Fixed
- 'MenuModal' and 'MenuLoading' can no longer be assigned as the MenuHandler's initial screen.
- Fixed issue where 'null' could incorrectly be pushed to the screen stack, potentially causing a Null Reference Exception, attempting to push 'null' will now return a rejected Promise.

### Added
- Added 'MenuScreenTransitionContext' struct.
- Added 'MenuWidget_Toggle' class.
- Added 'MenuRaycaster' class.
- Added 'MenuTooltipHandler' class.
- Added 'IMenuStack' interface.
- Added 'IMenuTooltipProvider' interface.
- Added new Menu Sample assets.
- Added Documentation.

## [1.0.2] - 2023-01-22
### Changed
- Changed Unity Version from 2021.3.6f1 to 2021.3.0f1.
- Updated Promises Package dependency from 2.0.0 to 2.1.1.
- Updated Transitions Package dependency from 1.0.0 to 1.0.2.

## [1.0.1] - 2022-07-14
### Changed
- Updated Unity Version from 2021.2.0f1 to 2021.3.6f1
- Updated Text Mesh Pro version from 2.0.1 to 3.0.6 due to a GitHub security warning regarding the package.

## [1.0.0] - 2022-03-14
### Changed
- Updated Promises Package dependency from 1.1.0 to 2.0.0.

## [0.9.0-preview.1] - 2022-03-09
### Changed
- Renamed 'IMenuDialogue' and 'MenuDialogue' to 'IMenuModal' and 'MenuModal' respectively.
- Renamed 'MenuHandler.Dialogue' to 'MenuHandler.Modal'.
- Removed 'MenuTransition' classes and moved them into their own package 'com.vulpes.transitions'.
- Added 'com.vulpes.transitions' as a package dependency.
- Other updates preparing package for 1.0.0 release.

## [0.6.0-preview.1] - 2021-10-28
### Changed
- Stripped down MenuLoading class.
- Removed Loading Screen Tip class.
- MenuWidget value changed and click events are now UnityEvents rather than Action based events.
- Updated Example Assets.
- Changed default MenuTransitionOption to Sequential, was Parallel.
- MenuHandler will now correctly get Alert, Dialogue, and Tooltip using their relevant interfaces rather than base classes.
- MenuHandler now includes an interface reference to the Loading Screen.
- Loading Screen spinner now uses unscaled deltatime in its rotation.

## [0.5.0-preview.1] - 2021-10-10
### Changed
- Add Unity Input System Package as a dependency.

### Added
- Added Menu Tooltip and Tooltip Details classes support when Mouse Input is in use.

## [0.4.0-preview.3] - 2021-09-04
### Changed
- Fixed typo in Read Me file.
- Made loading screen tip body text a multiline text area.
- Cleaned up code.

## [0.4.0-preview.2] - 2021-05-07
### Changed
- Removed readonly 'in' modifier from nullable arguments.

## [0.4.0-preview.1] - 2021-05-02
### Changed
- Rolled forward minimum Unity version to 2019.3.10f1.
- Cleaned up code.
- Simplified Menu Transition delay option.
- Implemented 'in' keyword for readonly arguments.
- Default Menu Transition curves are now linear.
- Removed 'Reset On Initialize', 'Reset On Play', and 'Disable When Done' options from Menu Transitions and replaced them with 'Flags'.

## [0.3.5-preview.1] - 2020-11-21
### Changed
- Merged Sample Packages into one.

## [0.3.4-preview.1] - 2020-11-08
### Changed
- Added ability to set the transitioned in 'interactable' and 'blocksRaycast' states for MenuTransition_CanvasGroupAlpha.

## [0.3.3-preview.1] - 2020-10-13
### Changed
- Fixed a bug where MenuScreens that didn't use a MenuTransition component couldn't be interacted with after transitioning in.
- Resolved issue where changing Slider and Enumerable values wouldn't reselect the MenuWidget.
- Updated Examples.

## [0.3.2-preview.2] - 2020-09-27
### Changed
- Added more cref tags to method and class summaries.

## [0.3.2-preview.1] - 2020-09-26
### Changed
- Added cref tags to method and class summaries.
- Unsealed MenuHandler, MenuAlert, MenuDialogue, MenuLoading, and MenuLoadingTip classes.
- Moved MenuWidgets out of the Experimental namespace.

## [0.3.1-preview.1] - 2020-09-20
### Changed
- Made Start and End values of MenuTransition virtual so that they can be overridden.
- Fixed issue with MenuTransition_AnchoredPosition where changing the Start and End values at runtime would have no effect.

## [0.3.0-preview.1] - 2020-09-20
### Changed
- Changed default value of MenuTransitionMode to Reverse, was Forward. This should resolve an issue where some menus wouldn't transition in correctly.
- MenuDialogue can no longer be shown if a MenuDialogue is already being shown. This is a temporary fix to prevent softlocking.

### Added
- Added IsCurrentScreen value to MenuScreen.

## [0.2.3-preview.1] - 2020-08-27
### Changed
- Fixed an issue with Position, Euler Angles, and Scale MenuTransitions where they were lerping as Vector2s instead of Vector3s.

## [0.2.2-preview.1] - 2020-08-27
### Changed
- Fixed issue when pushing screen with uninitialised screen stack.

### Added
- Added initial screen option to Menu Handler, this screen will automatically be pushed at startup if assigned.

## [0.2.1-preview.2] - 2020-08-23
### Changed
- Updated some method summaries.

## [0.2.1-preview.1] - 2020-08-15
### Changed
- MenuScreens now include an option to remember the last selected item prior to transitioning out.

## [0.2.0-preview.3] - 2020-08-14
### Added
- Added MenuDialogue and MenuAlert Sample Prefabs.

## [0.2.0-preview.2] - 2020-08-14
### Changed
- Cleaned up some of the MenuWidget code following further testing.

## [0.2.0-preview.1] - 2020-08-14
### Added
- Added MenuWidget example Prefabs.
- Added more sample sptites.
- Added MenuLoading screen class.
- Added MenuLoadingTip scriptable object class.
- Added MenuWidget<T> class that extends MenuWidget (see changes for more information).
- MenuWidget now contains the following events: OnSelectEvent, OnPointerEnterEvent, OnPointerClickEvent, OnSubmitEvent.
- MenuWidget<T> now includes the OnValueChanged<T> event that was previously included in the MenuWidget class.

### Changed
- Widget headers are no longer forced to upper case.
- Split MenuWidget into two classes and genericised some of the Value functionality.
- MenuWidget_Enumerable now uses MenuWidget<int> as a base component.
- MenuWidget_Slider now uses MenuWidget<int> as a base component.
- MenuWidget_Dropdown now uses MenuWidget<int> as a base component.
- MenuWidget_InputField now uses MenuWidget<string> as a base component.
- MenuWidget no longer contains an OnValueChanged method, this has been moved to the MenuWidget<T> class.

## [0.1.0-preview.1] - 2020-08-03
This is the first release of *Vulpes Menu Framework* as a Package.
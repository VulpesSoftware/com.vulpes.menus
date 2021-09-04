# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

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
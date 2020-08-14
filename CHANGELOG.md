# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

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
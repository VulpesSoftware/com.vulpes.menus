# Vulpes Menu Framework

*This is a preview package and is subject to radical change, please use at your own peril.*

A Promise driven game menu framework for Unity.

*NOTE:* This Package depends on '[com.vulpes.promises](https://github.com/VulpesSoftware/com.vulpes.promises.git#1.0.0)'.

## Installing this package

As of version 0.3.4-preview.1 of this package, you are now able to install it, along with other Vulpes Software packages via the Unity Package Manager. 

In Unity 2019 LTS and Unity 2020 onwards you can install the package through 'Project Settings/Package Manager'. Under 'Scoped Registries' click the little '+' button and input the following into the fields on the right.

*Name:* Vulpes Software
*URL:* https://registry.npmjs.org
*Scope(s):* com.vulpes

Click 'Apply', now you should be able to access the Vulpes Software registry under the 'My Registries' section in the Package Manager window using the second dropdown in the top left.

## Using this library

To use this library efficiently you should first learn about Promises:

- [Promises on Wikpedia](http://en.wikipedia.org/wiki/Futures_and_promises)
- [Good overview](https://www.promisejs.org/)
- [Mozilla](https://developer.mozilla.org/en/docs/Web/JavaScript/Reference/Global_Objects/Promise)

## Samples

The current Samples included with this package are in an incomplete state and may not always be updated from version to version, a proper comprehensive collection of sample assets will be included in version 1.0.0 of this package whenever it is released.

## Current Development Roadmap

Several features and changes are currently under investigation and may or may not be added to the package in the future.

*In Progress:*
- Tooltip component for displaying a floating text box at the mouse position when hovering over certain objects (UX).
  - Currently looking into adding conditional support for Unity's Input System package.
  - Investigating ways of supporting tooltip popup when a Gamepad is in use.

*Planned:*
- Color Profile System for quick UI color changes at runtime (Accessibility).
  - Assessing how to best support tints for buttons, toggles, sliders, etc.
  - Assessing options for swapping entire sprites.
- Genericising Menu Transition Components and spliting them into their own package that isn't dependent on the Menu Framework (Modularity).
  - Vulpes Transitions package is currently in an internal preview state.
  - Likely to be released along with Vulpes Menu Framework version 0.5.0-preview.1 or 0.6.0-preview.1 at the latest.

 *Under Consideration*
- Use of the Unity Jobs System or Threading / Tasks as an alternative to Promises (Performance).
  - Currently in very early phase of investigation.
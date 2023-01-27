# Menu Loading

The **Menu Loading** screen is a unique **Menu Screen** that can be used to mask various loading sequences. When the Show method is called you can provide a predicate to determine completion of the load operation, a progress predicate that will update the loading percentage and loading bars, and a load operation that can be used to trigger code when the completion predicate resolves.

|**Property:** |**Function:** |
|:---|:---|
|**Cursor Visibility** |Can be used to change the visibility of the mouse cursor on supported platforms. Options are _Unchanged_, _Show Cursor_, _Hide Cursor_. |
|**Remember Selection** |If true, this screen will attempt to remember the previous selected menu item for the next time that this screen is shown. |
|**Initial Selection** |The initial selection on this screen when it is Transitioned in. |
|**Transition** |The Transition used for showing/hiding this screen. |
|**Progress Percentage Text** |Displays load progress as a percentage on the screen. |
|**Progress Bar Slider** |Displays load progress as a bar on the screen. |
|**Spinner Rect Transform** |This Rect Transform will be spun clockwise based on Spinner Speed while this screen is active. |
|**Spinner Speed** |Rotation speed of the Spinner Rect Transform in degrees per second. |


## Hints
* All values on this screen are optional in order to allow it to be easily customised on a per-project basis.
* There is a generic example Menu Loading screen included as part of the Menu Samples, you can use this as a template for building your own loading screen.
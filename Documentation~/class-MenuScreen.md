# Menu Screen

The **Menu Screen** class is the base class for all screens, it contains controls for cursor visibility as well as the **Transition** types.

|**Property:** |**Function:** |
|:---|:---|
|**Cursor Visibility** |Can be used to change the visibility of the mouse cursor on supported platforms. Options are _Unchanged_, _Show Cursor_, _Hide Cursor_. |
|**Remember Selection** |If true, this screen will attempt to remember the previous selected menu item for the next time that this screen is shown. |
|**Initial Selection** |The initial selection on this screen when it is Transitioned in. |
|**Transition** |The Transition used for showing/hiding this screen. |


## Hints
* Leave Cursor Visibility as 'Unchanged' if you want to control Cursor Locking and Visibility from your own code.
* If no Transition is assigned, Screen Transitions will be resolved by simply turning the GameObject on and off.
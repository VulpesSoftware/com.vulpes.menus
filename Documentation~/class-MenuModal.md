# Menu Modal

The **Menu Modal** is a unique **Menu Screen** that can be used to query the user. It can be displayed with one, two, or three **Buttons**. When displayed the **Menu Modal** will push itself to the screen stack and will automatically remove itself once a selection is made. When the Show method is called on the **Menu Modal**, it will return a **Promise** that resolves with a return type of **MenuModalResult**, this can be used to execute additional code based on the selected option.

|**Property:** |**Function:** |
|:---|:---|
|**Cursor Visibility** |Can be used to change the visibility of the mouse cursor on supported platforms. Options are _Unchanged_, _Show Cursor_, _Hide Cursor_. |
|**Remember Selection** |If true, this screen will attempt to remember the previous selected menu item for the next time that this screen is shown. |
|**Initial Selection** |The initial selection on this screen when it is Transitioned in. |
|**Transition** |The Transition used for showing/hiding this screen. |
|**Title Text** |Text used for displaying a title. |
|**Body Text** |Text used for displaying a larger body of text. |
|**Confirm Text** |Text used on the Confirm Button. |
|**Cancel Text** |Text used on the Cancel Button. |
|**Alternate Text** |Text used on the Alternate Button. |
|**Confirm Button** |The Confirm Button. |
|**Cancel Button** |The Cancel Button. |
|**Alternate Button** |The Alternate Button. |


## Hints
* There is a generic example Menu Modal included as part of the Menu Samples, you can use this as a template for building your own modal.
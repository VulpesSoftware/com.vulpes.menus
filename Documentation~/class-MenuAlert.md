# Menu Alert

The **Menu Alert** is a universal, non-intrusive popup that can be shown for a specified duration, or until a given **Promise** is resolved. The **Menu Alert** will return a **Promise** that resolves once the current message is finished showing. It’s also worth noting that if the **Menu Alert** is already displaying a message, any subsequent attempts to Show a message will add the message to the active **Promise** chain, and attempting to show the alert again will queue the same message again. Ideally the **Menu Alert** should only be used for displaying system messages like when the game is saving, etc.

|**Property:** |**Function:** |
|:---|:---|
|**Alert Text** |Text used for displaying a string in the alert. |
|**Icon Image** |Image used for displaying a Sprite in the alert. |
|**Transition** |The Transition used for showing/hiding the alert. |
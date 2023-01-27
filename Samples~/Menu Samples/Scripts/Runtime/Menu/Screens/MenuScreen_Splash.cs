using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Vulpes.Menus.Examples
{
    [AddComponentMenu("Vulpes/Menus/Examples/Screens/Splash")]
    public sealed class MenuScreen_Splash : MenuScreen
    {
        // This screen is just a basic call to action screen, when an input is received the
        // screen stack will be set to the title screen.

        private IMenuScreen _titleSceen;

#if ENABLE_INPUT_SYSTEM
        private bool CurrentKeyboardButtonPressed
            => Keyboard.current != null &&
            Keyboard.current.anyKey.isPressed;

        private bool CurrentMouseButtonPressed
            => Mouse.current != null &&
            (Mouse.current.leftButton.isPressed ||
            Mouse.current.rightButton.isPressed ||
            Mouse.current.middleButton.isPressed);

        private bool CurrentGamepadButtonPressed
            => Gamepad.current != null &&
            (Gamepad.current.aButton.isPressed ||
            Gamepad.current.crossButton.isPressed ||
            Gamepad.current.bButton.isPressed ||
            Gamepad.current.circleButton.isPressed ||
            Gamepad.current.xButton.isPressed ||
            Gamepad.current.squareButton.isPressed ||
            Gamepad.current.yButton.isPressed ||
            Gamepad.current.triangleButton.isPressed ||
            Gamepad.current.startButton.isPressed ||
            Gamepad.current.selectButton.isPressed);

        private bool AnyButtonPressed
            => CurrentKeyboardButtonPressed ||
            CurrentMouseButtonPressed ||
            CurrentGamepadButtonPressed;
#endif

        public override void OnInitialize()
            => _titleSceen = MenuHandler.GetScreen<MenuScreen_Title>();

        public override void OnActive()
        {
            // Since OnActive is called every frame while this is the current screen we don't want
            // to allow inputs to be polled until it's fully transitioned in.
            if (State != MenuScreenState.In)
            {
                return;
            }
#if ENABLE_INPUT_SYSTEM
            if (AnyButtonPressed)
#else
            if (Input.anyKeyDown)
#endif
            {
                MenuHandler.SetScreenStack(_titleSceen, MenuTransitionOptions.Sequential);
            }
        }
    }
}
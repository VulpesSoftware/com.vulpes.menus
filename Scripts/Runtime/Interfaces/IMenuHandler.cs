using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    [Flags]
    public enum MenuTransitionOptions
    {
        /// <summary>Transitions both screens at the same time.</summary>
        Parallel = (1 << 0),
        /// <summary>Transitions the current screen out, then the new one in.</summary>
        Sequential = (1 << 1),
        /// <summary>Transitions in the new screen instantly.</summary>
        InInstant = (1 << 2),
        /// <summary>Transitions out the current screen instantly.</summary>
        OutInstant = (1 << 3),
        /// <summary>Transitions both screens instantly.</summary>
        BothInstant = (InInstant | OutInstant),
    }

    public interface IMenuHandler 
    {
        EventSystem EventSystem { get; }

        CanvasGroup CanvasGroup { get; }

        bool Visible { get; set; }

        bool CursorLocked { get; set; }

        event Action<IMenuScreen, IMenuScreen> OnScreenStateWillChangeEvent;

        event Action<IMenuScreen, IMenuScreen> OnScreenStateDidChangeEvent;

        Stack<IMenuScreen> ScreenStack { get; }

        IMenuScreen CurrentScreen { get; }

        bool HasScreen { get; }

        IMenuAlert Alert { get; }

        IMenuDialogue Dialogue { get; }

        IPromise PushScreen(IMenuScreen akMenuScreen, MenuTransitionOptions akOptions = MenuTransitionOptions.Parallel);

        IPromise PopScreen(MenuTransitionOptions akOptions = MenuTransitionOptions.Parallel);

        IPromise PopPushScreen(IMenuScreen akMenuScreen, MenuTransitionOptions akOptions = MenuTransitionOptions.Parallel);

        IPromise PopToScreen(IMenuScreen akMenuScreen, MenuTransitionOptions akOptions = MenuTransitionOptions.Parallel);

        IPromise PopAllScreens(IMenuScreen akMenuScreen = null, MenuTransitionOptions akOptions = MenuTransitionOptions.Parallel);

        IPromise SetScreenStack(IMenuScreen akMenuScreens, MenuTransitionOptions akOptions = MenuTransitionOptions.Parallel);

        IPromise SetScreenStack(IMenuScreen[] akMenuScreens, MenuTransitionOptions akOptions = MenuTransitionOptions.Parallel);

        T GetScreen<T>() where T : IMenuScreen;
    }
}

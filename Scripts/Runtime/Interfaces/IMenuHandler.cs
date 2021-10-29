using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    [Flags]
    public enum MenuTransitionOptions : int
    {
        /// <summary>Transitions both screens at the same time.</summary>
        Parallel = 1 << 0,
        /// <summary>Transitions the current screen out, then the new one in.</summary>
        Sequential = 1 << 1,
        /// <summary>Transitions in the new screen instantly.</summary>
        InInstant = 1 << 2,
        /// <summary>Transitions out the current screen instantly.</summary>
        OutInstant = 1 << 3,
        /// <summary>Transitions both screens instantly.</summary>
        BothInstant = InInstant | OutInstant,
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

        IMenuLoading Loading { get; }

        IPromise PushScreen(IMenuScreen menuScreen, in MenuTransitionOptions options = MenuTransitionOptions.Parallel);

        IPromise PopScreen(in MenuTransitionOptions options = MenuTransitionOptions.Parallel);

        IPromise PopPushScreen(IMenuScreen menuScreen, in MenuTransitionOptions options = MenuTransitionOptions.Parallel);

        IPromise PopToScreen(IMenuScreen menuScreen, in MenuTransitionOptions options = MenuTransitionOptions.Parallel);

        IPromise PopAllScreens(IMenuScreen menuScreen = null, in MenuTransitionOptions options = MenuTransitionOptions.Parallel);

        IPromise SetScreenStack(IMenuScreen menuScreens, in MenuTransitionOptions options = MenuTransitionOptions.Parallel);

        IPromise SetScreenStack(IMenuScreen[] menuScreens, in MenuTransitionOptions options = MenuTransitionOptions.Parallel);

        T GetScreen<T>() where T : IMenuScreen;
    }
}

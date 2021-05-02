using System;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    public enum MenuScreenState : int
    {
        /// <summary>The screen is currently fully transitioned out.</summary>
        Out,
        /// <summary>The screen is currently transitioning in.</summary>
        TransitioningIn,
        /// <summary>The screen is currently fully transitioned in.</summary>
        In,
        /// <summary>The screen is currently transitioning out.</summary>
        TransitioningOut,
    }

    public interface IMenuScreen 
    {
        IMenuHandler MenuHandler { get; }

        MenuScreenState State { get; }

        bool Interactable { get; set; }

        bool IsTransitioning { get; }

        event Action<MenuScreenState, MenuScreenState> OnStateChangedEvent;

        IPromise TransitionIn(bool instant = false);

        IPromise TransitionOut(bool instant = false);

        void Initialize(IMenuHandler menuHandler);

        void OnWillAppear();

        void OnDidAppear();

        void OnWillDisappear();

        void OnDidDisappear();

        void OnInitialize();

        void OnActive();
    }
}

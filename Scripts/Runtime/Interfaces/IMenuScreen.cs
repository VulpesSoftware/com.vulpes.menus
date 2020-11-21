using System;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    public enum MenuScreenState
    {
        /// <summary>The screen is currently fully transitioned out.</summary>
        Out = 0,
        /// <summary>The screen is currently transitioning in.</summary>
        TransitioningIn = 1,
        /// <summary>The screen is currently fully transitioned in.</summary>
        In = 2,
        /// <summary>The screen is currently transitioning out.</summary>
        TransitioningOut = 3,
    }

    public interface IMenuScreen 
    {
        IMenuHandler MenuHandler { get; }

        MenuScreenState State { get; }

        bool Interactable { get; set; }

        bool IsTransitioning { get; }

        event Action<MenuScreenState, MenuScreenState> OnStateChangedEvent;

        IPromise TransitionIn(bool abInstant = false);

        IPromise TransitionOut(bool abInstant = false);

        void Initialize(IMenuHandler akMenuHandler);

        void OnWillAppear();

        void OnDidAppear();

        void OnWillDisappear();

        void OnDidDisappear();

        void OnInitialize();

        void OnActive();
    }
}

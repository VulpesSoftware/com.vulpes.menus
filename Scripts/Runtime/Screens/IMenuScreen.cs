using Vulpes.Promises;

namespace Vulpes.Menus
{
    public enum MenuScreenState : int
    {
        Out,
        TransitioningIn,
        In,
        TransitioningOut,
    }

    public interface IMenuScreen 
    {
        IMenuHandler MenuHandler { get; }

        MenuScreenState State { get; }

        bool IsTransitioning { get; }

        bool IsCurrentScreen { get; }

        IPromise TransitionIn(MenuScreenTransitionContext context);

        IPromise TransitionOut(MenuScreenTransitionContext context);

        void Initialize(IMenuHandler menuHandler);

        void Active();
    }
}
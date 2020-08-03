using Vulpes.Promises;

namespace Vulpes.Menus
{
    public enum MenuTransitionMode
    {
        Forward = 0,
        Reverse = 1,
    }

    public interface IMenuTransition 
    {
        void Initialize();

        IPromise Play(MenuTransitionMode akTransitionMode = MenuTransitionMode.Forward, bool abInstant = false, float? afDelay = null);
        
        void Complete();
        
        void SetTime(float afTime, MenuTransitionMode akMode = MenuTransitionMode.Forward);

        float CurrentTime { get; }
    }
}

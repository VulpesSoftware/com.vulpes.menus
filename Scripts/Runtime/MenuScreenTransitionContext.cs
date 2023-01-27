namespace Vulpes.Menus
{
    public readonly struct MenuScreenTransitionContext
    {
        public readonly bool instant;
        public readonly IMenuScreen otherScreen;

        public MenuScreenTransitionContext(in bool instant, IMenuScreen otherScreen)
        {
            this.instant = instant;
            this.otherScreen = otherScreen;
        }
    }
}
using System;
using System.Collections.Generic;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    [Flags]
    public enum MenuTransitionOptions : int
    {
        Parallel = 1 << 0,
        Sequential = 1 << 1,
        InInstant = 1 << 2,
        OutInstant = 1 << 3,
        BothInstant = InInstant | OutInstant,
    }

    public interface IMenuStack
    {
        Stack<IMenuScreen> ScreenStack { get; }

        IMenuScreen CurrentScreen { get; }

        IPromise PushScreen(IMenuScreen menuScreen, MenuTransitionOptions options = MenuTransitionOptions.Parallel);

        IPromise PopScreen(MenuTransitionOptions options = MenuTransitionOptions.Parallel);

        IPromise PopPushScreen(IMenuScreen menuScreen, MenuTransitionOptions options = MenuTransitionOptions.Parallel);

        IPromise PopToScreen(IMenuScreen menuScreen, MenuTransitionOptions options = MenuTransitionOptions.Parallel);

        IPromise PopAllScreens(IMenuScreen menuScreen = null, MenuTransitionOptions options = MenuTransitionOptions.Parallel);

        IPromise SetScreenStack(IMenuScreen menuScreens, MenuTransitionOptions options = MenuTransitionOptions.Parallel);

        IPromise SetScreenStack(IMenuScreen[] menuScreens, MenuTransitionOptions options = MenuTransitionOptions.Parallel);

        IPromise SetScreenStack(MenuStackSnapshot snapshot, MenuTransitionOptions options = MenuTransitionOptions.Parallel);

        MenuStackSnapshot GetCurrentMenuStackSnapshot();
    }
}
using System.Collections.Generic;

namespace Vulpes.Menus
{
    public readonly struct MenuStackSnapshot
    {
        public readonly IMenuScreen[] menuScreens;

        public MenuStackSnapshot(params IMenuScreen[] menuScreens)
            => this.menuScreens = menuScreens;

        public MenuStackSnapshot(Stack<IMenuScreen> screenStack)
        {
            Stack<IMenuScreen> reversedScreenStack = new();
            while (screenStack.Count > 0)
            {
                reversedScreenStack.Push(screenStack.Pop());
            }
            menuScreens = reversedScreenStack.ToArray();
        }
    }
}
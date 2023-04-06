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
            menuScreens = screenStack.ToArray();
            int i = 0;
            int j = menuScreens.Length - 1;
            while (i < j)
            {
                IMenuScreen temp = menuScreens[i];
                menuScreens[i] = menuScreens[j];
                menuScreens[j] = temp;
                i++;
                j--;
            }
        }
    }
}
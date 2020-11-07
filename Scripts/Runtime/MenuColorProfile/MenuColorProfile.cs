using UnityEngine;
using UnityEngine.UI;

namespace Vulpes.Menus.Experimental
{
    [CreateAssetMenu(fileName = "New Menu Color Profile", menuName = "Vulpes/Menus/Experimental/Menu Color Profile", order = 4096)]
    public sealed class MenuColorProfile : ScriptableObject
    {
        public Color[] colors = new Color[1] { Color.white };
        public ColorBlock[] colorBlocks = new ColorBlock[1] { ColorBlock.defaultColorBlock };

        public Color GetColor(int index)
        {
            return (index < 0 || index >= colors.Length) ? Color.white : colors[index];
        }

        public ColorBlock GetColorBlock(int index)
        {
            return (index < 0 || index >= colorBlocks.Length) ? ColorBlock.defaultColorBlock : colorBlocks[index];
        }
    }
}
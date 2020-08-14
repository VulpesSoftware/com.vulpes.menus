using UnityEngine;

namespace Vulpes.Menus
{
    [CreateAssetMenu(fileName = "New Menu Loading Tip", menuName = "Vulpes/Menus/Menu Loading Tip", order = 4096)]
    public sealed class MenuLoadingTip : ScriptableObject
    {
        public string title;
        public string subtitle;
        public string body;
        public Texture texture;
    }
}

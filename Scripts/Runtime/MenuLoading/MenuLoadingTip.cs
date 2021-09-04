using UnityEngine;

namespace Vulpes.Menus
{
    [CreateAssetMenu(fileName = "New Menu Loading Tip", menuName = "Vulpes/Menus/Menu Loading Tip", order = 4096)]
    public class MenuLoadingTip : ScriptableObject
    {
        public string title;
        public string subtitle;
        [TextArea(minLines: 4, maxLines: 16)] public string body;
        public Texture texture;
    }
}

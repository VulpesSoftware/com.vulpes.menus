using UnityEngine.EventSystems;

namespace Vulpes.Menus
{
    public interface IMenuTooltipProvider : IPointerEnterHandler, IPointerExitHandler
    {
        void Initialize(IMenuTooltip tooltip);
    }
}
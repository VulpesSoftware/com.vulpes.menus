using UnityEngine.EventSystems;

namespace Vulpes.Menus
{
    public interface IMenuTooltipData : IPointerEnterHandler, IPointerExitHandler
    {
        string Title { get; }

        string Body { get; }

        void Initialize(IMenuTooltip tooltip);
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vulpes.Menus
{
    [AddComponentMenu("Vulpes/Menus/Menu Tooltip Details")]
    public sealed class MenuTooltipDetails : UIBehaviour, IMenuTooltipData
    {
        public string title;
        [TextArea(4, 16)] public string body;

        private IMenuTooltip tooltip;

        public string Title => title;

        public string Body => body;

        public void Initialize(IMenuTooltip tooltip) => this.tooltip = tooltip;

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltip.Show(Title, Body);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltip.Hide();
        }
    }
}
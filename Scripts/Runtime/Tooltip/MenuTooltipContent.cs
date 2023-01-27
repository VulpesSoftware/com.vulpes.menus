using UnityEngine;
using UnityEngine.EventSystems;

namespace Vulpes.Menus
{
    [AddComponentMenu("Vulpes/Menus/Tooltip/Menu Tooltip Content"), DisallowMultipleComponent]
    public sealed class MenuTooltipContent : UIBehaviour, IMenuTooltipProvider
    {
        [SerializeField] private string title = default;
        [SerializeField, TextArea(4, 16)] private string body = default;

        private IMenuTooltip tooltip;

        public void Initialize(IMenuTooltip tooltip) 
            => this.tooltip = tooltip;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (tooltip == null)
            {
                return;
            }
            tooltip.Show(title, body);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (tooltip == null)
            {
                return;
            }
            tooltip.Hide();
        }
    }
}
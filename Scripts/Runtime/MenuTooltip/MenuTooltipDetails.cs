using UnityEngine;
using UnityEngine.EventSystems;

namespace Vulpes.Menus
{
    [AddComponentMenu("Vulpes/Menus/Menu Tooltip Details")]
    public sealed class MenuTooltipDetails : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        public string title;
        public string body;
        public Transform defaultAnchorPoint;

        // HACK
        public MenuTooltip menuTooltip;

        private void Awake()
        {
            if (defaultAnchorPoint == null)
            {
                defaultAnchorPoint = transform;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            menuTooltip.Show(title, body);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                return;
            }
            menuTooltip.Hide();
        }

        public void OnSelect(BaseEventData eventData)
        {
            menuTooltip.Show(title, body);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            menuTooltip.Hide();
        }
    }
}
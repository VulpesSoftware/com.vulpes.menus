using UnityEngine;
using UnityEngine.EventSystems;

namespace Vulpes.Menus
{
    [AddComponentMenu("Vulpes/Menus/Menu Tooltip Details")]
    public sealed class MenuTooltipDetails : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string title;
        [Multiline(4)]
        public string body;

        [SerializeField] private MenuTooltip menuTooltip;

        public void OnPointerEnter(PointerEventData eventData)
        {
            menuTooltip.Show(title, body);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            menuTooltip.Hide();
        }
    }
}
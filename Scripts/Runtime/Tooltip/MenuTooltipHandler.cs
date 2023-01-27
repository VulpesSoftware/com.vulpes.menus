using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Vulpes.Menus
{
    [AddComponentMenu("Vulpes/Menus/Tooltip/Menu Tooltip Handler"), RequireComponent(typeof(Canvas)), DisallowMultipleComponent]
    public sealed class MenuTooltipHandler : UIBehaviour
    {
        private Canvas canvas;
        private RectTransform rectTransform;
        private IMenuTooltip tooltip;

        protected override void Awake()
        {
            base.Awake();
            canvas = GetComponent<Canvas>();
            rectTransform = GetComponent<RectTransform>();
            tooltip = GetComponentInChildren<IMenuTooltip>(true);
            if (tooltip == null)
            {
                Debug.LogWarning($"No Menu Tooltip found in Hierarchy, consider removing the 'MenuTooltipHandler' component from '{gameObject.name}' if it's not needed.");
                return;
            }
            IMenuTooltipProvider[] tooltipData = GetComponentsInChildren<IMenuTooltipProvider>(true);
            for (int i = tooltipData.Length - 1; i >= 0; i--)
            {
                tooltipData[i].Initialize(tooltip);
            }
        }

        private void Update()
        {
            if (tooltip == null)
            {
                return;
            }
#if ENABLE_INPUT_SYSTEM
            Vector2 mousePosition = Mouse.current.position.ReadValue();
#else
            Vector2 mousePosition = Input.mousePosition;
#endif
            Camera camera = canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, camera, out Vector2 localPoint);
            if (localPoint.x > 0.0f)
            {
                tooltip.PivotRight();
            } else
            {
                tooltip.PivotLeft();
            }
            tooltip.SetPosition(localPoint);
        }
    }
}
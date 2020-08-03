using UnityEngine;

namespace Vulpes.Menus
{
    /// <summary>
    /// Transitions the Size Delta of a Rect Transform from one value to another.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Transitions/Transition Size Delta")]
    public sealed class MenuTransition_SizeDelta : MenuTransition<Vector2>
    {
        [SerializeField] private RectTransform rectTransform = default;

        public override Vector2 Current
        {
            get
            {
                return rectTransform.sizeDelta;
            }
        }

        public override void Initialize()
        {
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
        }

        protected override void OnTransitionStart()
        {
            
        }

        protected override void OnTransitionUpdate(float afTime)
        {
            rectTransform.sizeDelta = Vector2.LerpUnclamped(start, end, afTime);
        }

        protected override void OnTransitionEnd()
        {

        }
    }
}

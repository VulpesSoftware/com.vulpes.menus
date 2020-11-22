using UnityEngine;

namespace Vulpes.Menus
{
    /// <summary>
    /// Transitions the Pivot of a <see cref="RectTransform"/> from one position to another.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Transitions/Transition Rect Transform Pivot")]
    public sealed class MenuTransition_RectTransformPivot : MenuTransition<Vector2>
    {
        [SerializeField] private RectTransform rectTransform = default;

        public override Vector2 Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }

        public override Vector2 End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
            }
        }

        public override Vector2 Current
        {
            get
            {
                return rectTransform.pivot;
            }
        }

        public override void Initialize()
        {
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
        }

        protected override void OnTransitionStart() { }

        protected override void OnTransitionUpdate(float afTime)
        {
            rectTransform.pivot = Vector2.LerpUnclamped(start, end, afTime);
        }

        protected override void OnTransitionEnd() { }
    }
}
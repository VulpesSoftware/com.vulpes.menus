using UnityEngine;

namespace Vulpes.Menus
{
    /// <summary>
    /// Transitions the Anchored Position of a Rect Transform from one position to another.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Transitions/Transition Anchored Position")]
    public sealed class MenuTransition_AnchoredPosition : MenuTransition<Vector2>
    {
        [SerializeField] private RectTransform rectTransform = default;
        [SerializeField] private bool useViewportSpace = false;

        private Vector2 initialOffset;
        private Vector2 startPosition;
        private Vector2 endPosition;

        public override Vector2 Start
        {
            get
            {
                return startPosition;
            }
            set
            {
                start = value;
                startPosition = useViewportSpace ? Vector2.Scale(start, new Vector2(Screen.width, Screen.height)) + initialOffset : start;
            }
        }

        public override Vector2 End
        {
            get
            {
                return endPosition;
            }
            set
            {
                end = value;
                endPosition = useViewportSpace ? Vector2.Scale(end, new Vector2(Screen.width, Screen.height)) + initialOffset : end;
            }
        }

        public override Vector2 Current
        {
            get
            {
                return rectTransform.anchoredPosition;
            }
        }

        public override void Initialize()
        {
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
            initialOffset = rectTransform.anchoredPosition;
            startPosition = useViewportSpace ? Vector2.Scale(start, new Vector2(Screen.width, Screen.height)) + initialOffset : start;
            endPosition = useViewportSpace ? Vector2.Scale(end, new Vector2(Screen.width, Screen.height)) + initialOffset : end;
        }

        protected override void OnTransitionStart() { }

        protected override void OnTransitionUpdate(float afTime)
        {
            rectTransform.anchoredPosition = Vector2.LerpUnclamped(startPosition, endPosition, afTime);
        }

        protected override void OnTransitionEnd() { }
    }
}

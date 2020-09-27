using UnityEngine;

namespace Vulpes.Menus
{
    /// <summary>
    /// Transitions the alpha of a <see cref="CanvasGroup"/> from one value to another, 
    /// automatically handling the interactable and blocks raycasts values.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Transitions/Transition Canvas Group Alpha")]
    public sealed class MenuTransition_CanvasGroupAlpha : MenuTransition<float>
    {
        [SerializeField] private CanvasGroup canvasGroup = default;

        public override float Current
        {
            get
            {
                return canvasGroup.alpha;
            }
        }

        public override void Initialize()
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
            if (resetOnInitialize)
            {
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
            } else
            {
                canvasGroup.blocksRaycasts = true;
                canvasGroup.interactable = true;
            }
        }

        protected override void OnTransitionStart()
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = false;
        }

        protected override void OnTransitionUpdate(float afTime)
        {
            canvasGroup.alpha = Mathf.LerpUnclamped(start, end, afTime);
        }

        protected override void OnTransitionEnd()
        {
            canvasGroup.blocksRaycasts = transitionMode == MenuTransitionMode.Forward ? true : false;
            canvasGroup.interactable = transitionMode == MenuTransitionMode.Forward ? true : false;
        }
    }
}

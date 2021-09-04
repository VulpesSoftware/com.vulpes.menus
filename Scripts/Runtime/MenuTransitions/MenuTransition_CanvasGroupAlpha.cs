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
        [SerializeField] private bool interactable = true;
        [SerializeField] private bool blocksRaycasts = true;

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
            if (flags.HasFlag(MenuTransitionFlags.ResetOnInitialize))
            {
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
            } else
            {
                canvasGroup.blocksRaycasts = blocksRaycasts;
                canvasGroup.interactable = interactable;
            }
        }

        protected override void OnTransitionStart()
        {
            canvasGroup.blocksRaycasts = blocksRaycasts;
            canvasGroup.interactable = false;
        }

        protected override void OnTransitionUpdate(in float time)
        {
            canvasGroup.alpha = Mathf.LerpUnclamped(start, end, time);
        }

        protected override void OnTransitionEnd()
        {
            canvasGroup.blocksRaycasts = Mode == MenuTransitionMode.Forward && blocksRaycasts;
            canvasGroup.interactable = Mode == MenuTransitionMode.Forward && interactable;
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Vulpes.Promises;
using Vulpes.Transitions;

namespace Vulpes.Menus
{
    [AddComponentMenu("Vulpes/Menus/Tooltip/Menu Tooltip"), RequireComponent(typeof(CanvasGroup)), DisallowMultipleComponent]
    public sealed class MenuTooltip : UIBehaviour, IMenuTooltip
    {
        [SerializeField] private TextMeshProUGUI titleText = default;
        [SerializeField] private TextMeshProUGUI bodyText = default;
        [SerializeField] private Transition transition = default;
        [SerializeField] private Transition_RectTransformPivot pivotTransition = default;

        private CanvasGroup canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            // Disable raycast target on children.
            Graphic[] graphics = GetComponentsInChildren<Graphic>();
            for (int i = graphics.Length - 1; i >= 0; i--)
            {
                graphics[i].raycastTarget = false;
            }
        }

        private void SetText(in string title, in string body)
        {
            titleText.text = title;
            titleText.gameObject.SetActive(!string.IsNullOrEmpty(title));
            bodyText.text = body;
            bodyText.gameObject.SetActive(!string.IsNullOrEmpty(body));
        }

        public IPromise Show(in string title, in string body)
        {
            SetText(title, body);
            return transition.Play(TransitionMode.Forward);
        }

        public IPromise Hide()
            => transition.Play(TransitionMode.Reverse);

        public void SetPosition(Vector2 position)
            => transform.localPosition = position;

        private bool CanPivot(TransitionMode transitionMode)
            => pivotTransition != null && pivotTransition.Mode != transitionMode;

        private void Pivot(TransitionMode transitionMode)
        {
            if (CanPivot(transitionMode))
            {
                pivotTransition.Play(transitionMode);
            }
        }

        public void PivotLeft()
            => Pivot(TransitionMode.Reverse);

        public void PivotRight()
            => Pivot(TransitionMode.Forward);
    }
}
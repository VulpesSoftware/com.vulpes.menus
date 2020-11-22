using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    [AddComponentMenu("Vulpes/Menus/Menu Tooltip"), RequireComponent(typeof(CanvasGroup))]
    public sealed class MenuTooltip : UIBehaviour, IMenuTooltip
    {
        [SerializeField] private TextMeshProUGUI titleText = default;
        [SerializeField] private TextMeshProUGUI bodyText = default;
        [SerializeField] private MenuTransition transition = default;
        [SerializeField] private MenuTransition_RectTransformPivot pivotTransition = default;

        private CanvasGroup canvasGroup;

        public bool IsActive
        {
            get
            {
                return gameObject.activeSelf;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            // Disable raycast target on image.
            var image = GetComponent<Image>();
            if (image != null)
            {
                image.raycastTarget = false;
            }
        }

        public IPromise Show(string title, string body)
        {
            SetText(title, body);
            return transition.Play(MenuTransitionMode.Forward);
        }

        public IPromise Hide()
        {
            return transition.Play(MenuTransitionMode.Reverse);
        }

        public void SetText(string title, string body)
        {
            titleText.text = title;
            titleText.gameObject.SetActive(!string.IsNullOrEmpty(title));
            bodyText.text = body;
            bodyText.gameObject.SetActive(!string.IsNullOrEmpty(body));
        }

        public void SetPosition(Vector2 position)
        {
            transform.localPosition = position;
        }

        public void PivotLeft()
        {
            if (pivotTransition != null && pivotTransition.Mode != MenuTransitionMode.Reverse)
            {
                pivotTransition.Play(MenuTransitionMode.Reverse);
            }
        }

        public void PivotRight()
        {
            if (pivotTransition != null && pivotTransition.Mode != MenuTransitionMode.Forward)
            {
                pivotTransition.Play(MenuTransitionMode.Forward);
            }
        }
    }
}
using UnityEngine;
using UnityEngine.EventSystems;
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

        private CanvasGroup canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
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
            bodyText.text = body;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
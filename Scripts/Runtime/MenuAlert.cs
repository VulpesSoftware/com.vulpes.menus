using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    [AddComponentMenu("Vulpes/Menus/Menu Alert"), RequireComponent(typeof(CanvasGroup))]
    public sealed class MenuAlert : UIBehaviour, IMenuAlert
    {
        [SerializeField] private TextMeshProUGUI alertText = default;
        [SerializeField] private Image iconImage = default;
        [SerializeField] private MenuTransition transition = default;

        private IPromise promiseChain;
        private PromiseTimer promiseTimer;

        public void UpdateTimer(float afDeltaTime)
        {
            if (promiseTimer == null)
            {
                promiseTimer = new PromiseTimer();
            } else
            {
                promiseTimer.Update(afDeltaTime);
            }
        }

        /// <summary>
        /// Sets the message and icon for the Alert.
        /// </summary>
        private void SetMessageAndIcon(string asMessage, Sprite akIcon)
        {
            alertText.text = asMessage;
            iconImage.sprite = akIcon;
            iconImage.gameObject.SetActive(akIcon != null);
        }

        /// <summary>
        /// Shows an Alert with the specified message and icon and returns 
        /// a Promise that resolves automatically after the specified duration.
        /// </summary>
        public IPromise Show(string asMessage, Sprite akIcon, float afDuration)
        {
            IPromise promise = Promise.Create();
            if (promiseChain == null)
            {
                SetMessageAndIcon(asMessage, akIcon);
                promiseChain = transition.Play(MenuTransitionMode.Forward)
                    .Then(() => promiseTimer.WaitFor(afDuration))
                    .Then(() => transition.Play(MenuTransitionMode.Reverse))
                    .Then(() => promise.Resolve());
            } else
            {
                promiseChain = promiseChain
                    .Then(() => SetMessageAndIcon(asMessage, akIcon))
                    .Then(() => transition.Play(MenuTransitionMode.Forward))
                    .Then(() => promiseTimer.WaitFor(afDuration))
                    .Then(() => transition.Play(MenuTransitionMode.Reverse))
                    .Then(() => promise.Resolve());
            }
            return promise;
        }

        /// <summary>
        /// Shows an Alert with the specified message and icon and returns 
        /// a Promise that resolves one second after the provided Promise resolves.
        /// </summary>
        public IPromise Show(string asMessage, Sprite akIcon, IPromise akPromise)
        {
            IPromise promise = Promise.Create();
            if (promiseChain == null)
            {
                SetMessageAndIcon(asMessage, akIcon);
                promiseChain = transition.Play(MenuTransitionMode.Forward)
                    .Then(() => akPromise)
                    .Then(() => promiseTimer.WaitFor(1.0f))
                    .Then(() => transition.Play(MenuTransitionMode.Reverse))
                    .Then(() => promise.Resolve());
            } else
            {
                promiseChain = promiseChain
                    .Then(() => SetMessageAndIcon(asMessage, akIcon))
                    .Then(() => transition.Play(MenuTransitionMode.Forward))
                    .Then(() => akPromise)
                    .Then(() => promiseTimer.WaitFor(1.0f))
                    .Then(() => transition.Play(MenuTransitionMode.Reverse))
                    .Then(() => promise.Resolve());
            }
            return promise;
        }
    }
}
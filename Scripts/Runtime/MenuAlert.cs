using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    /// <summary>
    /// The <see cref="MenuAlert"/> is a universal, non-intrusive popup that can be shown for 
    /// a specified duration or until a <see cref="Promise"/> resolves.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Menu Alert"), RequireComponent(typeof(CanvasGroup))]
    public class MenuAlert : UIBehaviour, IMenuAlert
    {
        [SerializeField] private TextMeshProUGUI alertText = default;
        [SerializeField] private Image iconImage = default;
        [SerializeField] private MenuTransition transition = default;

        private IPromise promiseChain;
        private PromiseTimer promiseTimer;

        /// <summary>
        /// Updates the <see cref="PromiseTimer"/> and creates it if it doesn't exist.
        /// </summary>
        /// <param name="deltaTime">Current deltaTime used for incrementing the timer.</param>
        public void UpdateTimer(in float deltaTime)
        {
            if (promiseTimer == null)
            {
                promiseTimer = new PromiseTimer();
            } else
            {
                promiseTimer.Update(deltaTime);
            }
        }

        /// <summary>
        /// Sets the message and icon for the Alert.
        /// </summary>
        private void SetMessageAndIcon(string message, Sprite icon)
        {
            alertText.text = message;
            iconImage.sprite = icon;
            iconImage.gameObject.SetActive(icon != null);
        }

        /// <summary>
        /// Shows an Alert with the specified message and icon and returns 
        /// a <see cref="Promise"/> that resolves automatically after the specified duration.
        /// </summary>
        public IPromise Show(string message, Sprite icon, float duration)
        {
            IPromise promise = Promise.Create();
            if (promiseChain == null)
            {
                SetMessageAndIcon(message, icon);
                promiseChain = transition.Play(MenuTransitionMode.Forward)
                    .Then(() => promiseTimer.WaitFor(duration))
                    .Then(() => transition.Play(MenuTransitionMode.Reverse))
                    .Then(() => promise.Resolve());
            } else
            {
                promiseChain = promiseChain
                    .Then(() => SetMessageAndIcon(message, icon))
                    .Then(() => transition.Play(MenuTransitionMode.Forward))
                    .Then(() => promiseTimer.WaitFor(duration))
                    .Then(() => transition.Play(MenuTransitionMode.Reverse))
                    .Then(() => promise.Resolve());
            }
            return promise;
        }

        /// <summary>
        /// Shows an Alert with the specified message and icon and returns 
        /// a Promise that resolves one second after the provided <see cref="Promise"/> resolves.
        /// </summary>
        public IPromise Show(string message, Sprite icon, IPromise onResolved)
        {
            IPromise promise = Promise.Create();
            if (promiseChain == null)
            {
                SetMessageAndIcon(message, icon);
                promiseChain = transition.Play(MenuTransitionMode.Forward)
                    .Then(() => onResolved)
                    .Then(() => promiseTimer.WaitFor(1.0f))
                    .Then(() => transition.Play(MenuTransitionMode.Reverse))
                    .Then(() => promise.Resolve());
            } else
            {
                promiseChain = promiseChain
                    .Then(() => SetMessageAndIcon(message, icon))
                    .Then(() => transition.Play(MenuTransitionMode.Forward))
                    .Then(() => onResolved)
                    .Then(() => promiseTimer.WaitFor(1.0f))
                    .Then(() => transition.Play(MenuTransitionMode.Reverse))
                    .Then(() => promise.Resolve());
            }
            return promise;
        }
    }
}
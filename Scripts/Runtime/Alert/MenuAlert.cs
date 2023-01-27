using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Vulpes.Promises;
using Vulpes.Transitions;

namespace Vulpes.Menus
{
    /// <summary>
    /// The <see cref="MenuAlert"/> is a universal, non-intrusive popup that can be shown for 
    /// a specified duration or until a <see cref="IPromise"/> resolves.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Menu Alert"), RequireComponent(typeof(CanvasGroup)), DisallowMultipleComponent]
    public class MenuAlert : UIBehaviour, IMenuAlert
    {
        [SerializeField] private TextMeshProUGUI alertText = default;
        [SerializeField] private Image iconImage = default;
        [SerializeField] private Transition transition = default;

        private IPromise promiseChain;
        private IPromiseTimer promiseTimer;

        public void Initialize()
            => promiseTimer = new PromiseTimer();

        public void UpdatePromiseTimer(in float deltaTime)
            => promiseTimer.Update(deltaTime);

        private void SetMessageAndIcon(in string message, Sprite icon)
        {
            alertText.text = message;
            iconImage.sprite = icon;
            iconImage.gameObject.SetActive(icon != null);
        }

        public IPromise Show(string message, Sprite icon, float duration)
        {
            IPromise promise = Promise.Create();
            if (promiseChain == null)
            {
                SetMessageAndIcon(message, icon);
                promiseChain = Promise.Sequence(
                        () => transition.Play(TransitionMode.Forward),
                        () => promiseTimer.WaitFor(duration),
                        () => transition.Play(TransitionMode.Reverse))
                    .Then(promise.Resolve);
            } else
            {
                promiseChain = promiseChain
                    .Then(() => SetMessageAndIcon(message, icon))
                    .ThenSequence(
                        () => transition.Play(TransitionMode.Forward),
                        () => promiseTimer.WaitFor(duration),
                        () => transition.Play(TransitionMode.Reverse))
                    .Then(promise.Resolve);
            }
            return promise;
        }

        public IPromise Show(string message, Sprite icon, IPromise promiseToWaitFor)
        {
            IPromise promise = Promise.Create();
            if (promiseChain == null)
            {
                SetMessageAndIcon(message, icon);
                promiseChain = Promise.Sequence(
                        () => transition.Play(TransitionMode.Forward),
                        () => promiseToWaitFor,
                        () => promiseTimer.WaitFor(1.0f),
                        () => transition.Play(TransitionMode.Reverse))
                    .Then(promise.Resolve);
            } else
            {
                promiseChain = promiseChain
                    .Then(() => SetMessageAndIcon(message, icon))
                    .ThenSequence(
                        () => transition.Play(TransitionMode.Forward),
                        () => promiseToWaitFor,
                        () => promiseTimer.WaitFor(1.0f),
                        () => transition.Play(TransitionMode.Reverse))
                    .Then(promise.Resolve);
            }
            return promise;
        }
    }
}
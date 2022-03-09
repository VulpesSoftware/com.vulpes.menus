using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    [AddComponentMenu("Vulpes/Menus/Menu Loading")]
    public class MenuLoading : MenuScreen, IMenuLoading
    {
        // All values on this screen are optional in order to allow it to be easily customised on a per-project basis.

        // Progress
        [SerializeField] private TextMeshProUGUI progressPercentageText = default;
        [SerializeField] private Slider progressBarSlider = default;

        // Spinner
        [SerializeField] private RectTransform spinnerRectTransform = default;
        [SerializeField] private float spinnerSpeed = 360.0f;

        private PromiseTimer promiseTimer;

        public override void OnInitialize()
        {
            base.OnInitialize();
            promiseTimer = new PromiseTimer();
        }

        public override void OnWillAppear()
        {
            base.OnWillAppear();
            SetProgress(0.0f);
        }

        public override void OnWillDisappear()
        {
            base.OnWillDisappear();
            SetProgress(1.0f);
        }

        public override void OnActive()
        {
            base.OnActive();
            if (spinnerRectTransform != null)
            {
                spinnerRectTransform.Rotate(Vector3.forward, Time.unscaledDeltaTime * -spinnerSpeed, Space.Self);
            }
            promiseTimer.Update(Time.unscaledDeltaTime);
        }

        /// <summary>
        /// Sets the progress value for the percentage <see cref="TextMeshProUGUI"/> and progress <see cref="Slider"/> if they are assigned.
        /// </summary>
        public void SetProgress(in float progress)
        {
            if (progressPercentageText != null)
            {
                int progressPercentage = Mathf.RoundToInt(progress * 100.0f);
                progressPercentageText.text = $"{progressPercentage}%";
            }
            if (progressBarSlider != null)
            {
                progressBarSlider.value = progress;
            }
        }

        public IPromise Show(Action loadOperation, Func<bool> completionPredicate, Func<float> progressPredicate)
        {
            IPromise promise = Promise.Create();
            MenuHandler.SetScreenStack(this)
                .Then(loadOperation.Invoke)
                .Then(() => promiseTimer.WaitUntil(t => {
                    SetProgress(progressPredicate.Invoke());
                    return completionPredicate.Invoke(); 
                    }))
                .Done(promise.Resolve);
            return promise;
        }

        public IPromise Show(AsyncOperation asyncOperation)
        {
            IPromise promise = Promise.Create();
            // The AsyncOperation starts immediately and cannot be stopped so we'll stop it to allow the Loading Screen to appear.
            asyncOperation.allowSceneActivation = false;

            MenuHandler.SetScreenStack(this)
                .Then(() => promiseTimer.WaitUntil(ao => {
                    SetProgress(asyncOperation.progress);
                    return asyncOperation.isDone;
                }))
                .Done(promise.Resolve);
            return promise;
        }
    }
}
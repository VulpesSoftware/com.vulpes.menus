using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    [AddComponentMenu("Vulpes/Menus/Screens/Menu Loading")]
    public class MenuLoading : MenuScreen, IMenuLoading
    {
        // All values on this screen are optional in order to allow it to be easily customised on a per-project basis.

        // Progress
        [SerializeField] private TextMeshProUGUI progressPercentageText = default;
        [SerializeField] private Slider progressBarSlider = default;

        // Spinner
        [SerializeField] private RectTransform spinnerRectTransform = default;
        [SerializeField] private float spinnerSpeed = 360.0f;

        private IPromiseTimer promiseTimer;

        public override void OnInitialize()
            => promiseTimer = new PromiseTimer();

        public override void OnWillAppear()
            => SetProgress(0.0f);

        public override void OnWillDisappear()
            => SetProgress(1.0f);

        public override void OnActive()
        {
            if (spinnerRectTransform != null)
            {
                spinnerRectTransform.Rotate(Vector3.forward, Time.unscaledDeltaTime * -spinnerSpeed, Space.Self);
            }
            promiseTimer.Update(Time.unscaledDeltaTime);
        }

        private void SetProgress(in float progress)
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

        public IPromise Show(Action loadOperation, Func<bool> completionPredicate, Func<float> progressPredicate, MenuTransitionOptions options = MenuTransitionOptions.Parallel)
        {
            IPromise promise = Promise.Create();
            MenuHandler.SetScreenStack(this, options)
                .Then(loadOperation.Invoke)
                .Then(() => promiseTimer.WaitUntil(t => {
                    SetProgress(progressPredicate.Invoke());
                    return completionPredicate.Invoke(); 
                }))
                .Done(promise.Resolve);
            return promise;
        }
    }
}
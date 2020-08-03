using UnityEngine;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    /// <summary>
    /// Triggers a series of Menu Transitions all at once.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Transitions/Transition Group"), DefaultExecutionOrder(100)]
    public sealed class MenuTransitionGroup : MenuTransition
    {
        [SerializeField] private MenuTransition[] transitions = default;

        public override float Duration
        {
            get
            {
                duration = 0.0f;
                for (int i = 0; i < transitions.Length; i++)
                {
                    duration = Mathf.Max(duration, transitions[i].TotalDuration);
                }
                return duration;
            }
        }

        public override void Initialize() 
        { 
            if (resetOnInitialize)
            {
                for (int i = 0; i < transitions.Length; i++)
                {
                    transitions[i].ResetOnInitialize = true;
                }
            }
        }

        protected override void OnTransitionStart() { }

        protected override void OnTransitionUpdate(float afTime) { }

        protected override void OnTransitionEnd() { }

        private void StartTransition()
        {
            if (transitionMode == MenuTransitionMode.Forward)
            {
                gameObject.SetActive(true);
            }

            OnTransitionStart();

            int i;

            if (instant)
            {
                for (i = 0; i < transitions.Length; i++)
                {
                    transitions[i].Play(transitionMode, instant);
                }
                OnTransitionUpdate(transitionMode == MenuTransitionMode.Forward ? 1.0f : 0.0f);
                EndTransition();
                return;
            }

            for (i = 0; i < transitions.Length; i++)
            {
                if (transitionMode == MenuTransitionMode.Reverse)
                {
                    transitions[i].Play(transitionMode, instant, instant ? 0.0f : duration - transitions[i].TotalDuration);
                } else
                {
                    transitions[i].Play(transitionMode, instant);
                }
            }

            transitionRoutine = GetMenuTransitionAnchor().StartCoroutine(
                TransitionRoutine(
                    (t) => OnTransitionUpdate(t),
                    duration,
                    Curve,
                    transitionMode == MenuTransitionMode.Reverse,
                    () =>
                    {
                        EndTransition();
                    }
                ));
        }

        private void EndTransition()
        {
            isPlaying = false;
            OnTransitionEnd();
            transitionPromise.Resolve();
            if (transitionMode == MenuTransitionMode.Reverse)
            {
                gameObject.SetActive(false);
            }
        }

        public override IPromise Play(MenuTransitionMode akTransitionMode = MenuTransitionMode.Forward, bool abInstant = false, float? afDelay = null)
        {
            if (isPlaying)
            {
                Complete();
            }

            duration = 0.0f;
            for (int i = 0; i < transitions.Length; i++)
            {
                duration = Mathf.Max(duration, transitions[i].TotalDuration);
            }

            transitionPromise = Promise.Create();
            instant = abInstant;
            transitionMode = akTransitionMode;

            isPlaying = true;

            if (!afDelay.HasValue)
            {
                afDelay = delay;
            }
            if (afDelay > 0.0f)
            {
                GetMenuTransitionAnchor().StartCoroutine(DelayedTransitionRoutine(afDelay.Value, StartTransition));
            } else
            {
                StartTransition();
            }

            return transitionPromise;
        }

        public override void Complete()
        {
            if (isPlaying)
            {
                if (transitionRoutine != null)
                {
                    GetMenuTransitionAnchor().StopCoroutine(transitionRoutine);
                }
                OnTransitionUpdate(transitionMode == MenuTransitionMode.Forward ? 1.0f : 0.0f);
                for (int i = 0; i < transitions.Length; i++)
                {
                    transitions[i].Complete();
                }
                EndTransition();
            }
        }
    }
}

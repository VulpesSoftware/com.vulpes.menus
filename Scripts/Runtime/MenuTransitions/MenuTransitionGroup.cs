using UnityEngine;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    /// <summary>
    /// Triggers a series of <see cref="MenuTransition"/>s all at once.
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
            if (flags.HasFlag(MenuTransitionFlags.ResetOnInitialize))
            {
                for (int i = 0; i < transitions.Length; i++)
                {
                    transitions[i].Flags |= MenuTransitionFlags.ResetOnInitialize;
                }
            }
        }

        protected override void OnTransitionStart() { }

        protected override void OnTransitionUpdate(in float time) { }

        protected override void OnTransitionEnd() { }

        private void StartTransition()
        {
            if (Mode == MenuTransitionMode.Forward)
            {
                gameObject.SetActive(true);
            }

            OnTransitionStart();

            int i;

            if (Instant)
            {
                for (i = 0; i < transitions.Length; i++)
                {
                    transitions[i].Play(Mode, Instant);
                }
                OnTransitionUpdate(Mode == MenuTransitionMode.Forward ? 1.0f : 0.0f);
                EndTransition();
                return;
            }

            for (i = 0; i < transitions.Length; i++)
            {
                if (Mode == MenuTransitionMode.Reverse)
                {
                    transitions[i].Play(Mode, Instant, Instant ? 0.0f : duration - transitions[i].TotalDuration);
                } else
                {
                    transitions[i].Play(Mode, Instant);
                }
            }

            transitionRoutine = GetMenuTransitionAnchor().StartCoroutine(
                TransitionRoutine(
                    (t) => OnTransitionUpdate(t),
                    duration,
                    Curve,
                    Mode == MenuTransitionMode.Reverse,
                    () =>
                    {
                        EndTransition();
                    }
                ));
        }

        private void EndTransition()
        {
            IsPlaying = false;
            OnTransitionEnd();
            transitionPromise.Resolve();
            if (Mode == MenuTransitionMode.Reverse)
            {
                gameObject.SetActive(false);
            }
        }

        public override IPromise Play(in MenuTransitionMode mode = MenuTransitionMode.Forward, in bool instant = false, in float? delayOverride = null)
        {
            if (IsPlaying)
            {
                Complete();
            }

            duration = 0.0f;
            for (int i = 0; i < transitions.Length; i++)
            {
                duration = Mathf.Max(duration, transitions[i].TotalDuration);
            }

            transitionPromise = Promise.Create();
            Instant = instant;
            Mode = mode;

            IsPlaying = true;

            if (delayOverride.GetValueOrDefault() > 0.0f || delay > 0.0f)
            {
                GetMenuTransitionAnchor().StartCoroutine(DelayedTransitionRoutine(delayOverride ?? delay, StartTransition));
            } else
            {
                StartTransition();
            }

            return transitionPromise;
        }

        public override void Complete()
        {
            if (IsPlaying)
            {
                if (transitionRoutine != null)
                {
                    GetMenuTransitionAnchor().StopCoroutine(transitionRoutine);
                }
                OnTransitionUpdate(Mode == MenuTransitionMode.Forward ? 1.0f : 0.0f);
                for (int i = 0; i < transitions.Length; i++)
                {
                    transitions[i].Complete();
                }
                EndTransition();
            }
        }
    }
}

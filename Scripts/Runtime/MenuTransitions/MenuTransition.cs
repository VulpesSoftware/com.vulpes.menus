using System;
using System.Collections;
using UnityEngine;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    /// <summary>
    /// Base class for all <see cref="MenuTransition"/>s.
    /// </summary>
    public abstract class MenuTransition : MonoBehaviour, IMenuTransition
    {
        /// <summary>
        /// Dummy Component used for anchoring Coroutines to a static scene object.
        /// </summary>
        public sealed class MenuTransitionAnchor : MonoBehaviour { }

        private static MenuTransitionAnchor menuTransitionAnchor;

        public static MenuTransitionAnchor GetMenuTransitionAnchor()
        {
            if (menuTransitionAnchor != null)
            {
                return menuTransitionAnchor;
            }

            menuTransitionAnchor = FindObjectOfType<MenuTransitionAnchor>();

            if (menuTransitionAnchor != null)
            {
                return menuTransitionAnchor;
            }

            GameObject menuTransitionAnchorObject = new GameObject("MenuTransitionAnchor", typeof(MenuTransitionAnchor));
            menuTransitionAnchorObject.hideFlags |= HideFlags.HideInHierarchy | HideFlags.HideInInspector;
            DontDestroyOnLoad(menuTransitionAnchorObject);
            menuTransitionAnchor = menuTransitionAnchorObject.GetComponent<MenuTransitionAnchor>();
            return menuTransitionAnchor;
        }

        [SerializeField, Min(0.0f)] protected float duration = 1.0f;
        [SerializeField, Min(0.0f)] protected float delay = 0.0f;
        [SerializeField] protected AnimationCurve forwardCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        [SerializeField] protected AnimationCurve reverseCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        [SerializeField] protected MenuTransitionFlags flags = MenuTransitionFlags.Everything;

        protected Coroutine transitionRoutine;
        protected IPromise transitionPromise;

        public float CurrentTime { get; protected set; }

        public bool Instant { get; protected set; }

        public MenuTransitionMode Mode { get; protected set; }

        public AnimationCurve Curve => Mode == MenuTransitionMode.Forward ? forwardCurve : reverseCurve;

        public bool IsPlaying { get; protected set; }

        public virtual float Duration
        {
            get => duration;
            set => duration = value;
        }

        public float Delay
        {
            get => delay;
            set => delay = value;
        }

        public float TotalDuration => Delay + Duration;

        public MenuTransitionFlags Flags
        {
            get => flags;
            set => flags = value;
        }

        protected IEnumerator TransitionRoutine(Action<float> action, float duration, AnimationCurve curve, bool reversed, Action onComplete = null)
        {
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                action(curve.Evaluate(reversed ? 1.0f - t : t));
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            action(curve.Evaluate(reversed ? 0.0f : 1.0f));

            onComplete?.Invoke();
        }

        protected IEnumerator TransitionRoutine(Action<float> action, float duration, float elapsed, AnimationCurve curve, bool reversed, Action onComplete = null)
        {
            if (reversed)
            {
                elapsed = 1.0f - elapsed;
            }
            
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                action(curve.Evaluate(reversed ? 1.0f - t : t));
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            
            action(curve.Evaluate(reversed ? 0.0f : 1.0f));

            onComplete?.Invoke();
        }

        protected void Awake()
        {
            Initialize();
            if (flags.HasFlag(MenuTransitionFlags.ResetOnInitialize))
            {
                OnTransitionUpdate(0.0f);
                if (flags.HasFlag(MenuTransitionFlags.DisableWhenDone))
                {
                    gameObject.SetActive(false);
                }
            } else
            {
                if (flags.HasFlag(MenuTransitionFlags.DisableWhenDone))
                {
                    gameObject.SetActive(true);
                }
                OnTransitionUpdate(1.0f);
            }
        }

        public abstract void Initialize();

        protected abstract void OnTransitionStart();

        protected abstract void OnTransitionUpdate(in float time);

        protected abstract void OnTransitionEnd();

        /// <summary>
        /// Plays the <see cref="MenuTransition"/> in the specified direction and returns a <see cref="Promise"/> 
        /// that will resolve once the <see cref="MenuTransition"/> is complete.
        /// </summary>
        public virtual IPromise Play(in MenuTransitionMode mode = MenuTransitionMode.Forward, in bool instant = false, float? delayOverride = null)
        {
            if (IsPlaying)
            {
                if (Mode == mode)
                {
                    if (transitionPromise == null)
                    {
                        return Promise.Resolved();
                    } else
                    {
                        return transitionPromise;
                    }
                }
                float t = CurrentTime;
                Complete();
                CurrentTime = t;
            }

            IsPlaying = true;

            transitionPromise = Promise.Create();
            Instant = instant;
            Mode = mode;

            if ((delayOverride.GetValueOrDefault() > 0.0f || delay > 0.0f) && !instant)
            {
                if (flags.HasFlag(MenuTransitionFlags.ResetOnPlay))
                {
                    OnTransitionUpdate(Mode == MenuTransitionMode.Forward ? 0.0f : 1.0f);
                }
                transitionRoutine = GetMenuTransitionAnchor().StartCoroutine(DelayedTransitionRoutine(delayOverride ?? delay, StartTransition));
            } else
            {
                StartTransition();
            }

            return transitionPromise;
        }

        protected IEnumerator DelayedTransitionRoutine(float delay, Action onComplete)
        {
            if (delay > 0.0f)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            onComplete?.Invoke();
        }

        private void StartTransition()
        {
            if (flags.HasFlag(MenuTransitionFlags.DisableWhenDone) && Mode == MenuTransitionMode.Forward)
            {
                gameObject.SetActive(true);
            }

            if (flags.HasFlag(MenuTransitionFlags.ResetOnPlay))
            {
                CurrentTime = Mode == MenuTransitionMode.Forward ? 0.0f : 1.0f;
            }

            OnTransitionStart();

            if (Instant)
            {
                OnTransitionUpdate(Mode == MenuTransitionMode.Forward ? 1.0f : 0.0f);
                EndTransition();
                return;
            }

            transitionRoutine = GetMenuTransitionAnchor().StartCoroutine(
                TransitionRoutine(
                    (t) =>
                    {
                        CurrentTime = t;
                        OnTransitionUpdate(t);
                    },
                    duration,
                    CurrentTime, 
                    Curve,
                    Mode == MenuTransitionMode.Reverse,
                    EndTransition
                ));
        }

        private void EndTransition()
        {
            IsPlaying = false;
            CurrentTime = Mode == MenuTransitionMode.Forward ? 1.0f : 0.0f;
            OnTransitionEnd();
            transitionPromise.Resolve();

            if (flags.HasFlag(MenuTransitionFlags.DisableWhenDone) && Mode == MenuTransitionMode.Reverse)
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Completes the active <see cref="MenuTransition"/> immediately and resolves the pending <see cref="Promise"/>.
        /// </summary>
        public virtual void Complete()
        {
            if (IsPlaying)
            {
                if (transitionRoutine != null)
                {
                    GetMenuTransitionAnchor().StopCoroutine(transitionRoutine);
                }
                OnTransitionUpdate(Mode == MenuTransitionMode.Forward ? 1.0f : 0.0f);
                EndTransition();
            }
        }

        /// <summary>
        /// Sets the current time of the <see cref="MenuTransition"/>.
        /// </summary>
        public void SetTime(in float time, in MenuTransitionMode mode = MenuTransitionMode.Forward)
        {
            CurrentTime = mode == MenuTransitionMode.Reverse ? 1.0f - time : time;
            OnTransitionUpdate(time);
        }
    }

    /// <summary>
    /// Base class for all <see cref="MenuTransition"/>s that require a value.
    /// </summary>
    public abstract class MenuTransition<T> : MenuTransition where T : IEquatable<T>
    {
        [SerializeField] protected T start = default;
        [SerializeField] protected T end = default;

        public virtual T Start
        {
            get => start;
            set => start = value;
        }

        public virtual T End
        {
            get => end;
            set => end = value;
        }

        public abstract T Current { get; }
    }
}

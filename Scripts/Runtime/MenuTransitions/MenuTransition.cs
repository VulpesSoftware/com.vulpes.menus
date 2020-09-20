using System;
using System.Collections;
using UnityEngine;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    /// <summary>
    /// Base class for all <b>Menu Transitions<\b>.
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
        [SerializeField] protected AnimationCurve forwardCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));
        [SerializeField] protected AnimationCurve reverseCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));
        [SerializeField, Tooltip("Reset the transition to the begining at startup?")] protected bool resetOnInitialize = true;
        [SerializeField, Tooltip("Reset the transition before playing?")] protected bool resetOnPlay = true;
        [SerializeField, Tooltip("Disable the GameObject when transitioned out (good for performance)?")] protected bool disableWhenDone = true;

        protected MenuTransitionMode transitionMode;
        protected Coroutine transitionRoutine;
        protected IPromise transitionPromise;
        protected float currentTime;
        protected bool isPlaying;
        protected bool instant;

        public MenuTransitionMode Mode
        {
            get
            {
                return transitionMode;
            }
        }

        protected AnimationCurve Curve
        {
            get
            {
                return transitionMode == MenuTransitionMode.Forward ? forwardCurve : reverseCurve;
            }
        }

        public bool IsPlaying
        {
            get
            {
                return isPlaying;
            }
        }

        public float CurrentTime
        {
            get
            {
                return currentTime;
            }
        }

        public virtual float Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
            }
        }

        public float Delay
        {
            get
            {
                return delay;
            }
            set
            {
                delay = value;
            }
        }

        public float TotalDuration
        {
            get
            {
                return Delay + Duration;
            }
        }

        public bool ResetOnInitialize
        {
            get
            {
                return resetOnInitialize;
            }
            set
            {
                resetOnInitialize = value;
            }
        }

        public bool ResetOnPlay
        {
            get
            {
                return resetOnPlay;
            }
        }

        public bool DisableWhenDone
        {
            get
            {
                return disableWhenDone;
            }
        }

        protected IEnumerator TransitionRoutine(Action<float> akAction, float afDuration, AnimationCurve akCurve, bool abReversed, Action akOnComplete = null)
        {
            float elapsed = 0.0f;

            while (elapsed < afDuration)
            {
                float t = elapsed / afDuration;
                akAction(akCurve.Evaluate(abReversed ? 1.0f - t : t));
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            akAction(akCurve.Evaluate(abReversed ? 0.0f : 1.0f));

            akOnComplete?.Invoke();
        }

        protected IEnumerator TransitionRoutine(Action<float> akAction, float afDuration, float afElapsed, AnimationCurve akCurve, bool abReversed, Action akOnComplete = null)
        {
            if (abReversed)
            {
                afElapsed = 1.0f - afElapsed;
            }
            
            while (afElapsed < afDuration)
            {
                float t = afElapsed / afDuration;
                akAction(akCurve.Evaluate(abReversed ? 1.0f - t : t));
                afElapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            
            akAction(akCurve.Evaluate(abReversed ? 0.0f : 1.0f));

            akOnComplete?.Invoke();
        }

        protected void Awake()
        {
            Initialize();
            if (resetOnInitialize)
            {
                OnTransitionUpdate(0.0f);
                if (disableWhenDone)
                {
                    gameObject.SetActive(false);
                }
            } else
            {
                if (disableWhenDone)
                {
                    gameObject.SetActive(true);
                }
                OnTransitionUpdate(1.0f);
            }
        }

        public abstract void Initialize();

        protected abstract void OnTransitionStart();

        protected abstract void OnTransitionUpdate(float afTime);

        protected abstract void OnTransitionEnd();

        /// <summary>
        /// Plays the transition in the specified direction and returns a Promise 
        /// that will resolve once the transition is complete.
        /// </summary>
        public virtual IPromise Play(MenuTransitionMode akTransitionMode = MenuTransitionMode.Forward, bool abInstant = false, float? afDelay = null)
        {
            if (isPlaying)
            {
                if (transitionMode == akTransitionMode)
                {
                    if (transitionPromise == null)
                    {
                        return Promise.Resolved();
                    } else
                    {
                        return transitionPromise;
                    }
                }
                float t = currentTime;
                Complete();
                currentTime = t;
            }

            isPlaying = true;

            transitionPromise = Promise.Create();
            instant = abInstant;
            transitionMode = akTransitionMode;

            if (!afDelay.HasValue)
            {
                afDelay = delay;
            }
            if (afDelay > 0.0f && !abInstant)
            {
                if (resetOnPlay)
                {
                    OnTransitionUpdate(transitionMode == MenuTransitionMode.Forward ? 0.0f : 1.0f);
                }
                transitionRoutine = GetMenuTransitionAnchor().StartCoroutine(DelayedTransitionRoutine(afDelay.Value, StartTransition));
            } else
            {
                StartTransition();
            }

            return transitionPromise;
        }

        protected IEnumerator DelayedTransitionRoutine(float afDelay, Action akOnComplete)
        {
            if (afDelay > 0.0f)
            {
                yield return new WaitForSecondsRealtime(afDelay);
            }
            akOnComplete?.Invoke();
        }

        private void StartTransition()
        {
            if (disableWhenDone && transitionMode == MenuTransitionMode.Forward)
            {
                gameObject.SetActive(true);
            }

            if (resetOnPlay)
            {
                currentTime = transitionMode == MenuTransitionMode.Forward ? 0.0f : 1.0f;
            }

            OnTransitionStart();

            if (instant)
            {
                OnTransitionUpdate(transitionMode == MenuTransitionMode.Forward ? 1.0f : 0.0f);
                EndTransition();
                return;
            }

            transitionRoutine = GetMenuTransitionAnchor().StartCoroutine(
                TransitionRoutine(
                    (t) =>
                    {
                        currentTime = t;
                        OnTransitionUpdate(t);
                    },
                    duration,
                    currentTime, 
                    Curve,
                    transitionMode == MenuTransitionMode.Reverse,
                    EndTransition
                ));
        }

        private void EndTransition()
        {
            isPlaying = false;
            currentTime = transitionMode == MenuTransitionMode.Forward ? 1.0f : 0.0f;
            OnTransitionEnd();
            transitionPromise.Resolve();

            if (disableWhenDone && transitionMode == MenuTransitionMode.Reverse)
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Completes the active transition immediately and resolves the pending Promise.
        /// </summary>
        public virtual void Complete()
        {
            if (isPlaying)
            {
                if (transitionRoutine != null)
                {
                    GetMenuTransitionAnchor().StopCoroutine(transitionRoutine);
                }
                OnTransitionUpdate(transitionMode == MenuTransitionMode.Forward ? 1.0f : 0.0f);
                EndTransition();
            }
        }

        /// <summary>
        /// Sets the current time of the transition.
        /// </summary>
        public void SetTime(float afTime, MenuTransitionMode akMode = MenuTransitionMode.Forward)
        {
            if (akMode == MenuTransitionMode.Reverse)
            {
                afTime = 1.0f - afTime;
            }
            currentTime = afTime;
            OnTransitionUpdate(afTime);
        }
    }

    /// <summary>
    /// Base class for all Menu Transitions.
    /// </summary>
    public abstract class MenuTransition<T> : MenuTransition where T : IEquatable<T>
    {
        [SerializeField] protected T start = default;
        [SerializeField] protected T end = default;

        public virtual T Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }

        public virtual T End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
            }
        }

        public abstract T Current { get; }
    }
}

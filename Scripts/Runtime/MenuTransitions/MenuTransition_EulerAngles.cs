using UnityEngine;

namespace Vulpes.Menus
{
    /// <summary>
    /// Transitions the Euler Angles of a Transform from one orientation to another.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Transitions/Transition Euler Angles")]
    public sealed class MenuTransition_EulerAngles : MenuTransition<Vector3>
    {
        [SerializeField] private Transform targetTransform = default;
        [SerializeField] private bool useLocalSpace = false;

        public override Vector3 Current
        {
            get
            {
                return useLocalSpace ? transform.localEulerAngles : targetTransform.eulerAngles;
            }
        }

        public override void Initialize()
        {
            if (targetTransform == null)
            {
                targetTransform = transform;
            }
        }

        protected override void OnTransitionStart() { }

        protected override void OnTransitionUpdate(float afTime)
        {
            if (useLocalSpace)
            {
                targetTransform.localEulerAngles = Vector2.LerpUnclamped(start, end, afTime);
            } else
            {
                targetTransform.eulerAngles = Vector2.LerpUnclamped(start, end, afTime);
            }
        }

        protected override void OnTransitionEnd() { }
    }
}

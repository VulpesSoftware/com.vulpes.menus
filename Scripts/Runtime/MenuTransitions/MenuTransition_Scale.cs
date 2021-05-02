using UnityEngine;

namespace Vulpes.Menus
{
    /// <summary>
    /// Transitions the scale of a <see cref="Transform"/> from one scale to another.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Transitions/Transition Scale")]
    public sealed class MenuTransition_Scale : MenuTransition<Vector3>
    {
        [SerializeField] private Transform targetTransform = default;

        public override Vector3 Current
        {
            get
            {
                return targetTransform.localScale;
            }
        }

        public override void Initialize()
        {
            if (targetTransform == null)
            {
                targetTransform = transform;
            }
        }

        protected override void OnTransitionStart()
        {

        }

        protected override void OnTransitionUpdate(in float time)
        {
            targetTransform.localScale = Vector3.LerpUnclamped(start, end, time);
        }

        protected override void OnTransitionEnd()
        {

        }
    }
}

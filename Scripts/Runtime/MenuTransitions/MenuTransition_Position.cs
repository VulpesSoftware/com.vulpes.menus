using UnityEngine;

namespace Vulpes.Menus
{
    /// <summary>
    /// Transitions the Position of a Transform from one position to another.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Transitions/Transition Position")]
    public sealed class MenuTransition_Position : MenuTransition<Vector3>
    {
        [SerializeField] private Transform targetTransform = default;
        [SerializeField] private bool useLocalSpace = false;

        public override Vector3 Current
        {
            get
            {
                return useLocalSpace ? transform.localPosition : targetTransform.position;
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

        protected override void OnTransitionUpdate(float afTime)
        {
            if (useLocalSpace)
            {
                targetTransform.localPosition = Vector2.LerpUnclamped(start, end, afTime);
            } else
            {
                targetTransform.position = Vector2.LerpUnclamped(start, end, afTime);
            }
        }

        protected override void OnTransitionEnd()
        {

        }
    }
}

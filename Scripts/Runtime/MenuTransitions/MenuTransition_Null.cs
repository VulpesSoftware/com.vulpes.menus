using UnityEngine;

namespace Vulpes.Menus
{
    /// <summary>
    /// Transitions nothing but the delay and duration values are still used in Group Transitions.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Transitions/Transition Null")]
    public sealed class MenuTransition_Null : MenuTransition
    {
        public override void Initialize() { }

        protected override void OnTransitionStart() { }

        protected override void OnTransitionUpdate(float afTime) { }

        protected override void OnTransitionEnd() { }
    }
}

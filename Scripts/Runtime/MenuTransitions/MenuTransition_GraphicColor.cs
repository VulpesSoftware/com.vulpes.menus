using UnityEngine;
using UnityEngine.UI;

namespace Vulpes.Menus
{
    /// <summary>
    /// Transitions the <see cref="Color"/> of a <see cref="Graphic"/> from one <see cref="Color"/> to another using a <see cref="Gradient"/>.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Transitions/Transition Graphic Color")]
    public sealed class MenuTransition_GraphicColor : MenuTransition
    {
        [SerializeField] private Graphic targetGraphic = default;
        [SerializeField] private Gradient color = new Gradient();

        public Color Current
        {
            get
            {
                return targetGraphic.color;
            }
        }

        public override void Initialize()
        {
            if (targetGraphic == null)
            {
                targetGraphic = GetComponent<Graphic>();
            }
        }

        protected override void OnTransitionStart()
        {

        }

        protected override void OnTransitionUpdate(float afTime)
        {
            targetGraphic.color = color.Evaluate(afTime);
        }

        protected override void OnTransitionEnd()
        {

        }
    }
}

﻿using UnityEngine;

namespace Vulpes.Menus
{
    /// <summary>
    /// Transitions the size delta of a <see cref="RectTransform"/> from one value to another.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Transitions/Transition Size Delta")]
    public sealed class MenuTransition_SizeDelta : MenuTransition<Vector2>
    {
        [SerializeField] private RectTransform rectTransform = default;

        public override Vector2 Current
        {
            get
            {
                return rectTransform.sizeDelta;
            }
        }

        public override void Initialize()
        {
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
        }

        protected override void OnTransitionStart()
        {
            
        }

        protected override void OnTransitionUpdate(in float time)
        {
            rectTransform.sizeDelta = Vector2.LerpUnclamped(start, end, time);
        }

        protected override void OnTransitionEnd()
        {

        }
    }
}

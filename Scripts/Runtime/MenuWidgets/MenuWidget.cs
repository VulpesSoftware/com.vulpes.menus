using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Vulpes.Menus.Experimental
{
    /// <summary>
    /// Base class for all Menu Widgets.
    /// Note: This is an experimental feature use it at your own peril.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class MenuWidget : Selectable, IPointerClickHandler, ISubmitHandler
    {
        [NonSerialized] public bool valueChanged;

        private ScrollRect scrollRect;

        protected override void Awake()
        {
            base.Awake();
            scrollRect = GetComponentInParent<ScrollRect>();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            // TODO Fire an event here.
            if (scrollRect != null)
            {
                // FIXME This is for automatically scrolling the scroll view up and down, need to make sure it's working as intended.
                // TODO Adding left and right scrolling might aslo be nice.
                float contentHeight = scrollRect.content.rect.height;
                float viewportHeight = scrollRect.viewport.rect.height;
                float centerLine = eventData.selectedObject.transform.localPosition.y;
                float rectHeight = eventData.selectedObject.GetComponent<RectTransform>().rect.height;
                float upperBound = centerLine + (rectHeight * 1.0f);
                float lowerBound = centerLine - (rectHeight * 1.0f);
                float lowerVisible = (contentHeight - viewportHeight) * scrollRect.normalizedPosition.y - contentHeight;
                float upperVisible = lowerVisible + viewportHeight;
                float desiredLowerBound;
                if (upperBound > upperVisible)
                {
                    desiredLowerBound = upperBound - viewportHeight + (rectHeight * 1.0f);
                } else if (lowerBound < lowerVisible)
                {
                    desiredLowerBound = lowerBound - (rectHeight * 1.0f);
                } else
                {
                    return;
                }
                float normalizedDesired = (desiredLowerBound + contentHeight) / (contentHeight - viewportHeight);
                scrollRect.normalizedPosition = new Vector2(0.0f, Mathf.Clamp01(normalizedDesired));
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            // TODO Fire an event here.
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            // TODO Fire an event here.
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            // TODO Fire an event here.
        }

        protected virtual void OnValueChanged()
        {
            valueChanged = true;
            // TODO Fire an event here.
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Vulpes.Menus
{
    /// <summary>
    /// Base class for all <see cref="MenuWidget"/>.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class MenuWidget : Selectable, ISubmitHandler
    {
        private ScrollRect scrollRect;

        protected override void Awake()
        {
            base.Awake();
            scrollRect = GetComponentInParent<ScrollRect>();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            if (scrollRect != null)
            {
                // FIXME This is for automatically scrolling the scroll view up and down, need to make sure it's working as intended.
                // TODO Adding left and right scrolling might also be nice.
                float contentHeight = scrollRect.content.rect.height;
                float viewportHeight = scrollRect.viewport.rect.height;
                float centerLineY = eventData.selectedObject.transform.localPosition.y;
                RectTransform rectTransform = eventData.selectedObject.GetComponent<RectTransform>();
                float rectHeight = rectTransform.rect.height;
                float lowerBound = centerLineY - (rectHeight * 1.0f);
                float upperBound = centerLineY + (rectHeight * 1.0f);
                float lowerVisible = (contentHeight - viewportHeight) * scrollRect.normalizedPosition.y - contentHeight;
                float upperVisible = lowerVisible + viewportHeight;
                float desiredBoundY;
                if (upperBound > upperVisible)
                {
                    desiredBoundY = upperBound - viewportHeight + (rectHeight * 1.0f);
                } else if (lowerBound < lowerVisible)
                {
                    desiredBoundY = lowerBound - (rectHeight * 1.0f);
                } else
                {
                    return;
                }
                float normalizedDesiredY = (desiredBoundY + contentHeight) / (contentHeight - viewportHeight);
                scrollRect.normalizedPosition = new(0.0f, Mathf.Clamp01(normalizedDesiredY));
            }
        }

        public virtual void OnSubmit(BaseEventData eventData) { }
    }

    /// <summary>
    /// Base class for all <see cref="MenuWidget"/>s that control a value.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class MenuWidget<T> : MenuWidget 
        where T : IComparable
    {
        [SerializeField, Tooltip("The value of the Widget.")] 
        protected T value = default;

        public UnityEvent<T> onValueChanged = new();

        public virtual T Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged(this.value);
            }
        }

        protected virtual void OnValueChanged(T newValue)
        {
            onValueChanged.Invoke(newValue);
            Select();
        }
    }
}
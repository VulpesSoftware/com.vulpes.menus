using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vulpes.Menus.Experimental
{
    /// <summary>
    /// Menu Widget Button.
    /// Note: This is an experimental feature use it at your own peril.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Widgets/Button")]
    public sealed class MenuWidget_Button : MenuWidget
    {
        [SerializeField] private TextMeshProUGUI headerText = default;

        public event Action OnClickEvent = default;

        public void Initialize(string asHeader, Action akOnClick)
        {
            headerText.text = asHeader.ToUpper();
            OnClickEvent -= akOnClick;
            OnClickEvent += akOnClick;
        }

        public override void OnMove(AxisEventData eventData)
        {
            switch (eventData.moveDir)
            {
                case MoveDirection.Left:
                    break;
                case MoveDirection.Up:
                    base.OnMove(eventData);
                    break;
                case MoveDirection.Right:
                    break;
                case MoveDirection.Down:
                    base.OnMove(eventData);
                    break;
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            OnClickEvent?.Invoke();
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            OnClickEvent?.Invoke();
        }
    }
}
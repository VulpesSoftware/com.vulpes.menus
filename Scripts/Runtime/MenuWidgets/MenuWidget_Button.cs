using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vulpes.Menus
{
    /// <summary>
    /// <see cref="MenuWidget"/> Button.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Widgets/Button")]
    public sealed class MenuWidget_Button : MenuWidget
    {
        [SerializeField] private TextMeshProUGUI headerText = default;

        public event Action OnClickEvent = default;

        public void Initialize(string header, Action onClick)
        {
            headerText.text = header;
            OnClickEvent -= onClick;
            OnClickEvent += onClick;
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
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Vulpes.Menus
{
    /// <summary>
    /// <see cref="MenuWidget"/> Button.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Widgets/Button")]
    public sealed class MenuWidget_Button : MenuWidget, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI headerText = default;

        public UnityEvent onClick = new();

        public void Initialize(string header)
            => headerText.text = header;

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

        public void OnPointerClick(PointerEventData eventData)
            => onClick.Invoke();

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            onClick.Invoke();
        }
    }
}
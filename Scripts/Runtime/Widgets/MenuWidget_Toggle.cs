using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Vulpes.Menus
{
    /// <summary>
    /// <see cref="MenuWidget"/> Toggle.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Widgets/Toggle")]
    public sealed class MenuWidget_Toggle : MenuWidget<bool>, IPointerClickHandler
    {
        [SerializeField] private Graphic checkGraphic = default;

        [SerializeField] private TextMeshProUGUI headerText = default;

        public void Initialize(string header, bool value)
        {
            headerText.text = header;
            this.value = value;
        }

        protected override void Awake()
        {
            base.Awake();
            if (Application.isPlaying)
            {
                checkGraphic.enabled = Value;
            }
        }

        private void Toggle()
        {
            Value = !Value;
            checkGraphic.enabled = Value;
        }

        public void OnPointerClick(PointerEventData eventData)
            => Toggle();

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            Toggle();
        }
    }
}
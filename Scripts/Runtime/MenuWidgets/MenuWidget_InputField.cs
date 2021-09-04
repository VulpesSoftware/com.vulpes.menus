using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vulpes.Menus
{
    /// <summary>
    /// <see cref="MenuWidget"/> Input Field.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Widgets/Input Field")]
    public sealed class MenuWidget_InputField : MenuWidget<string>
    {
        [SerializeField] private TextMeshProUGUI headerText = default;
        [SerializeField] private TMP_InputField inputField = default;

        public override string Value
        {
            get
            {
                return inputField.text;
            }
            set
            {
                inputField.text = value;
                OnValueChanged(value);
            }
        }

        public void Initialize(string header, string defaultInput)
        {
            headerText.text = header;
            inputField.text = defaultInput;
        }

        protected override void Awake()
        {
            base.Awake();
            if (Application.isPlaying)
            {
                inputField.onSubmit.RemoveAllListeners();
                inputField.onSubmit.AddListener(OnValueChanged);
            }
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            inputField.Select();
            inputField.ActivateInputField();
        }

        protected override void OnValueChanged(string newValue)
        {
            Select();
            base.OnValueChanged(newValue);
        }
    }
}

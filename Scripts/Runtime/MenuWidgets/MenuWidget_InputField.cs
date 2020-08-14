using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vulpes.Menus.Experimental
{
    /// <summary>
    /// Menu Widget Input Field.
    /// Note: This is an experimental feature use it at your own peril.
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

        public void Initialize(string asHeader, string asDefault)
        {
            headerText.text = asHeader;
            inputField.text = asDefault;
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

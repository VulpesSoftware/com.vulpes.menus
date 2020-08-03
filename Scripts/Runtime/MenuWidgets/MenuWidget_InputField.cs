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
    public sealed class MenuWidget_InputField : MenuWidget
    {
        [SerializeField] private TextMeshProUGUI headerText = default;

        [SerializeField] private TMP_InputField inputField = default;

        public string Value
        {
            get
            {
                return inputField.text;
            }
            set
            {
                inputField.text = value;
            }
        }

        public void Initialize(string asHeader, string asDefault)
        {
            headerText.text = asHeader.ToUpper();
            inputField.text = asDefault;
        }

        protected override void Awake()
        {
            base.Awake();
            if (Application.isPlaying)
            {
                inputField.onSubmit.RemoveAllListeners();
                inputField.onSubmit.AddListener(OnInputFieldSubmit);
            }
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            inputField.Select();
            inputField.ActivateInputField();
        }

        private void OnInputFieldSubmit(string asText)
        {
            Select();
            OnValueChanged();
        }
    }
}

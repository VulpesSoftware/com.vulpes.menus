using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Vulpes.Menus.Experimental
{
    /// <summary>
    /// Menu Widget Slider.
    /// Note: This is an experimental feature use it at your own peril.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Widgets/Slider")]
    public sealed class MenuWidget_Slider : MenuWidget<int>
    {
        [SerializeField, Tooltip("The minimum value of the slider.")] 
        private int minValue = 0;
        [SerializeField, Tooltip("The maximum value of the slider.")] 
        private int maxValue = 100;
        [SerializeField, Tooltip("The value to increment / decrement the slider by when pressing buttons.")] 
        private int stepValue = 5;

        [SerializeField] private TextMeshProUGUI headerText = default;
        [SerializeField] private Button previousButton = default;
        [SerializeField] private Button nextButton = default;
        [SerializeField] private Slider slider = default;
        [SerializeField] private TMP_InputField inputField = default;

        public void Initialize(string asHeader, int aiValue, int aiMinValue, int aiMaxValue, int aiStepValue)
        {
            headerText.text = asHeader;
            value = aiValue;
            minValue = aiMinValue;
            maxValue = aiMaxValue;
            stepValue = aiStepValue;
        }

        protected override void Awake()
        {
            base.Awake();
            if (Application.isPlaying)
            {
                // Set values for the slider.
                slider.minValue = minValue;
                slider.maxValue = maxValue;
                slider.value = value;
                slider.wholeNumbers = true;
                slider.onValueChanged.AddListener(OnSliderValueChanged);
                // Now handle the input field.
                inputField.onSubmit.RemoveAllListeners();
                inputField.onSubmit.AddListener(OnInputFieldSubmit);
                inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
                inputField.inputType = TMP_InputField.InputType.Standard;
                inputField.text = value.ToString();
                inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
                // Lastly map the buttons.
                previousButton.onClick.AddListener(OnPreviousButtonClick);
                nextButton.onClick.AddListener(OnNextButtonClick);
            }
        }

        public override void OnMove(AxisEventData eventData)
        {
            switch (eventData.moveDir)
            {
                case MoveDirection.Left:
                    OnPreviousButtonClick();
                    break;
                case MoveDirection.Up:
                    base.OnMove(eventData);
                    break;
                case MoveDirection.Right:
                    OnNextButtonClick();
                    break;
                case MoveDirection.Down:
                    base.OnMove(eventData);
                    break;
            }
        }

        private void OnPreviousButtonClick()
        {
            value = Mathf.Max(minValue, value - stepValue);
            OnValueChanged(value);
        }

        private void OnNextButtonClick()
        {
            value = Mathf.Min(maxValue, value + stepValue);
            OnValueChanged(value);
        }

        private void OnInputFieldEndEdit(string asText)
        {
            if (int.TryParse(asText, out int newValue))
            {
                newValue = Mathf.Clamp(newValue, minValue, maxValue);
                value = newValue;
            }
            OnValueChanged(value);
        }

        private void OnSliderValueChanged(float afValue)
        {
            value = Mathf.Clamp((int)afValue, minValue, maxValue);
            OnValueChanged(value);
        }

        protected override void OnValueChanged(int newValue)
        {
            slider.value = value;
            inputField.text = value.ToString();
            base.OnValueChanged(newValue);
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
        }
    }
}
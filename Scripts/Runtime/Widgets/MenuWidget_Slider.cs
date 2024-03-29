﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Vulpes.Menus
{
    /// <summary>
    /// <see cref="MenuWidget"/> Slider.
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

        public int MinValue => minValue;

        public int MaxValue => maxValue;

        public void Initialize(string header, int value, int minValue, int maxValue, int stepValue)
        {
            headerText.text = header;
            base.value = value;
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.stepValue = stepValue;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (minValue > maxValue)
            {
                minValue = maxValue;
            }
        }
#endif

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
            => Value = Mathf.Max(minValue, Value - stepValue);

        private void OnNextButtonClick()
            => Value = Mathf.Min(maxValue, Value + stepValue);

        private void OnInputFieldEndEdit(string text)
        {
            if (int.TryParse(text, out int newValue))
            {
                newValue = Mathf.Clamp(newValue, minValue, maxValue);
                Value = newValue;
            }
        }

        private void OnInputFieldSubmit(string text)
            => Select();

        private void OnSliderValueChanged(float value)
            => Value = Mathf.Clamp((int)value, minValue, maxValue);

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            inputField.Select();
            inputField.ActivateInputField();
        }

        protected override void OnValueChanged(int newValue)
        {
            slider.value = newValue;
            inputField.text = newValue.ToString();
            base.OnValueChanged(newValue);
        }
    }
}
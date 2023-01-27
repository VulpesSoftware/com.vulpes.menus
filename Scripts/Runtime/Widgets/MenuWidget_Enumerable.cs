using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Vulpes.Menus
{
    /// <summary>
    /// <see cref="MenuWidget"/> Enumerable.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Widgets/Enumerable")]
    public sealed class MenuWidget_Enumerable : MenuWidget<int>
    {
        [SerializeField, Tooltip("Reverses the order of enumeration.")]
        private bool reverse = false;

        [SerializeField, Tooltip("The available options when cycling the enumerable.")]
        private string[] options = new string[2]
        {
            "Disabled",
            "Enabled"
        };

        [SerializeField] private TextMeshProUGUI headerText = default;
        [SerializeField] private Button previousButton = default;
        [SerializeField] private Button nextButton = default;
        [SerializeField] private TextMeshProUGUI text = default;

        public string[] Options
        {
            get => options;
            set => options = value;
        }

        public void Initialize(string header, int index, string[] options)
        {
            headerText.text = header;
            value = index;
            this.options = options;
        }

        protected override void Awake()
        {
            base.Awake();
            if (Application.isPlaying)
            {
                // Clamp index so that we don't start out of range.
                value = Mathf.Clamp(value, 0, options.Length - 1);
                // Set the text.
                text.text = options[value];
                // Map the buttons.
                if (!reverse)
                {
                    previousButton.onClick.AddListener(OnPreviousButtonClick);
                    nextButton.onClick.AddListener(OnNextButtonClick);
                } else
                {
                    previousButton.onClick.AddListener(OnNextButtonClick);
                    nextButton.onClick.AddListener(OnPreviousButtonClick);
                }
            }
        }

        public override void OnMove(AxisEventData eventData)
        {
            switch (eventData.moveDir)
            {
                case MoveDirection.Left:
                    if (!reverse)
                    {
                        OnPreviousButtonClick();
                    } else
                    {
                        OnNextButtonClick();
                    }
                    break;
                case MoveDirection.Up:
                    base.OnMove(eventData);
                    break;
                case MoveDirection.Right:
                    if (!reverse)
                    {
                        OnNextButtonClick();
                    } else
                    {
                        OnPreviousButtonClick();
                    }
                    break;
                case MoveDirection.Down:
                    base.OnMove(eventData);
                    break;
            }
        }

        private void OnPreviousButtonClick()
        {
            if (Value - 1 < 0)
            {
                Value = options.Length - 1;
            } else
            {
                Value--;
            }
        }

        private void OnNextButtonClick()
        {
            if (Value + 1 >= options.Length)
            {
                Value = 0;
            } else
            {
                Value++;
            }
        }

        protected override void OnValueChanged(int newValue)
        {
            text.text = options[Mathf.Clamp(Value, 0, options.Length - 1)];
            base.OnValueChanged(newValue);
        }
    }
}
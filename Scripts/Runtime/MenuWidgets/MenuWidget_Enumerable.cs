using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Vulpes.Menus.Experimental
{
    /// <summary>
    /// Menu Widget Enumerable.
    /// Note: This is an experimental feature use it at your own peril.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Widgets/Enumerable")]
    public sealed class MenuWidget_Enumerable : MenuWidget
    {
        [SerializeField] private TextMeshProUGUI headerText = default;

        [SerializeField] private Button previousButton = default;
        [SerializeField] private Button nextButton = default;

        [SerializeField] private TextMeshProUGUI text = default;

        [SerializeField] private int index = 0;

        [SerializeField] private bool reverse = false;

        [SerializeField] private string[] options = new string[2]
        {
            "Disabled",
            "Enabled"
        };

        public int Value
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
                OnValueChanged();
            }
        }

        public string[] Options
        {
            get
            {
                return options;
            }
            set
            {
                options = value;
            }
        }

        public void Initialize(string asHeader, int aiIndex, string[] asOptions)
        {
            headerText.text = asHeader.ToUpper();
            index = aiIndex;
            options = asOptions;
        }

        protected override void Awake()
        {
            base.Awake();
            if (Application.isPlaying)
            {
                // Clamp index so that we don't start out of range.
                index = Mathf.Clamp(index, 0, options.Length - 1);
                // Set the text.
                text.text = options[index];
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
            index--;
            if (index < 0)
            {
                index = options.Length - 1;
            }
            OnValueChanged();
        }

        private void OnNextButtonClick()
        {
            index++;
            if (index >= options.Length)
            {
                index = 0;
            }
            OnValueChanged();
        }

        protected override void OnValueChanged()
        {
            text.text = options[Mathf.Clamp(index, 0, options.Length - 1)];
            base.OnValueChanged();
        }
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vulpes.Menus
{
    /// <summary>
    /// <see cref="MenuWidget"/> Dropdown.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Widgets/Dropdown")]
    public sealed class MenuWidget_Dropdown : MenuWidget<int>
    {
        [SerializeField] private TextMeshProUGUI headerText = default;
        [SerializeField] private TMP_Dropdown dropdown = default;

        public override int Value
        {
            get => dropdown.value;
            set => dropdown.value = value;
        }

        public void Initialize(string header)
            => headerText.text = header;

        protected override void Awake()
        {
            base.Awake();
            if (Application.isPlaying)
            {
                dropdown.onValueChanged.RemoveAllListeners();
                dropdown.onValueChanged.AddListener(OnValueChanged);
            }
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            dropdown.Select();
            dropdown.Show();
        }

        protected override void OnValueChanged(int newValue)
        {
            dropdown.Hide();
            Select();
            base.OnValueChanged(newValue);
        }
    }
}
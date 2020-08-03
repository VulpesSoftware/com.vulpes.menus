using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vulpes.Menus.Experimental
{
    /// <summary>
    /// Menu Widget Dropdown.
    /// Note: This is an experimental feature use it at your own peril.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Widgets/Dropdown")]
    public sealed class MenuWidget_Dropdown : MenuWidget
    {
        [SerializeField] private TextMeshProUGUI headerText = default;

        [SerializeField] private TMP_Dropdown dropdown = default;

        public int Value
        {
            get
            {
                return dropdown.value;
            }
            set
            {
                dropdown.value = value;
            }
        }

        public void Initialize(string asHeader)
        {
            headerText.text = asHeader.ToUpper();
        }

        protected override void Awake()
        {
            base.Awake();
            if (Application.isPlaying)
            {
                dropdown.onValueChanged.RemoveAllListeners();
                dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            }
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            dropdown.Select();
            dropdown.Show();
        }

        private void OnDropdownValueChanged(int aiIndex)
        {
            dropdown.Hide();
            Select();
            OnValueChanged();
        }
    }
}
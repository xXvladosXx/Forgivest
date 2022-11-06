using TMPro;
using UnityEngine;

namespace UI.Inventory.Tooltips
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _bodyText;
        [SerializeField] TextMeshProUGUI _nameText;
        [SerializeField] TextMeshProUGUI _requirementsText;

        public void Setup(string itemDescription, string itemName, string requirements)
        {
            _bodyText.text = itemDescription;
            _nameText.text = itemName;
            
            if(_requirementsText != null)
                _requirementsText.text = requirements;
        }
    }
}

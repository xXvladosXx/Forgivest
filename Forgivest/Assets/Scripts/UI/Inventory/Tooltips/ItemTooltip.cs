using TMPro;
using UnityEngine;

namespace UI.Inventory.Tooltips
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI titleText = null;
        [SerializeField] TextMeshProUGUI bodyText = null;

        public void Setup(string itemData)
        {
            bodyText.text = itemData;
        }
    }
}

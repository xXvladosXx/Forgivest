using TMPro;
using UnityEngine;

namespace UI.Inventory.Tooltips
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI bodyText = null;

        public void Setup(string itemData)
        {
            bodyText.text = itemData;
        }
    }
}

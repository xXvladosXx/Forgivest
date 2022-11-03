using UI.Inventory.Core;
using UnityEngine;

namespace UI.Data
{
    [CreateAssetMenu(fileName = "ReusableData", menuName = "UI/ReusableData")]
    public class UIReusableData : ScriptableObject
    {
        public IInventoryHolder LastRaycastedInventoryHolder { get; internal set; } 
    }
}
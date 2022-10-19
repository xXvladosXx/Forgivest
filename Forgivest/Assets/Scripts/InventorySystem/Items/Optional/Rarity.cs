using UnityEngine;

namespace InventorySystem.Items.Optional
{
    [CreateAssetMenu(menuName = "InventorySystem/Rarity")]
    public class Rarity : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
    }
}
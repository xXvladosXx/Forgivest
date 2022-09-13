namespace InventorySystem.Items
{
    public interface IUpgradable
    {
        public Item NextLevel { get; }
    }
}
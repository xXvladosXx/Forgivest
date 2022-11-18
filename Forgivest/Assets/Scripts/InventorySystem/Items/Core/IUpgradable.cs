namespace InventorySystem.Items.Core
{
    public interface IUpgradable
    {
        public Item NextLevel { get; }
    }
}
using AttackSystem.Core;
using Utilities;

namespace GameCore.Crutches
{
    public interface IPlayerObserver
    {
        DamageHandler DamageHandler { get; set; }
        PlayerInputProvider PlayerInputProvider { get; set; }
    }
}
using AttackSystem.Core;
using Utilities;

namespace GameCore.StateMachine
{
    public interface IPlayerObserver
    {
        DamageHandler DamageHandler { get; set; }
        PlayerInputProvider PlayerInputProvider { get; set; }
    }
}
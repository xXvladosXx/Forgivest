using AttackSystem.Core;

namespace GameCore.StateMachine
{
    public interface IGameObserver
    {
        DamageHandler Player { get; set; }
    }
}
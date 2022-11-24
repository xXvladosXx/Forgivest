using AttackSystem.Core;

namespace GameCore.StateMachine
{
    public class GameObserver : IGameObserver
    {
        public DamageHandler Player { get; set; }
    }
}
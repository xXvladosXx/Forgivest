namespace GameCore.StateMachine.States
{
    public interface IState : IExitableState
    {
        void Enter();
    }

    public interface IExitableState
    {
        void Exit();
    }
    
    public interface ILoadState<TLoad> : IExitableState
    {
        void Enter(TLoad load);
    }
}
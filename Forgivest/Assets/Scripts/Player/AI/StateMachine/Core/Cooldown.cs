namespace Player.AI.StateMachine.Core
{
    public interface IHasCooldown
    {
        int Id { get; }
        float CooldownDuration { get; }
    }
}

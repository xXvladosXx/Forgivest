using AI.StateMachine.Config;
using AnimationSystem;
using AttackSystem;
using MovementSystem;
using StatsSystem;
using StatSystem;
using UnityEngine;
using UnityEngine.AI;

namespace AI.StateMachine.Core
{
    public interface IAIEnemy
    {
        NavMeshAgent NavMeshAgent { get; }
        
        Transform Target { get; }
        
        AIStateMachine StateMachine { get; }
        
        AIAgentConfig Config { get; }
        
        Movement Movement { get; }
        GameObject[] Objects { get; }
        GameObject Enemy { get; }
        
        Animator Animator { get; }

        AnimationChanger AnimationChanger { get; }
        
        IAnimationEventUser AnimationEventUser { get; }
        
        StatController StatController { get; }

        AttackApplier AttackApplier { get; }
        
        CooldownSystem Cooldown { get; }

    }
}
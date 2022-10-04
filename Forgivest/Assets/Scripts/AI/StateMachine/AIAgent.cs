using AI.StateMachine.Config;
using AI.StateMachine.Core;
using AnimationSystem;
using Data.Player;
using MovementSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace AI.StateMachine
{
    public class AIAgent : MonoBehaviour, IAIEnemy, IAnimationEventUser
    {
        [field: SerializeField] public GameObject[] Objects { get; private set; }
        
        [field: SerializeField] public AliveEntityAnimationData AliveEntityAnimationData { get; private set; }

        public GameObject Enemy => gameObject; 
        [field: SerializeField] public AIAgentConfig Config { get; private set; }
        
        private Rigidbody _rb;
        
        public Movement Movement { get; private set; }
        public NavMeshAgent NavMeshAgent { get; private set; }
        public Transform Target { get; private set; }
        public AIStateMachine StateMachine { get; private set; }
        
        public Animator Animator { get; private set; }
        public AnimationChanger AnimationChanger { get; private set; }
        public IAnimationEventUser AnimationEventUser => this; 


        private void Awake()
        {
            AliveEntityAnimationData.Init();
            _rb = GetComponent<Rigidbody>();
            Animator = GetComponent<Animator>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Movement = new Movement(NavMeshAgent, _rb, transform);
            AnimationChanger = new AnimationChanger(Animator);
        }

        private void Start()
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
            StateMachine = new AIStateMachine(this);
            StateMachine.ChangeState(StateMachine.AIIdleEnemyState);
        }

        private void Update()
        {
            StateMachine.Update();
        }
        
        public void OnAnimationStarted()
        {
            StateMachine.OnAnimationEnterEvent();
        }

        public void OnAnimationTransitioned()
        {
            StateMachine.OnAnimationTransitionEvent();
        }

        public void OnAnimationEnded()
        {
           StateMachine.OnAnimationExitEvent();
        }

        public void ApplyAttack(float timeOfActiveCollider)
        {
          
        }
    }
}
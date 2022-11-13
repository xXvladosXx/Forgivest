using UnityEngine;

namespace Data.Player
{
    [CreateAssetMenu(menuName = "StateMachine/AnimationData")]
    public class AliveEntityAnimationData : ScriptableObject
    {
        [field: Header(" Movement")]
        [SerializeField] private string HorizontalParameterName = "Horizontal";

        [SerializeField] private string VerticalParameterName = "Vertical";

        [Header("State Group Parameter Names")] 
        [SerializeField] private string _groundedParameterName = "Grounded";

        [SerializeField] private string _movingParameterName = "Moving";
        [SerializeField] private string _stoppingParameterName = "Stopping";
        [SerializeField] private string _landingParameterName = "Landing";
        [SerializeField] private string _airborneParameterName = "Airborne";

        [Header("Grounded Parameter Names")] 
        [SerializeField] private string _idleParameterName = "isIdling";
        [SerializeField] private string _dashParameterName = "isDashing";
        [SerializeField] private string _walkParameterName = "isWalking";
        [SerializeField] private string _runParameterName = "isRunning";
        [SerializeField] private string _sprintParameterName = "isSprinting";
        [SerializeField] private string _mediumStopParameterName = "isMediumStopping";
        [SerializeField] private string _hardStopParameterName = "isHardStopping";
        [SerializeField] private string _rollParameterName = "isRolling";
        [SerializeField] private string _hardLandParameterName = "isHardLanding";

        [Header("Combat Parameter Names")]
        [SerializeField] private string _aimingParameterName = "isAiming";
        [SerializeField] private string _firingParameterName = "isFiring";
        [SerializeField] private string _equippingParameterName = "isEquipping";
        [SerializeField] private string _reloadingParameterName = "isReloading";
        [SerializeField] private string _comboParameterName = "Combo";
        [SerializeField] private string _airAttackParameterName = "AirAttack";
        [SerializeField] private string _airAttackLandedParameterName = "AirAttackLanded";
        [SerializeField] private string _airAttackDashParameterName = "AirAttackDash";
        [SerializeField] private string _dashAttackParameterName = "DashAttack";
        [SerializeField] private string _sprintAttackParameterName = "SprintAttack";
        [SerializeField] private string _attackParameterName = "Attack";
        [SerializeField] private string _speedParameterName = "Speed";

        [Header("Airborne Parameter Names")] 
        [SerializeField] private string _fallParameterName = "isFalling";
        
        [Header("Skill Parameter Names")]
        [SerializeField] private string _skillParameterName = "CastSkill";
        [SerializeField] private string _firstSkillParameterName = "FirstSkill";
        [SerializeField] private string _secondSkillParameterName = "SecondSkill";
        [SerializeField] private string _thirdSkillParameterName = "ThirdSkill";
        [SerializeField] private string _fourthSkillParameterName = "FourthSkill";
        [SerializeField] private string _fifthSkillParameterName = "FifthSkill";
        [SerializeField] private string _sixthSkillParameterName = "SixthSkill";

        public int GroundedParameterHash { get; private set; }
        public int MovingParameterHash { get; private set; }
        public int StoppingParameterHash { get; private set; }
        public int LandingParameterHash { get; private set; }
        public int AirborneParameterHash { get; private set; }

        public int IdleParameterHash { get; private set; }
        public int DashParameterHash { get; private set; }
        public int WalkParameterHash { get; private set; }
        public int RunParameterHash { get; private set; }
        public int SprintParameterHash { get; private set; }
        public int MediumStopParameterHash { get; private set; }
        public int HardStopParameterHash { get; private set; }
        public int RollParameterHash { get; private set; }
        public int HardLandParameterHash { get; private set; }

        public int FallParameterHash { get; private set; }
        public int HorizontalParameterHash { get; private set; }
        public int VerticalParameterHash { get; private set; }

        public int SpeedParameterHash { get; private set; }
        public int AimingParameterHash { get; private set; }
        public int SprintAttackParameterHash { get; private set; }
        public int DashAttackParameterHash { get; private set; }
        public int FiringParameterHash { get; private set; }
        public int ComboParameterHash { get; private set; }
        public int ReloadParameterHash { get; private set; }
        public int EquippingParameterHash { get; private set; }
        public int AirAttackParameterHash { get; private set; }
        public int AttackParameterHash { get; private set; }
        public int AirAttackDashParameterHash { get; private set; }
        public int AirAttackLandedParameterHash { get; private set; }
        
        public int FirstSkillParameterHash { get; private set; }
        public int SecondSkillParameterHash { get; private set; }
        public int ThirdSkillParameterHash { get; private set; }
        public int FourthSkillParameterHash { get; private set; }
        public int FifthSkillParameterHash { get; private set; }
        public int SixthSkillParameterHash { get; private set; }
        public int SkillParameterHash { get; private set; }
        public void Init()
        {
            GroundedParameterHash = Animator.StringToHash(_groundedParameterName);
            MovingParameterHash = Animator.StringToHash(_movingParameterName);
            StoppingParameterHash = Animator.StringToHash(_stoppingParameterName);
            LandingParameterHash = Animator.StringToHash(_landingParameterName);
            AirborneParameterHash = Animator.StringToHash(_airborneParameterName);
            DashParameterHash = Animator.StringToHash(_dashParameterName);

            IdleParameterHash = Animator.StringToHash(_idleParameterName);
            DashParameterHash = Animator.StringToHash(_dashParameterName);
            WalkParameterHash = Animator.StringToHash(_walkParameterName);
            RunParameterHash = Animator.StringToHash(_runParameterName);
            SprintParameterHash = Animator.StringToHash(_sprintParameterName);
            MediumStopParameterHash = Animator.StringToHash(_mediumStopParameterName);
            HardStopParameterHash = Animator.StringToHash(_hardStopParameterName);
            RollParameterHash = Animator.StringToHash(_rollParameterName);
            HardLandParameterHash = Animator.StringToHash(_hardLandParameterName);

            FallParameterHash = Animator.StringToHash(_fallParameterName);

            SpeedParameterHash = Animator.StringToHash(_speedParameterName);

            AimingParameterHash = Animator.StringToHash(_aimingParameterName);
            FiringParameterHash = Animator.StringToHash(_firingParameterName);
            ReloadParameterHash = Animator.StringToHash(_reloadingParameterName);
            EquippingParameterHash = Animator.StringToHash(_equippingParameterName);
            ComboParameterHash = Animator.StringToHash(_comboParameterName);
            AirAttackParameterHash = Animator.StringToHash(_airAttackParameterName);
            AirAttackLandedParameterHash = Animator.StringToHash(_airAttackLandedParameterName);
            SprintAttackParameterHash = Animator.StringToHash(_sprintAttackParameterName);
            DashAttackParameterHash = Animator.StringToHash(_dashAttackParameterName);
            AirAttackDashParameterHash = Animator.StringToHash(_airAttackDashParameterName);

            HorizontalParameterHash = Animator.StringToHash(HorizontalParameterName);
            VerticalParameterHash = Animator.StringToHash(VerticalParameterName);

            AttackParameterHash = Animator.StringToHash(_attackParameterName);
            
            SkillParameterHash = Animator.StringToHash(_skillParameterName);
            FirstSkillParameterHash = Animator.StringToHash(_firstSkillParameterName);
            SecondSkillParameterHash = Animator.StringToHash(_secondSkillParameterName);
            ThirdSkillParameterHash = Animator.StringToHash(_thirdSkillParameterName);
            FourthSkillParameterHash = Animator.StringToHash(_fourthSkillParameterName);
            FifthSkillParameterHash = Animator.StringToHash(_fifthSkillParameterName);
            SixthSkillParameterHash = Animator.StringToHash(_sixthSkillParameterName);
        }
    }
}
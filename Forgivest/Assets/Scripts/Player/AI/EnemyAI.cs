using UnityEngine;
using UnityEngine.Serialization;

namespace AI
{
    public class EnemyAI : MovementAI
    {
        private bool _detected = false;
        
        
        [SerializeField] protected GameObject[] objects;
        [SerializeField] protected GameObject player;
        public override void Start()
        {
            base.Start();
            var isDetected = new Leaf("Is Detected", IsDetected);
            
            var whileDetected = new BehaviourTree();
            whileDetected.AddChild(isDetected);
            var selectObject = new RSelector("Select Objects To View");
            for (var i = 0; i < objects.Length; i++)
            {
                var gta = new Leaf("Go to " + objects[i].name, i, GoToObjects);
                selectObject.AddChild(gta);
            }

            var loopGoing = new Loop("Go to Objects", whileDetected);
            loopGoing.AddChild(selectObject);
            var runTo = new Sequence("Run Away");
            var canSee = new Leaf("Can See Player?", CanSeePlayer);
            var run = new Leaf("Run To Player", RunToPlayer);
            var attackPlayer = new Leaf("Attack Player", AttackPlayer);
            var cantSee = new Inverter("Cant See Player?");
            cantSee.AddChild(canSee);
            
            
            var enemyConditions = new BehaviourTree();
            var conditions = new Sequence("Enemy Conditions");
            conditions.AddChild(cantSee);
            enemyConditions.AddChild(conditions);
            var beEnemy = new DepSequence("be Enemy", enemyConditions, agent);
            beEnemy.AddChild(loopGoing);
            runTo.AddChild(canSee);
            runTo.AddChild(run);
            runTo.AddChild(attackPlayer);

            var toBeEnemy = new Selector("Be enemy");
            toBeEnemy.AddChild(beEnemy);
            toBeEnemy.AddChild(runTo);
            
            tree.AddChild(toBeEnemy);

            tree.PrintTree();
        }
        public Node.Status RunToPlayer()
        {
            return RunTo(player.transform.position, 10);
        }
        
        public Node.Status AttackPlayer()
        {
            Node.Status s = RunToPlayer();
            if(s == Node.Status.SUCCESS)
                Debug.Log("Attacked!");
            return s;
        }

        public Node.Status CanSeePlayer()
        {
            _detected = true;
            return CanSee(player.transform.position, "dodik", 10, 360);
        }
        
        public Node.Status GoToObjects(int i)
        {
            if (!objects[i].activeSelf) return Node.Status.FAILURE;
            var s = GoToLocation(objects[i].transform.position);
            return s;
        }

        public Node.Status IsDetected()
        {
            if (_detected == true)
                return Node.Status.SUCCESS;
            else
                return Node.Status.FAILURE;
        }
    }
}
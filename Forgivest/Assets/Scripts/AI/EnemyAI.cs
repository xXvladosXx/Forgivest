using UnityEngine;

namespace AI
{
    public class EnemyAI : MovementAI
    {
        [SerializeField] protected GameObject player;
        public override void Start()
        {
            base.Start();

            var runTo = new Sequence("Run Away");
            var canSee = new Leaf("Can See Player?", CanSeePlayer);
            var run = new Leaf("Run To Player", RunToPlayer);


            runTo.AddChild(canSee);
            runTo.AddChild(run);

            tree.AddChild(runTo);

            tree.PrintTree();
        }
        public Node.Status RunToPlayer()
        {
            return RunTo(player.transform.position, 10);
        }

        public Node.Status CanSeePlayer()
        {
            return CanSee(player.transform.position, "dodik", 10, 360);
        }
    }
}
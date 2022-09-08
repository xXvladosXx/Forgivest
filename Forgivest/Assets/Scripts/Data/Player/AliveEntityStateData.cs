using UnityEngine;

namespace Data.Player
{
    [CreateAssetMenu(fileName = "AliveEntityStateData", menuName = "StateMachine/StateData")]
    public class AliveEntityStateData : ScriptableObject
    {
        [field: SerializeField] public AliveEntityGroundedData GroundedData { get; private set; }
    }
}
using UnityEngine;

namespace StudioScor.StateMachine
{
    public abstract class CheckDelayInRange<T> : Decision<T> where T : MonoBehaviour
    {
        [SerializeField] private float _MinRange = 0f;
        [SerializeField] private float _MaxRange = 1f;

        public override bool Decide(StateMachine<T> stateMachine)
        {
            return stateMachine.StateTimeElapsed > _MinRange && stateMachine.StateTimeElapsed < _MaxRange;
        }
    }
}

using UnityEngine;

namespace KimScor.StateMachine
{
    public abstract class CheckDelay<T> : Decision<T>
    {
        [SerializeField] private float _Delay = 0.2f;

        public override bool Decide(StateMachine<T> stateMachine)
        {
            return stateMachine.StateTimeElapsed > _Delay != _IsInverse;
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

namespace KimScor.StateMachine
{
    public abstract class CheckCurrentState<T> : Decision<T>
    {
        [SerializeField] private List<State<T>> _States;
        public override bool Decide(StateMachine<T> stateMachine)
        {
            return _States.Contains(stateMachine.CurrentState) != _IsInverse;
        }
    }
}

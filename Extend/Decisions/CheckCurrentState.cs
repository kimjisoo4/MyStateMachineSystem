using UnityEngine;
using System.Collections.Generic;

namespace StudioScor.StateMachine
{
    public abstract class CheckCurrentState : Decision
    {
        [SerializeField] private List<State> _States;
        public override bool Decide(StateMachineComponent stateMachine)
        {
            return _States.Contains(stateMachine.CurrentState) != _IsInverse;
        }
    }
}

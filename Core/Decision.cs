using UnityEngine;
using System.Collections;
using StudioScor.Utilities;

namespace StudioScor.StateMachine
{
    public abstract class Decision : BaseScriptableObject
    {
        [SerializeField] protected bool _IsInverse = false;

        public virtual void EnterDecide(StateMachineComponent stateMachine) { }
        public abstract bool Decide(StateMachineComponent stateMachine);
        public virtual void ExitDecide(StateMachineComponent stateMachine) { }
    }
}
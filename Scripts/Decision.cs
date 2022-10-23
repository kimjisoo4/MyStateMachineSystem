using UnityEngine;
using System.Collections;

namespace KimScor.StateMachine
{
    public abstract class Decision<T> : ScriptableObject where T : MonoBehaviour
    {
        [SerializeField] protected bool _IsInverse = false;

        public virtual void EnterDecide(StateMachine<T> stateMachine) { }
        public virtual void ExitDecide(StateMachine<T> stateMachine) { }
        public abstract bool Decide(StateMachine<T> stateMachine);
    }
}
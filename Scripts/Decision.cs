using UnityEngine;
using System.Collections;

namespace KimScor.StateMachine
{
    public abstract class Decision<T> : ScriptableObject
    {
        [SerializeField] protected bool _IsInverse = false;
        public abstract bool Decide(StateMachine<T> stateMachine);
    }
}
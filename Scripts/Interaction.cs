using UnityEngine;

namespace KimScor.StateMachine
{
    public abstract class Interaction<T> : ScriptableObject
    {
        public abstract void Interactive(StateMachine<T> onwer, StateMachine<T> target);

    }
}
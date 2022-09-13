using System.Collections.Generic;

namespace KimScor.StateMachine
{
    [System.Serializable]
    public class DecisionAction<T>
    {
        public string Contents = " Write your explanation here. ";
        public bool isOnce = false;
        public Decisions<T> decisions;
        public List<Action<T>> action = null;

    }

}

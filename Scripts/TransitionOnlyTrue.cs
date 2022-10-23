using UnityEngine;

namespace KimScor.StateMachine
{
    [System.Serializable]
    public class TransitionOnlyTrue<T> where T : MonoBehaviour
    {
        public Decisions<T> decisions;
        public State<T> trueState;
        public bool CheckTransition(StateMachine<T> stateMachine)
        {
            bool isDecide = decisions.CheckDecisions(stateMachine);

            if (isDecide)
            {
                stateMachine.TransitionToState(trueState);
            }

            return isDecide;
        }
    }

}
using System.Collections;
using UnityEngine;

namespace KimScor.StateMachine
{

    [System.Serializable]
    public class Transition<T> where T : MonoBehaviour
    {
#if UNITY_EDITOR
        public string _TransitionName;
#endif
        public Decisions<T> decisions;
        public State<T> trueState;
        public State<T> falseState;

        public void EnterTransition(StateMachine<T> stateMachine)
        {
            decisions.EnterDecisions(stateMachine);
        }
        public void ExitTransition(StateMachine<T> stateMachine)
        {
            decisions.ExitDecisions(stateMachine);
        }

        public void CheckTransition(StateMachine<T> stateMachine)
        {
            bool isDecide = decisions.CheckDecisions(stateMachine);

            if (isDecide)
            {
                stateMachine.TransitionToState(trueState);
            }
            else
            {
                stateMachine.TransitionToState(falseState);
            }
        }
    }

}
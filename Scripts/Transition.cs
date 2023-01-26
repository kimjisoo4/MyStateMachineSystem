using System.Collections;
using UnityEngine;

namespace StudioScor.StateMachine
{

    [System.Serializable]
    public class Transition<T> where T : MonoBehaviour
    {
        public string _TransitionName;
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
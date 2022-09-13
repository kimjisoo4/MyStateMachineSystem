using System.Collections;

namespace KimScor.StateMachine
{

    [System.Serializable]
    public class Transition<T>
    {
#if UNITY_EDITOR
        public string _TransitionName;
#endif
        public Decisions<T> decisions;
        public State<T> trueState;
        public State<T> falseState;

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
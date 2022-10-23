using UnityEngine;
using System.Collections.Generic;

namespace KimScor.StateMachine
{
    public abstract class TransitionState<T> : ScriptableObject where T : MonoBehaviour
    {
        public List<TransitionOnlyTrue<T>> transitions;

        public bool CheckTransition(StateMachine<T> stateMachine)
        {
            foreach (TransitionOnlyTrue<T> transition in transitions)
            {
                bool isDecide = transition.CheckTransition(stateMachine);

                if (isDecide)
                {
                    return true;
                }
            }

            return false;
        }
    }

}
using System.Collections.Generic;
using UnityEngine;

namespace KimScor.StateMachine
{
    [System.Serializable]
    public class Decisions<T> where T : MonoBehaviour
    {
        public enum LogicGate
        {
            And,
            Or,
            Not,
        }

        public LogicGate logicGate = LogicGate.And;
        public List<Decision<T>> decisions;

        public void EnterDecisions(StateMachine<T> stateMachine)
        {
            foreach (var decision in decisions)
            {
                decision.EnterDecide(stateMachine);
            }
        }
        public void ExitDecisions(StateMachine<T> stateMachine)
        {
            foreach (var decision in decisions)
            {
                decision.ExitDecide(stateMachine);
            }
        }
        public bool CheckDecisions(StateMachine<T> stateMachine)
        {
            bool isDecide = false;

            switch (logicGate)
            {
                case LogicGate.And:
                    isDecide = AndGate(stateMachine);
                    break;
                case LogicGate.Or:
                    isDecide = OrGate(stateMachine);
                    break;
                case LogicGate.Not:
                    isDecide = NotGate(stateMachine);
                    break;
            }

            return isDecide;
        }
        private bool AndGate(StateMachine<T> stateMachine)
        {
            foreach (Decision<T> decide in decisions)
            {
                if (!decide.Decide(stateMachine))
                {
                    return false;
                }
            }

            return true;
        }

        private bool OrGate(StateMachine<T> stateMachine)
        {
            foreach (Decision<T> decide in decisions)
            {
                if (decide.Decide(stateMachine))
                {
                    return true;
                }
            }

            return false;
        }
        private bool NotGate(StateMachine<T> stateMachine)
        {
            foreach (Decision<T> decide in decisions)
            {
                if (decide.Decide(stateMachine))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
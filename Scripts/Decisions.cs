using System.Collections.Generic;

namespace KimScor.StateMachine
{
    [System.Serializable]
    public class Decisions<T>
    {
        public enum LogicGate
        {
            And,
            Or,
            Not,
        }

        public LogicGate logicGate = LogicGate.And;
        public List<Decision<T>> decisions;

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
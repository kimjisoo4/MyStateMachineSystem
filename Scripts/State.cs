using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KimScor.StateMachine
{
    public abstract class State<T> : ScriptableObject where T : MonoBehaviour
    {
        public string StateName = "New State";
        public Color Color = Color.white;

        [SerializeField] private List<Action<T>> _EarlyActions;
        [SerializeField] private List<Action<T>> _ProcessActions;
        [SerializeField] private List<Action<T>> _LateActions;

        [SerializeField] private List<Transition<T>> _Transitions;
        [SerializeField] private List<DecisionAction<T>> _DecisionActions;
        public int GetDecisionActionCount => _DecisionActions.Count;
        public IReadOnlyList<DecisionAction<T>> DecisionActions => _DecisionActions;

        #region EDITOR ONLY
#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (Transition<T> transition in _Transitions)
            {
                string transitionName = "[T] ";

                if (transition.trueState != null)
                {
                    transitionName += transition.trueState.StateName;
                }

                transitionName += " | ";

                if (transition.falseState != null)
                {
                    transitionName += transition.falseState.StateName;
                }

                transitionName += " [F]";

                transition._TransitionName = transitionName;
            }
        }


        public void DrawGizmos(Transform transform)
        {
            foreach (Action<T> action in _EarlyActions)
            {
                action.DrawGizmos(transform);
            }
            foreach (Action<T> action in _ProcessActions)
            {
                action.DrawGizmos(transform);
            }
            foreach (Action<T> action in _LateActions)
            {
                action.DrawGizmos(transform);
            }

            Gizmos.color = Color;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * 2f, 0.1f);
        }
        public void DrawGizmosSelected(Transform transform)
        {
            foreach (Action<T> action in _EarlyActions)
            {
                action.DrawGizmosSelected(transform);
            }
            foreach (Action<T> action in _ProcessActions)
            {
                action.DrawGizmosSelected(transform);
            }
            foreach (Action<T> action in _LateActions)
            {
                action.DrawGizmosSelected(transform);
            }

            Gizmos.color = Color;
            Gizmos.DrawSphere(transform.position + Vector3.up * 2f, 0.1f);
        }
#endif
        #endregion

        public void EnterState(StateMachine<T> stateMachine)
        {
            EnterTransitions(stateMachine);
            EnterActions(stateMachine);

        }
        public void ExitState(StateMachine<T> stateMachine)
        {
            ExitTransitions(stateMachine);
            ExitActions(stateMachine);
        }
        private void EnterTransitions(StateMachine<T> stateMachine)
        {
            foreach (Transition<T> transition in _Transitions)
            {
                transition.EnterTransition(stateMachine);
            }
        }
            
        private void EnterActions(StateMachine<T> stateMachine)
        {
            foreach (Action<T> action in _EarlyActions)
            {
                action.EnterAction(stateMachine);
            }
            foreach (Action<T> action in _ProcessActions)
            {
                action.EnterAction(stateMachine);
            }
            foreach (Action<T> action in _LateActions)
            {
                action.EnterAction(stateMachine);
            }
        }

        private void ExitTransitions(StateMachine<T> stateMachine)
        {
            foreach (Transition<T> transition in _Transitions)
            {
                transition.ExitTransition(stateMachine);
            }
        }
        private void ExitActions(StateMachine<T> stateMachine)
        {
            foreach (Action<T> action in _EarlyActions)
            {
                action.ExitAction(stateMachine);
            }
            foreach (Action<T> action in _ProcessActions)
            {
                action.ExitAction(stateMachine);
            }
            foreach (Action<T> action in _LateActions)
            {
                action.ExitAction(stateMachine);
            }
        }

        public void UpdateState(StateMachine<T> stateMachine)
        {
            DoActions(stateMachine);

            CheckDecisionAction(stateMachine);

            CheckTransition(stateMachine);
        }

        public void DoActions(StateMachine<T> stateMachine)
        {
            foreach (Action<T> action in _EarlyActions)
            {
                action.UpdateAction(stateMachine);
            }
            foreach (Action<T> action in _ProcessActions)
            {
                action.UpdateAction(stateMachine);
            }
            foreach (Action<T> action in _LateActions)
            {
                action.UpdateAction(stateMachine);
            }
        }

        public void CheckTransition(StateMachine<T> stateMachine)
        {
            foreach (Transition<T> transition in _Transitions)
            {
                transition.CheckTransition(stateMachine);
            }
        }
        public void CheckDecisionAction(StateMachine<T> stateMachine)
        {
            for (int i = 0; i < _DecisionActions.Count; i++)
            {
                if (_DecisionActions[i].isOnce && stateMachine.isActive[i])
                {
                    continue;
                }

                bool isDecide = _DecisionActions[i].decisions.CheckDecisions(stateMachine);


                if (isDecide)
                {
                    if (!stateMachine.isActive[i])
                    {
                        foreach (Action<T> action in _DecisionActions[i].action)
                        {
                            action.EnterAction(stateMachine);
                        }
                    }

                    foreach (Action<T> action in _DecisionActions[i].action)
                    {
                        action.UpdateAction(stateMachine);
                    }

                    if (_DecisionActions[i].isOnce)
                    {
                        foreach (Action<T> action in _DecisionActions[i].action)
                        {
                            action.ExitAction(stateMachine);
                        }
                    }

                    stateMachine.isActive[i] = true;
                }
                else
                {
                    if (stateMachine.isActive[i])
                    {
                        foreach (Action<T> action in _DecisionActions[i].action)
                        {
                            action.ExitAction(stateMachine);
                        }
                    }

                    stateMachine.isActive[i] = false;
                }
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace StudioScor.StateMachine
{
    public abstract class State<T> : ScriptableObject where T : MonoBehaviour
    {
        [Header(" [ State ] ")]
        public string StateName = "New State";
        public Color Color = Color.white;
        [Space(5f)]
        [SerializeField] private List<Action<T>> _EarlyActions;
        [SerializeField] private List<Action<T>> _ProcessActions;
        [SerializeField] private List<Action<T>> _PhysisActions;
        [SerializeField] private List<Action<T>> _LateActions;
        [Space(5f)]
        [SerializeField] private List<DecisionAction<T>> _ConditionActions;

        [Header(" [ Transitions ] ")]
        [SerializeField] private List<Transition<T>> _Transitions;

        public int GetConditionActionCount => _ConditionActions.Count;

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
#endif

        [Conditional("UNITY_EDITOR")]
        public void DrawGizmos(StateMachine<T> stateMachine)
        {
#if UNITY_EDITOR
            foreach (Action<T> action in _EarlyActions)
            {
                action.DrawGizmos(stateMachine);
            }
            foreach (Action<T> action in _ProcessActions)
            {
                action.DrawGizmos(stateMachine);
            }
            foreach (Action<T> action in _LateActions)
            {
                action.DrawGizmos(stateMachine);
            }
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public void DrawGizmosSelected(StateMachine<T> stateMachine)
        {
#if UNITY_EDITOR
            foreach (Action<T> action in _EarlyActions)
            {
                action.DrawGizmosSelected(stateMachine);
            }
            foreach (Action<T> action in _ProcessActions)
            {
                action.DrawGizmosSelected(stateMachine);
            }
            foreach (Action<T> action in _LateActions)
            {
                action.DrawGizmosSelected(stateMachine);
            }
#endif
        }
        #endregion

        public void EnterState(StateMachine<T> stateMachine)
        {
            EnterTransitions(stateMachine);
            EnterActions(stateMachine);
        }
        public void UpdateState(StateMachine<T> stateMachine)
        {
            DoActions(stateMachine);

            TryConditionAction(stateMachine);
        }
        public void UpdatePhysicsState(StateMachine<T> stateMachine)
        {
            DoPhysicsActions(stateMachine);
        }
        public void LateState(StateMachine<T> stateMachine)
        {
            DoLateActions(stateMachine);

            CheckTransition(stateMachine);
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
            foreach (Action<T> action in _PhysisActions)
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
            foreach (Action<T> action in _PhysisActions)
            {
                action.ExitAction(stateMachine);
            }
        }

        

        private void DoActions(StateMachine<T> stateMachine)
        {
            foreach (Action<T> action in _EarlyActions)
            {
                action.UpdateAction(stateMachine);
            }
            foreach (Action<T> action in _ProcessActions)
            {
                action.UpdateAction(stateMachine);
            }
        }
        private void DoPhysicsActions(StateMachine<T> stateMachine)
        {
            foreach (Action<T> action in _PhysisActions)
            {
                action.UpdateAction(stateMachine);
            }
        }

        private void DoLateActions(StateMachine<T> stateMachine)
        {
            foreach (Action<T> action in _LateActions)
            {
                action.UpdateAction(stateMachine);
            }
        }


        private void CheckTransition(StateMachine<T> stateMachine)
        {
            foreach (Transition<T> transition in _Transitions)
            {
                transition.CheckTransition(stateMachine);
            }
        }

        private void TryConditionAction(StateMachine<T> stateMachine)
        {
            for (int i = 0; i < _ConditionActions.Count; i++)
            {
                if (_ConditionActions[i].isOnce && stateMachine.WasActivateConditionActions[i])
                    continue;

                bool isDecide = _ConditionActions[i].decisions.CheckDecisions(stateMachine);

                if (isDecide)
                {
                    if (!stateMachine.WasActivateConditionActions[i])
                    {
                        foreach (Action<T> action in _ConditionActions[i].action)
                        {
                            action.EnterAction(stateMachine);
                        }
                    }

                    foreach (Action<T> action in _ConditionActions[i].action)
                    {
                        action.UpdateAction(stateMachine);
                    }

                    if (_ConditionActions[i].isOnce)
                    {
                        foreach (Action<T> action in _ConditionActions[i].action)
                        {
                            action.ExitAction(stateMachine);
                        }
                    }

                    stateMachine.WasActivateConditionActions[i] = true;
                }
                else
                {
                    if (stateMachine.WasActivateConditionActions[i])
                    {
                        foreach (Action<T> action in _ConditionActions[i].action)
                        {
                            action.ExitAction(stateMachine);
                        }
                    }

                    stateMachine.WasActivateConditionActions[i] = false;
                }
            }
        }
    }

}

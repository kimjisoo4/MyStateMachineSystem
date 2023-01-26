using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StudioScor.StateMachine
{
    public class StateMachine<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Event
        public delegate void ChangedStateHandler(StateMachine<T> stateMachine, State<T> currentState, State<T> prevState);
        #endregion

        [Header(" [ State Machine ] ")]
        [Tooltip(" Start/Default State.")]
        [SerializeField] private State<T> _DefaultState;
        [Tooltip(" Remain Current State. ")]
        [SerializeField] private State<T> _RemainState;
        [Tooltip(" Transition Default State.")]
        [SerializeField] private State<T> _ResetState;
        
        [Space(5f)]
        [SerializeField] private T _Context;
        public T Context => _Context;
        
        private bool _WasSetup = false;
        private State<T> _CurrentState;
        private float _StateTimeElapsed = 0;
        private float _DeltaTime;
        private float _FixedDeltaTime;
        
        [HideInInspector] public bool[] WasActivateConditionActions;

        public State<T> CurrentState => _CurrentState;
        public float DeltaTime => _DeltaTime;
        public float FixedDeltaTime => _FixedDeltaTime;
        public float StateTimeElapsed => _StateTimeElapsed;

        public event ChangedStateHandler OnChangedState;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_CurrentState || !gameObject.activeInHierarchy)
                return;

            Gizmos.color = _CurrentState.Color;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * 2f, 0.5f);

            _CurrentState.DrawGizmos(this);
        }
        private void OnDrawGizmosSelected()
        {
            if (!_CurrentState || !gameObject.activeInHierarchy)
                return;

            Gizmos.color = _CurrentState.Color;
            Gizmos.DrawSphere(transform.position + Vector3.up * 2f, 0.5f);

            _CurrentState.DrawGizmosSelected(this);
        }
#endif

        private void Awake()
        {
            if (!_WasSetup)
                SetupStateMachine();
        }

        private void SetupStateMachine()
        {
            if (_WasSetup)
                return;

            _WasSetup = true;

            if (Context)
            {
                _Context = GetComponent<T>();
            }

            TransitionToState(_DefaultState);

            OnSetup();
        }

        protected virtual void OnSetup() { }


        private void Update()
        {
            _DeltaTime = Time.deltaTime;

            _StateTimeElapsed += _DeltaTime;

            _CurrentState.UpdateState(this);
        }
        private void FixedUpdate()
        {
            _FixedDeltaTime = Time.fixedDeltaTime;

            _CurrentState.UpdatePhysicsState(this);
        }

        public void TransitionToDefaultState()
        {
            TransitionToState(_DefaultState);
        }

        public void TransitionToState(State<T> nextState)
        {
            if (nextState == null)
                return;

            if (nextState == _RemainState)
                return;
            
            if (nextState == _ResetState)
            {
                if (_CurrentState == _DefaultState)
                {
                    return;
                }

                Transition(_DefaultState);
            }
            else
            {
                Transition(nextState);

            }        
        }
        private void Transition(State<T> nextState)
        {
            if (_CurrentState != null)
            {
                _CurrentState.ExitState(this);
            }

            var prevState = _CurrentState;

            _StateTimeElapsed = 0f;

            _CurrentState = nextState;

            WasActivateConditionActions = new bool[_CurrentState.GetConditionActionCount];

            _CurrentState.EnterState(this);

            Callback_OnChangedState(prevState);
        }

        #region Callback
        protected void Callback_OnChangedState(State<T> prevState)
        {
            OnChangedState?.Invoke(this, _CurrentState, prevState);
        }
        #endregion
    }
}
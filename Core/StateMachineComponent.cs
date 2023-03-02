using UnityEngine;
using StudioScor.Utilities;
using System;
using System.Collections;

namespace StudioScor.StateMachine
{
    public class StateMachineComponent : BaseMonoBehaviour
    {
        #region Event
        public delegate void ChangedStateHandler(StateMachineComponent stateMachine, State currentState, State prevState);
        #endregion

        [Header(" [ State Machine ] ")]
        [Tooltip(" Start/Default State.")]
        [SerializeField] private State _DefaultState;
        [SerializeField] private BlackBoard _BlackBoard;
        [Tooltip(" Remain Current State. ")]
        [SerializeField][ SReadOnlyWhenPlaying] private State _RemainState;
        [Tooltip(" Transition Default State.")]
        [SerializeField][ SReadOnlyWhenPlaying] private State _ResetState;

        private State _CurrentState;
        private float _StateTimeElapsed = 0;
        private float _DeltaTime;
        private float _FixedDeltaTime;

        public State CurrentState => _CurrentState;
        public float DeltaTime => _DeltaTime;
        public float FixedDeltaTime => _FixedDeltaTime;
        public float StateElapsed => _StateTimeElapsed;

        public BlackBoard BlackBoard => _BlackBoard;

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
            SetupStateMachine();
        }
        private void OnDestroy()
        {
            if (_CurrentState != null)
            {
                _CurrentState.ExitState(this);
            }

            _BlackBoard.Remove(this);
        }

        private void Start()
        {
            TransitionToState(_DefaultState);
        }

        private void SetupStateMachine()
        {
            _BlackBoard.Create(this);

            OnSetup();
        }
        public void ResetStateMachine()
        {
            _BlackBoard.Clear(this);

            OnReset();
        }

        protected virtual void OnSetup() { }
        protected virtual void OnReset() { }


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
        private void LateUpdate()
        {
            _DeltaTime = Time.deltaTime;

            _CurrentState.UpdateLateState(this);
        }

        public void TransitionToDefaultState()
        {
            TransitionToState(_DefaultState);
        }

        public void TransitionToState(State nextState)
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
        private void Transition(State nextState)
        {
            if (_CurrentState != null)
            {
                _CurrentState.ExitState(this);
            }

            var prevState = _CurrentState;

            _StateTimeElapsed = 0f;

            _CurrentState = nextState;

            _CurrentState.EnterState(this);

            Callback_OnChangedState(prevState);
        }

        #region Callback
        protected void Callback_OnChangedState(State prevState)
        {
            OnChangedState?.Invoke(this, _CurrentState, prevState);
        }
        #endregion
    }
}
﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KimScor.StateMachine
{

    [System.Serializable]
    public class StateMachine<T>
    {
        [Header(" 기본 상태 ")]
        [SerializeField] private State<T> _DefaultState;
        [Header(" 현재 상태 ")]
        [SerializeField] private State<T> _CurrentState;
        [Header(" 현재 상태의 지속 시간 ")]
        [SerializeField] private float _StateTimeElapsed = 0;
        [Header(" 빈 상태 ")]
        [SerializeField] private State<T> _RemainState;
        [Header(" 초기화 상태 ")]
        [SerializeField] private State<T> _ResetState;


        public State<T> CurrentState => _CurrentState;
        [HideInInspector] public bool[] isActive;

        [SerializeField] private T _Owner;
        [SerializeField] private Transform _Transform;
        public T Owner => _Owner;

        public Transform Transform => _Transform;

        private float _DeltaTime;

        public StateMachine()
        {

        }
        public StateMachine(T owner, Transform transform, State<T> defaultState, State<T> remainState, State<T> resetState)
        {
            _DefaultState = defaultState;
            _RemainState = remainState;
            _ResetState = resetState;
            _Owner = owner;
            _Transform = transform;

            TransitionToState(_DefaultState);
        }

        public float DeltaTime => _DeltaTime;
        public float StateTimeElapsed => _StateTimeElapsed;

        public void Setup(Transform transform)
        {
            _Transform = transform;

            TransitionToState(_DefaultState);
        }

        public void UpdateStateMachine(float deltaTime)
        {
            _DeltaTime = deltaTime;

            _StateTimeElapsed += _DeltaTime;

            _CurrentState.UpdateState(this);
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
                _CurrentState.ExitAction(this);
            }

            _StateTimeElapsed = 0f;

            _CurrentState = nextState;

            _CurrentState.EnterAction(this);

            isActive = new bool[_CurrentState.GetDecisionActionCount];

        }
    }
}
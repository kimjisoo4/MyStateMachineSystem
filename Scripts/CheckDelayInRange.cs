﻿using UnityEngine;

namespace KimScor.StateMachine
{
    public abstract class CheckDelayInRange<T> : Decision<T>
    {
        [SerializeField] private float _MinRange = 0f;
        [SerializeField] private float _MaxRange = 1f;

        public override bool Decide(StateMachine<T> stateMachine)
        {
            return stateMachine.StateTimeElapsed > _MinRange && stateMachine.StateTimeElapsed < _MaxRange;
        }
    }
}
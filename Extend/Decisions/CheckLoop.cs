using UnityEngine;
using System.Collections.Generic;
using StudioScor.Utilities;

namespace StudioScor.StateMachine
{
    [CreateAssetMenu(menuName = "StudioScor/StateMachine/Decisions/new Check Loop", fileName = "Check_Loop_")]
    public class CheckLoop : Decision
    {
        [Header(" [ Check Loop ] ")]
        [SerializeField] private bool _UseInfinity = false;
        [SerializeField][SCondition(nameof(_UseInfinity),  true, true)] private int _Loop = 2;

        private Dictionary<StateMachineComponent, int> _Data;

        public override void EnterDecide(StateMachineComponent stateMachine)
        {
            if (_Data is null)
                _Data = new();

            if (_Data.ContainsKey(stateMachine))
            {
                _Data[stateMachine] = _Data[stateMachine] + 1;
            }
            else
            {
                _Data.Add(stateMachine, 0);
            }
        }

        public override bool Decide(StateMachineComponent stateMachine)
        {
            if (_UseInfinity)
                return _IsInverse;

            if (_Data.TryGetValue(stateMachine, out int value) && value < _Loop)
                return _IsInverse;

            _Data.Remove(stateMachine);

            return !_IsInverse;
        }
    }
}

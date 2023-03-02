using UnityEngine;
using System.Collections.Generic;
using StudioScor.Utilities;

namespace StudioScor.StateMachine
{

    [CreateAssetMenu(menuName ="StudioScor/StateMachine/Decisions/new Check Delay", fileName = "Check_Delay")]
    public class CheckDelay : Decision
    {
        [Header(" [ Check Delay ] ")]
        [SerializeField, Min(0f)] private float _Delay = 2f;
        [SerializeField] private bool _UseRange = true;
        [SerializeField][SCondition(nameof(_UseRange))] private float _MinRange = -0.5f;
        [SerializeField][SCondition(nameof(_UseRange))] private float _MaxRange = 0.5f;


        private Dictionary<StateMachineComponent, float> _Data;

        public override void EnterDecide(StateMachineComponent stateMachine)
        {
            if (_Data is null)
                _Data = new Dictionary<StateMachineComponent, float>();

            float delay = _UseRange ? _Delay + Random.Range(_MinRange, _MaxRange) : _Delay;
            
            if (_Data.ContainsKey(stateMachine))
            {
                _Data[stateMachine] = delay;
            }
            else
            {
                _Data.Add(stateMachine, delay);
            }

            Log("Delay Time - " + delay.ToString("N1"));
        }

        public override void ExitDecide(StateMachineComponent stateMachine)
        {
            _Data.Remove(stateMachine);
        }

        public override bool Decide(StateMachineComponent stateMachine)
        {
            float delay = _Data[stateMachine];

            Log("State Elapesed - " + stateMachine.StateElapsed.ToString("N1") + " Delay Time - " + delay.ToString("N1"));

            return stateMachine.StateElapsed > delay != _IsInverse;
        }
    }
}

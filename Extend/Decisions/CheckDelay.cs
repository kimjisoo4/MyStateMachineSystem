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

        private Dictionary<StateMachineComponent, float> _Datas;

        protected override void OnReset()
        {
            base.OnReset();

            _Datas = null;
        }

        public override void EnterDecide(StateMachineComponent stateMachine)
        {
            if (_Datas is null)
                _Datas = new();

            float delay = _UseRange ? _Delay + Random.Range(_MinRange, _MaxRange) : _Delay;

            _Datas.TryAdd(stateMachine, delay);

            Log("Delay Time - " + delay.ToString("N1"));
        }

        public override void ExitDecide(StateMachineComponent stateMachine)
        {
            _Datas.Remove(stateMachine);
        }

        public override bool Decide(StateMachineComponent stateMachine)
        {
            if (_Datas.TryGetValue(stateMachine, out float delay))
            {
                Log("State Elapesed - " + stateMachine.StateElapsed.ToString("N1") + " Delay Time - " + delay.ToString("N1"));

                return stateMachine.StateElapsed > delay != _IsInverse;
            }

            return _IsInverse;
        }
    }
}

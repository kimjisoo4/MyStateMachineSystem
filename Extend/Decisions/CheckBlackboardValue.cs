using UnityEngine;

namespace StudioScor.StateMachine
{
    public class CheckBlackboardValue<TKey, TValue> : Decision where TKey : BlackBoardKey<TValue>
    {
        [SerializeField] private TKey _Key;
        [SerializeField] private TValue _Value;

        public override bool Decide(StateMachineComponent stateMachine)
        {
            return _Key.TryGetValue(stateMachine, out TValue value) && _Value.Equals(value);
        }
    }
}

using UnityEngine;

namespace KimScor.StateMachine
{
    public class DoActionDebugLog<T> : Action<T> where T : MonoBehaviour
    {
        [SerializeField] private string _EnterText = "Enter Action";
        [SerializeField] private string _UpdateText = "Update Action";
        [SerializeField] private string _ExitText = "Exit Action";

        public override void EnterAction(StateMachine<T> stateMachine)
        {
            Debug.Log("[" + stateMachine.Owner + "] " + _EnterText);
        }

        public override void ExitAction(StateMachine<T> stateMachine)
        {
            Debug.Log("[" + stateMachine.Owner + "] " + _UpdateText);
        }

        public override void UpdateAction(StateMachine<T> stateMachine)
        {
            Debug.Log("[" + stateMachine.Owner + "] " + _ExitText);
        }
    }
}
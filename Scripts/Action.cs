using UnityEngine;
using System.Collections;
using UnityEditor;

using System.Diagnostics;

namespace StudioScor.StateMachine
{
    public abstract class Action<T> : ScriptableObject where T : MonoBehaviour
    {
        public abstract void EnterAction(StateMachine<T> stateMachine);
        public abstract void UpdateAction(StateMachine<T> stateMachine);
        public abstract void ExitAction(StateMachine<T> stateMachine);


        [Conditional("UNITY_EDITOR")]
        public virtual void DrawGizmos(StateMachine<T> stateMachine)
        {

        }
        [Conditional("UNITY_EDITOR")]
        public virtual void DrawGizmosSelected(StateMachine<T> stateMachine)
        {

        }
    }
}
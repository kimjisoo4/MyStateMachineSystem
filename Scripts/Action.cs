using UnityEngine;
using System.Collections;
using UnityEditor;

using System.Diagnostics;

namespace KimScor.StateMachine
{
    public abstract class Action<T> : ScriptableObject 
    {
        public abstract void EnterAction(StateMachine<T> stateMachine);
        public abstract void UpdateAction(StateMachine<T> stateMachine);
        public abstract void ExitAction(StateMachine<T> stateMachine);


        [Conditional("UNITY_EDITOR")]
        public virtual void DrawGizmos(Transform transform)
        {

        }
        [Conditional("UNITY_EDITOR")]
        public virtual void DrawGizmosSelected(Transform transform)
        {

        }
    }
}
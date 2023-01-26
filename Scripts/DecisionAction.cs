using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.StateMachine
{
    [System.Serializable]
    public class DecisionAction<T> where T : MonoBehaviour
    {
        public string Contents = " Write your explanation here. ";
        public bool isOnce = false;
        public Decisions<T> decisions;
        public List<Action<T>> action = null;

    }

}

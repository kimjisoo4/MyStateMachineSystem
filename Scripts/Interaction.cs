﻿using UnityEngine;

namespace KimScor.StateMachine
{
    public abstract class Interaction<T> : ScriptableObject where T : MonoBehaviour
    {
        public abstract void Interactive(StateMachine<T> onwer, StateMachine<T> target);

    }
}
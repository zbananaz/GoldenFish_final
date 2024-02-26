using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unicorn
{
    public class EventBroker : MonoBehaviour
    {
        public static EventBroker Instance;

        [HideInInspector] public UnityEvent<int> OnTimeChange;
        [HideInInspector] public UnityEvent<int> OnCoinChange;
        [HideInInspector] public UnityEvent OnWin;
        [HideInInspector] public UnityEvent OnLose;

        public void EmitWin()
        {
            OnWin?.Invoke();
        }

        public void EmitLose()
        {
            OnLose?.Invoke();
        }
        public void EmitTimeChange(int totalScecond)
        {
            OnTimeChange?.Invoke(totalScecond);
        }

        public void EmitCoinChange(int coin)
        {
            OnCoinChange?.Invoke(coin);
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance =this;
            }
        }
        
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class Entity : MonoBehaviour
    {
        // Monobehavior parent that able to inherite
        //the mono one
        public FiniteStateMachine stateMachine;
        
        protected virtual void Awake()
        {
            stateMachine = new FiniteStateMachine();
        }

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
            if(stateMachine != null) 
                stateMachine.currentState.OnUpdate();
        }
        protected virtual void FixedUpdate()
        {
            if(stateMachine != null)
                stateMachine.currentState.OnFixedUpdate();
        }
    }
}

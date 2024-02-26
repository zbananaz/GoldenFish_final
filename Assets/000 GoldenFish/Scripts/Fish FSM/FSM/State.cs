using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class State 
    {
        protected FiniteStateMachine stateMachine;
        protected Entity entity;
        
        public State(Entity entity, FiniteStateMachine finiteStateMachine)
        {
            this.entity = entity;
            this.stateMachine = finiteStateMachine;
        }

        public virtual void OnEnter()
        {
        }
        public virtual void OnExit()
        {
        }
        public virtual void OnUpdate()
        {
            
        }
        public virtual void OnFixedUpdate()
        {
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class MoveState : State
    {
        private BaseFish baseFish;
        
        public MoveState(BaseFish entity, FiniteStateMachine finiteStateMachine) : base(entity, finiteStateMachine)
        {
            baseFish = entity;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            baseFish._collider.enabled = true;
            // baseFish.
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            baseFish.OnMoveUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            baseFish.OnMoveFixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
    
    
}

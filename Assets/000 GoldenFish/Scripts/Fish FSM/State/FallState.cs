using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class FallState : State
    {
        private BaseFish baseFish;
        
        public FallState(BaseFish entity, FiniteStateMachine finiteStateMachine) : base(entity, finiteStateMachine)
        {
            baseFish = entity;
        }
        public override void OnEnter()
        {
            baseFish.Fall();
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            baseFish.CheckFallQuitState();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        
    }
}

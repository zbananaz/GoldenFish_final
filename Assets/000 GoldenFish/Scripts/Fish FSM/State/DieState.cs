using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class DieState : State
    {
        private BaseFish _baseFish;
        public DieState(BaseFish entity, FiniteStateMachine finiteStateMachine) : base(entity, finiteStateMachine)
        {
            _baseFish = entity;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }
    }
}

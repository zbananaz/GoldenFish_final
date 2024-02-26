using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class AttackState : State
    {
        private BaseFish baseFish;
        public AttackState(BaseFish entity, FiniteStateMachine finiteStateMachine) : base(entity, finiteStateMachine)
        {
            baseFish = entity;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (baseFish is Balrog)
            {
                var balrog = (Balrog) baseFish;
                balrog.SetAttackAnimation();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            baseFish.OnAttackUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }
    }
}

using Common.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn.FSM
{
    public class InGameAction : UnicornFSMAction
    {
        public InGameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("OnEnter Ingame");
            base.OnEnter();
            LevelManager.Instance.StartLevel();
        }

        public override void OnExit()
        {
            base.OnExit();
            SoundManager.Instance.StopSound(SoundManager.GameSound.BGM);
        }
    }
}
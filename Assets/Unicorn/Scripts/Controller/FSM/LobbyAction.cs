using Common.FSM;
using UnityEngine;

namespace Unicorn.FSM
{
    public class LobbyAction : UnicornFSMAction
    {
        public LobbyAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
        }

        public override void OnEnter()
        {

            Debug.Log("OnEnter Lobby");

            base.OnEnter();

            GameManager.UiController.UiMainLobby.Show(true);
            SoundManager.Instance.PlayFxSound(soundEnum: SoundManager.GameSound.Lobby);
        }

        public override void OnExit()
        {
            base.OnExit();
            GameManager.UiController.UiMainLobby.Show(false);
            SoundManager.Instance.StopSound(SoundManager.GameSound.Lobby);
        }


    }
}
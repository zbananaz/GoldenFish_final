using Common.FSM;
using System.Collections;
using System.Collections.Generic;
// using Firebase.Crashlytics;
using Unicorn;
using Unicorn.FSM;
using UnityEngine;

namespace Unicorn
{
    /// <summary>
    /// Quản lý state trong game.
    /// </summary>
    public class GameFSM : Common.FSM.FSM
    {
        public GameState CurrentGameState { get; private set; }

        public FSMState LobbyGameState => lobbyGameState;
        public FSMState InGameState => inGameState;
        public FSMState EndGameState => endGameState;
        public FSMState ReviveGameState => reviveGameState;
        public FSMState PreparationState => preparationState;

        private FSMState lobbyGameState;
        private LobbyAction lobbyGameAction;

        private FSMState inGameState;
        private InGameAction inGameAction;

        private FSMState endGameState;
        private EndgameAction endGameAction;

        private FSMState reviveGameState;
        private ReviveAction reviveGameAction;

        private FSMState preparationState;

        public GameFSM(GameManager gameController) : base("Game FSM")
        {
            lobbyGameState = AddState((int) GameState.LOBBY);
            inGameState = AddState((int) GameState.IN_GAME);
            endGameState = AddState((int) GameState.END_GAME);

            lobbyGameAction = new LobbyAction(gameController, lobbyGameState);
            inGameAction = new InGameAction(gameController, InGameState);
            endGameAction = new EndgameAction(gameController, endGameState);

            lobbyGameState.AddAction(lobbyGameAction);
            inGameState.AddAction(inGameAction);
            endGameState.AddAction(endGameAction);
        }

        public void ChangeState(GameState state)
        {
            CurrentGameState = state;
            switch (state)
            {
                case GameState.LOBBY:
                    ChangeToState(lobbyGameState);
                    break;
                case GameState.IN_GAME:
                    ChangeToState(InGameState);
                    break;
                case GameState.END_GAME:
                    ChangeToState(endGameState);
                    break;
                default:
                    Debug.LogError($"{state} has not been set up.");
                    break;
            }
        }
    }

}
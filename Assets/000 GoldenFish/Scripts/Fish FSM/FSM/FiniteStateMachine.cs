using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class FiniteStateMachine 
    {
        public State currentState;
        
        public void Initialize(State state)
        {
            currentState = state;
            currentState.OnEnter();
        }


        public void ChangeState(State state)
        {
            //Quit current State
            currentState.OnExit();
            
            //Init to new state
            Initialize(state);
        }
    }
}

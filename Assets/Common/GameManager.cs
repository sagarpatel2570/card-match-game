using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [DefaultExecutionOrder(-1000)]
    public abstract class GameManager<TState> : MonoBehaviour,IGameManager<TState> where TState : Enum
    {
        public IGameState<TState> CurrentState => currentState;

        private IGameState<TState> prevState;
        private IGameState<TState> currentState;
        private Dictionary<TState, IGameState<TState>> states = new();

        private void Update()
        {
            currentState?.UpdateState();
        }

        public void RegisterState(TState key, IGameState<TState> state)
        {
            states[key] = state;
        }
        
        public void UnRegisterState(TState key, IGameState<TState> state)
        {
            if (states.ContainsKey(key))
            {
                states.Remove(key);
            }
        }

        public void ChangeState(TState key)
        {
            prevState = currentState;
            currentState?.Exit();

            if (states.TryGetValue(key, out var newState))
            {
                currentState = newState;
                currentState.Enter();
            }
            else
            {
                Debug.LogError($"State {key} not registered!");
            }
            
            GlobalEvents.Trigger(new GameStateEvent<TState>(){prevState = prevState,currentState = currentState});
        }

    }
    
    public class GameStateEvent<TState> where TState : Enum
    {
        public IGameState<TState> prevState;
        public IGameState<TState> currentState;
    }
}


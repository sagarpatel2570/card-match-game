using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameState<TState> where TState : Enum
{
    public TState Type { get; }
    
    public void Init(IGameManager<TState> gameManager);
    public void Enter();
    public void UpdateState();
    public void Exit();
}

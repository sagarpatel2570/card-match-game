using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager<TState> where TState : Enum
{
    public void ChangeState(TState key);
    public IGameState<TState> CurrentState { get; }
}

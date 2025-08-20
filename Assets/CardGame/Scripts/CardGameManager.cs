using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace CardGame
{
    public enum GameState
    {
        MainMenu = 0,
        LevelSelection = 1,
        GamePlay = 2,
    }
    [DefaultExecutionOrder(-100)]
    public class CardGameManager : GameManager<GameState>
    {
        public List<GameObject> gameStates;
        public GameState defaultGameState = GameState.MainMenu;

        private bool isDataLoaded = false;
        private bool loadGame = false;

        private void Awake()
        {
            GlobalEvents.Register<LoadDataEvent>(OnDataLoaded); 
        }
        
        private IEnumerator Start()
        {
            yield return new WaitWhile(() => isDataLoaded == false);
            Initialize();
        }
        
        private void OnDataLoaded(LoadDataEvent data)
        {
            isDataLoaded = true;
            loadGame = !string.IsNullOrEmpty(data.gameInfo.levelId);
        }

        private void Initialize()
        {
            foreach (var obj in gameStates)
            {
                if (obj.activeSelf)
                {
                    obj.gameObject.SetActive(false);
                }
                var gameState = obj.GetComponent<IGameState<GameState>>();
                if (gameState != null)
                {
                    gameState.Init(this);
                    RegisterState(gameState.Type,gameState);
                }
            }
            
            GlobalEvents.Trigger(new LoadGameInfoEvent());
            if (!loadGame)
            {
                ChangeState(defaultGameState);
            }
            else
            {
                ChangeState(GameState.GamePlay);
            }
        }

        private void OnDestroy()
        {
            GlobalEvents.UnRegister<LoadDataEvent>(OnDataLoaded); 

            foreach (var obj in gameStates)
            {
                if (obj != null)
                {
                    var gameState = obj.GetComponent<IGameState<GameState>>();
                    if (gameState != null)
                    {
                        UnRegisterState(gameState.Type, gameState);
                    }
                }
            }
        }
    }
}


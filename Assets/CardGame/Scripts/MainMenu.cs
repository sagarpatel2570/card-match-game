using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class MainMenu : MonoBehaviour,IGameState<GameState>
    {
        public GameState Type => GameState.MainMenu;
        public Button playButton;

        private IGameManager<GameState> gameManager;

        private void Start()
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            gameManager.ChangeState(GameState.LevelSelection);
        }

        public void Init(IGameManager<GameState> gameManager)
        {
            this.gameManager = gameManager;
        }
        

        public void Enter()
        {
            gameObject.SetActive(true);
        }

        public void UpdateState()
        {
        }

        public void Exit()
        {
            gameObject.SetActive(false);
        }
    }
}


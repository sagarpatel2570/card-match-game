using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class CardGameplayUI : MonoBehaviour
    {
        public Button homeButton;

        private IGameManager<GameState> gameManager;

        public void Init(IGameManager<GameState> gameManager)
        {
            this.gameManager = gameManager;
            
            homeButton.onClick.RemoveAllListeners();
            homeButton.onClick.AddListener(() =>
            {
                gameManager.ChangeState(GameState.MainMenu);
            });
        }
    }
}


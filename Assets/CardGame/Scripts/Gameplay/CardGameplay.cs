using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace CardGame
{
    public class CardGameplay : MonoBehaviour,IGameState<GameState>
    {
        public Board board;
        public LevelData CurrLevelData;
        public CardGameplayUI gameplayUI;
        public float showTime = 3;

        private Card previousCardSelected;
        private int pairsNeeded;
        private IGameManager<GameState> gameManager;
        
        public GameState Type => GameState.GamePlay;
        
        public void Init(IGameManager<GameState> gameManager)
        {
            this.gameManager = gameManager;
            gameplayUI.Init(gameManager);
            
            GlobalEvents.Register<LevelSelectionEvent>(OnLevelSelected);
        }

        private void OnDestroy()
        {
            GlobalEvents.UnRegister<LevelSelectionEvent>(OnLevelSelected);
        }

        private void OnLevelSelected(LevelSelectionEvent obj)
        {
            CurrLevelData = obj.data;
        }

        public void Enter()
        {
            gameObject.SetActive(true);
            StartCoroutine(StartGameCoroutine());
        }

        public void UpdateState()
        {
        }

        public void Exit()
        {
            Reset();
            gameObject.SetActive(false);
        }

        private IEnumerator StartGameCoroutine()
        {
            SetUpGame();
            foreach (var card in board.CardList)
            {
                card.ChangeState(Card.CardState.Shown,false);
            }
            
            yield return new WaitForSeconds(showTime);
            foreach (var card in board.CardList)
            {
                card.ChangeState(Card.CardState.Hidden);
            }
            
            foreach (var card in board.CardList)
            {
                card.OnCardStateChangeEvent += OnCardStateChange;
            }
        }

        public void Reset()
        {
            board.Reset();
        }

        private void SetUpGame()
        {
            Reset();
            board.GenerateBoard(CurrLevelData.levelInfo);
            pairsNeeded = board.CardList.Count / 2;
        }

        private void OnCardStateChange(Card card, Card.CardState state)
        {
            switch (state)
            {
                case Card.CardState.Hidden:
                    break;
                case Card.CardState.Shown:
                    if (previousCardSelected == null)
                    {
                        previousCardSelected = card;
                    }
                    else
                    {
                        if (card.CardInfo.Id == previousCardSelected.CardInfo.Id)
                        {
                            pairsNeeded--;
                            if (pairsNeeded <= 0)
                            {
                                StartCoroutine(FinishGameCoroutine(card, previousCardSelected));
                            }
                            StartCoroutine(WaitAndChangeStateCoroutine(card, previousCardSelected, Card.CardState.Matched,false));
                            
                            // trigger global card match event
                        }
                        else
                        {
                            StartCoroutine(WaitAndChangeStateCoroutine(card, previousCardSelected, Card.CardState.Hidden,true));
                        }
                        previousCardSelected = null;
                    }
                    break;
                case Card.CardState.Matched:
                    break;
            }
        }
        
        private IEnumerator WaitAndChangeStateCoroutine(Card card1, Card card2, Card.CardState state,bool wrongPair)
        {
            yield return new WaitWhile(() => card1.IsAnimating == true);
            yield return new WaitWhile(() => card2.IsAnimating == true);

            yield return new WaitForSeconds(0.5f);

            if (wrongPair)
            {
                GlobalEvents.Trigger(new WrongPairEvent());
            }
            else
            {
                GlobalEvents.Trigger(new RightPairEvent());
            }

            card1.ChangeState(state);
            card2.ChangeState(state);
        }

        private IEnumerator FinishGameCoroutine(Card card1, Card card2)
        {
            yield return new WaitWhile(() => card1.State != Card.CardState.Matched);
            yield return new WaitWhile(() => card2.State != Card.CardState.Matched);
            
            yield return new WaitWhile(() => card1.IsAnimating == true);
            yield return new WaitWhile(() => card2.IsAnimating == true);

            yield return new WaitForSeconds(0.5f);
            
            Debug.Log($"Game Finished");
            GlobalEvents.Trigger(new GameFinishEvent());
        }

        public void NextLevel()
        {
            GlobalEvents.Trigger(new NextLevelEvent());
        }

        public void MainMenu()
        {
            gameManager.ChangeState(GameState.MainMenu);
        }
        
    }
}


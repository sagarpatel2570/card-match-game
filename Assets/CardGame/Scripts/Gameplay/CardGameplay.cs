using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace CardGame
{
    public class CardGameplay : MonoBehaviour,IGameState<GameState>,ISave,ILoad
    {
        public Board board;
        public LevelData CurrLevelData;
        public CardGameplayUI gameplayUI;
        public float showTime = 3;
        public AudioClip rightPairSfx;
        public AudioClip wrongPairSfx;
        public AudioClip gameOverSfx;

        private Card previousCardSelected;
        private int pairsNeeded;
        private IGameManager<GameState> gameManager;
        public ScoreHandler ScoreHandler => scoreHandler;
        private ScoreHandler scoreHandler;
        
        private List<GameInfo.CardInfo> cardInfo = new List<GameInfo.CardInfo>();
        
        public GameState Type => GameState.GamePlay;
        
        public void Init(IGameManager<GameState> gameManager)
        {
            this.gameManager = gameManager;
            scoreHandler = GetComponent<ScoreHandler>();
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
            bool showHiddenCard = cardInfo.Count <= 0;
            SetUpGame();

            if (showHiddenCard)
            {
                foreach (var card in board.CardList)
                {
                    card.ChangeState(Card.CardState.Shown, false);
                }

                yield return new WaitForSeconds(showTime);
                foreach (var card in board.CardList)
                {
                    card.ChangeState(Card.CardState.Hidden);
                }
            }
            else
            {
                foreach (var card in board.CardList)
                {
                    card.ChangeState(Card.CardState.Hidden,false);
                }
            }
            
            foreach (var card in board.CardList)
            {
                card.OnCardStateChangeEvent += OnCardStateChange;
            }
        }

        public void Reset()
        {
            board.Reset();
            cardInfo.Clear();
        }

        private void SetUpGame()
        {
            board.GenerateBoard(CurrLevelData.levelInfo);
            
            if (cardInfo.Count <= 0)
            {
                foreach (var card in board.CardList)
                {
                    cardInfo.Add(new GameInfo.CardInfo() { ID = card.CardInfo.Id, state = 0 });
                }
                pairsNeeded = board.CardList.Count / 2;
                GlobalEvents.Trigger(new SaveGameInfoEvent());
            }
            else
            {
                for (var index = 0; index < board.CardList.Count; index++)
                {
                    var card = board.CardList[index];
                    var info = board.GetCardInfo(cardInfo[index].ID);
                    card.Init(info,index + 1);
                    card.ChangeState((Card.CardState)cardInfo[index].state,false);
                }
            }

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

            yield return new WaitForSeconds(0.2f);

            if (wrongPair)
            {
                SoundManager.Instance.PlaySfx(wrongPairSfx);
                GlobalEvents.Trigger(new WrongPairEvent());
            }
            else
            {
                cardInfo[card1.CardNo - 1].state = (int)Card.CardState.Matched;
                cardInfo[card2.CardNo - 1].state = (int)Card.CardState.Matched;
                
                SoundManager.Instance.PlaySfx(rightPairSfx);
                GlobalEvents.Trigger(new RightPairEvent());
            }

            card1.ChangeState(state);
            card2.ChangeState(state);
            
            GlobalEvents.Trigger(new SaveGameInfoEvent());
        }

        private IEnumerator FinishGameCoroutine(Card card1, Card card2)
        {
            yield return new WaitWhile(() => card1.State != Card.CardState.Matched);
            yield return new WaitWhile(() => card2.State != Card.CardState.Matched);
            
            yield return new WaitWhile(() => card1.IsAnimating == true);
            yield return new WaitWhile(() => card2.IsAnimating == true);

            yield return new WaitForSeconds(0.5f);
            
            SoundManager.Instance.PlaySfx(gameOverSfx);
            GlobalEvents.Trigger(new GameFinishEvent(){isCompleted = true});
        }

        public void NextLevel()
        {
            GlobalEvents.Trigger(new NextLevelEvent());
        }

        public void MainMenu()
        {
            gameManager.ChangeState(GameState.MainMenu);
        }

        public void Save(GameInfo gameInfo)
        {
            gameInfo.cardInfos = cardInfo;
            gameInfo.pairsNeeded = pairsNeeded;
        }

        public void Load(GameInfo gameInfo)
        {
            cardInfo = gameInfo.cardInfos;
            pairsNeeded = gameInfo.pairsNeeded;
        }
    }
}


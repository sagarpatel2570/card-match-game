using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class CardGameplayManager : MonoBehaviour
    {
        public Board board;
        
        private Card previousCardSelected;
        private int pairsNeeded;

        private IEnumerator Start()
        {
            SetUpGame();
            yield return new WaitForSeconds(5);
            foreach (var card in board.CardList)
            {
                card.ChangeState(Card.CardState.Hidden);
            }
        }

        private void SetUpGame()
        {
            board.GenerateBoard();
            foreach (var card in board.CardList)
            {
                card.OnCardStateChangeEvent += OnCardStateChange;
            }
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
                            StartCoroutine(WaitAndChangeStateCoroutine(card, previousCardSelected, Card.CardState.Matched));
                            
                            // trigger global card match event
                        }
                        else
                        {
                            StartCoroutine(WaitAndChangeStateCoroutine(card, previousCardSelected, Card.CardState.Hidden));
                        }
                        previousCardSelected = null;
                    }
                    break;
                case Card.CardState.Matched:
                    break;
            }
        }
        
        private IEnumerator WaitAndChangeStateCoroutine(Card card1, Card card2, Card.CardState state)
        {
            yield return new WaitWhile(() => card1.IsAnimating == true);
            yield return new WaitWhile(() => card2.IsAnimating == true);

            yield return new WaitForSeconds(0.5f);
            
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
            
            // trigger global game finish event
            Debug.Log($"Game Finished");
        }
    }
}


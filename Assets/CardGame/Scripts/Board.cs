using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CardGame
{
    public class Board : MonoBehaviour
    {
        public int sizeX;
        public int sizeY;
        public RectTransform bg;
        public GridLayoutGroup grid;
        public CardData cardData;
        public Card cardPrefab;

        public List<Card> CardList => cards;
        private List<Card> cards = new List<Card>();
        private Vector2 cardSize;

        private void Awake()
        {
            // GenerateBoard();
        }

        public void GenerateBoard()
        {
            int size = Mathf.Max(sizeX, sizeY);
            cardSize = (bg.sizeDelta - (grid.spacing * 2) - (grid.spacing * (size - 1))) / size;
            grid.cellSize = cardSize;

            grid.constraintCount = sizeY;

            int totalPairs = (sizeX * sizeY) / 2;
            int cardSelectionCount = Random.Range(2, Mathf.Min(totalPairs, cardData.cardInfos.Count));
            var cardInfos = cardData.GetRandomCard(cardSelectionCount);

            List<CardInfo> cardId = new List<CardInfo>();
            for (int i = 0; i < totalPairs; i++)
            {
                var cardInfo = cardInfos[i % cardInfos.Count];
                cardId.Add(cardInfo);
                cardId.Add(cardInfo);
            }
            cardId.Shuffle();

            int cardNo = 0;
            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    var card = Instantiate(cardPrefab, grid.transform);
                    card.name = $"card_{cardNo + 1}";
                    var info = cardId[cardNo];
                    cardNo++;
                    card.Init(info);
                    card.ChangeState(Card.CardState.Shown);
                    cards.Add(card);
                }
            }

        }
        
        
    }
}

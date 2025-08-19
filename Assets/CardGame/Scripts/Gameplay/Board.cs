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

        public void Reset()
        {
            foreach (var c in cards)
            {
                DestroyImmediate(c.gameObject);
            }
            cards.Clear();
        }

        public void GenerateBoard(LevelInfo levelInfo)
        {
            int size = Mathf.Max(levelInfo.sizeX, levelInfo.sizeY);
            cardSize = (bg.sizeDelta - (grid.spacing * 2) - (grid.spacing * (size - 1))) / size;
            grid.cellSize = cardSize;

            grid.constraintCount =levelInfo.sizeY;

            int totalPairs = (levelInfo.sizeX * levelInfo.sizeY) / 2;
            int cardSelectionCount = Random.Range(levelInfo.noOfCardVariation, Mathf.Min(totalPairs, cardData.cardInfos.Count));
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
            for (int i = 0; i < levelInfo.sizeY; i++)
            {
                for (int j = 0; j < levelInfo.sizeX; j++)
                {
                    var card = Instantiate(cardPrefab, grid.transform);
                    card.name = $"card_{cardNo + 1}";
                    var info = cardId[cardNo];
                    cardNo++;
                    card.Init(info);
                    card.ChangeState(Card.CardState.Hidden,false);
                    cards.Add(card);
                }
            }

        }
        
        
    }
}

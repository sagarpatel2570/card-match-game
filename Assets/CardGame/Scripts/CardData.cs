using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(menuName = "CardGame/CardData")]
    public class CardData : ScriptableObject
    {
        public List<CardInfo> cardInfos;

        public CardInfo GetCardInfo(string Id)
        {
            return cardInfos.Find(a => a.Id == Id);
        }

        public List<CardInfo> GetRandomCard(int count)
        {
            cardInfos.Shuffle();
            return cardInfos.Take(count).ToList();
        }
    }

    [Serializable]
    public class CardInfo
    {
        public string Id;
        public Sprite visual;
    }
}

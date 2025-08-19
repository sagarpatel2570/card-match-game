using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [System.Serializable]
    public class GameInfo
    {
        public string levelId;
        public List<CardInfo> cardInfos;
        public int totalPoints;
        public int pairsNeeded;
        
        public GameInfo()
        {
            cardInfos = new List<CardInfo>();
        }
        
        [System.Serializable]
        public class CardInfo
        {
            public string ID;
            public int state;
        }
    }
}


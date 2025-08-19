using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(menuName = "CardGame/Level")]
    public class LevelData : ScriptableObject
    {
        public LevelInfo levelInfo;
    }

    [Serializable]
    public class LevelInfo
    {
        public int levelNo;
        public int sizeX;
        public int sizeY;
        
        public int noOfCardVariation;
    }
}

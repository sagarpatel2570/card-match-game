using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class LevelSelectionEvent
    {
        public LevelData data;
    }

    public class NextLevelEvent
    {
    }
    
    public class GameFinishEvent
    {
        public bool isCompleted;
    }

    public class WrongPairEvent
    {
    }

    public class RightPairEvent
    {
    }
    
    public class SaveGameInfoEvent{}

    public class LoadDataEvent
    {
        public GameInfo gameInfo;
    }
    
}


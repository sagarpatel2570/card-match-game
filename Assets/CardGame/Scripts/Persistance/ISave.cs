using System.Collections;
using System.Collections.Generic;
using CardGame;
using UnityEngine;

namespace CardGame
{
    public interface ISave
    {
        public void Save(GameInfo gameInfo);
    }
}


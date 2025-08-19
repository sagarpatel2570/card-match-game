using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace CardGame
{
    public class ScoreHandler : MonoBehaviour
    {
        public float comboTime = 3;
        public int wrongPairPoint = -2;
        public int rightPairPoint = 4;

        public int TotalPoint => totalPoint;
        private int totalPoint;

        public int ComboNum => comboNum;  
        private int comboNum = 0;
        private float currComboTime;

        public event Action<int> OnComboEvent;
        public event Action<int,int> OnScoreChangeEvent;

        private void Start()
        {
            GlobalEvents.Register<GameStateEvent<GameState>>(GameStateChange);
            GlobalEvents.Register<WrongPairEvent>(OnWrongPair);
            GlobalEvents.Register<RightPairEvent>(OnRightPair);
        }

        private void Update()
        {
            if (comboNum <= 0)
            {
                return;
            }

            currComboTime -= Time.deltaTime;
            if (currComboTime <= 0)
            {
                comboNum = 0;
                OnComboEvent?.Invoke(comboNum);
            }
        }

        private void OnWrongPair(WrongPairEvent obj)
        {
            totalPoint += wrongPairPoint;
            totalPoint = Mathf.Max(0, totalPoint);
            
            OnScoreChangeEvent?.Invoke(totalPoint,wrongPairPoint);
        }
        
        private void OnRightPair(RightPairEvent obj)
        {
            comboNum++;
            currComboTime = comboTime;
            
            int pointToAdd = rightPairPoint * comboNum;
            totalPoint += pointToAdd;
            
            OnScoreChangeEvent?.Invoke(totalPoint,pointToAdd);
            OnComboEvent?.Invoke(comboNum);
            
        }

        private void OnDestroy()
        {
            GlobalEvents.UnRegister<GameStateEvent<GameState>>(GameStateChange);
        }

        private void GameStateChange(GameStateEvent<GameState> state)
        {
            if (state.currentState.Type == GameState.GamePlay)
            {
                Reset();
            }
        }

        private void Reset()
        {
            totalPoint = 0;
            currComboTime = 0;
            comboNum = 0;
        }
    }
}


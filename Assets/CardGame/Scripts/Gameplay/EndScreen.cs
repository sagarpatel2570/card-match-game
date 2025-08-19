using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class EndScreen : MonoBehaviour
    {
        public EndScreenUI endScreenUI;
        public CardGameplay gameplay;
        
        private void OnEnable()
        {
            GlobalEvents.Register<GameFinishEvent>(OnGameFinish);
        }

        private void OnGameFinish(GameFinishEvent obj)
        {
            if (obj.isCompleted)
            {
                endScreenUI.Show(ProceedToMainMenu, NextLevel);
            }
        }

        private void ProceedToMainMenu()
        {
            HideEndScreenUI();
            gameplay.MainMenu();
        }

        private void NextLevel()
        {
            HideEndScreenUI();
            gameplay.NextLevel();
        }

        public void HideEndScreenUI()
        {
            endScreenUI.Hide();
        }


        private void OnDisable()
        {
            GlobalEvents.UnRegister<GameFinishEvent>(OnGameFinish);
        }
    }
}


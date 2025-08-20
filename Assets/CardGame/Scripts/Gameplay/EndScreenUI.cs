using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class EndScreenUI : MonoBehaviour
    {
        public TextMeshProUGUI pointText;
        public Button homeButton;
        public Button nextLevelButton;

        public void Show(int points, Action onHomeClickEvent,Action nextLevelClickEvent)
        {
            gameObject.SetActive(true);
            pointText.text =  $"Score:\n{points}";
            
            homeButton.onClick.RemoveAllListeners();
            homeButton.onClick.AddListener(() =>
            {
                onHomeClickEvent?.Invoke();
                Hide();
            });
            
            nextLevelButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.AddListener(() =>
            {
                nextLevelClickEvent?.Invoke();
                Hide();
            });
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}


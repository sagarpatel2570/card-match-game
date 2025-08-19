using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class EndScreenUI : MonoBehaviour
    {
        public Button homeButton;
        public Button nextLevelButton;

        public void Show(Action onHomeClickEvent,Action nextLevelClickEvent)
        {
            gameObject.SetActive(true);
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


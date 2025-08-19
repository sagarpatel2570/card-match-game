using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class LevelInfoUI : MonoBehaviour
    {
        public Button button;
        public TextMeshProUGUI levelNameText;
        
        
        public void Init(LevelData data,Action<LevelData> onClick)
        {
            gameObject.SetActive(true);
            
            levelNameText.text = $"{data.levelInfo.sizeX} x {data.levelInfo.sizeY}";
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                onClick?.Invoke(data);
            });
        }
    }
}

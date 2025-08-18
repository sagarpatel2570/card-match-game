using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class Card : MonoBehaviour
    {
        private CardInfo info;
        public GameObject hiddenGo;
        public GameObject visibleGo;
        public Image image;

        public void Init(CardInfo info)
        {
            this.info = info;
            gameObject.SetActive(true);
            image.sprite = info.visual;
            visibleGo.SetActive(true);
        }
    }

    
}

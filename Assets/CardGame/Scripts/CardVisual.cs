using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class CardVisual : MonoBehaviour,ICardVisual
    {
        public GameObject hiddenGo;
        public GameObject visibleGo;
        public Image image;
        public CanvasGroup canvasGroup;

        public void Init(CardInfo info)
        {
            image.sprite = info.visual;
            visibleGo.SetActive(true);
        }

        public void ShowCard(Action onComplete)
        {
            StartCoroutine(RotateCard(true, onComplete));
        }

        public void HideCard(Action onComplete)
        {
            StartCoroutine(RotateCard(false, onComplete));
        }

        public void MatchCard(Action onComplete)
        {
            canvasGroup.DOFade(0, 1).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }

        private IEnumerator RotateCard(bool show, Action onComplete) 
        {
            if (show)
            {
                var obj = hiddenGo;
                for (float i = 0f; i <= 180f; i += 10f)
                {
                    obj.transform.rotation = Quaternion.Euler(0f, i, 0f);
                    if (i == 90f)
                    {
                        hiddenGo.gameObject.SetActive(false);
                        visibleGo.gameObject.SetActive(true);
                        visibleGo.transform.rotation = Quaternion.Euler(0f, i, 0f);
                        obj = visibleGo;
                    }
                    yield return new WaitForSeconds(0.03f);
                }
            }
            else
            {
                var obj = visibleGo;
                for (float i = 180f; i >= 0f; i -= 10f)
                {
                    obj.transform.rotation = Quaternion.Euler(0f, i, 0f);
                    if (i == 90f)
                    {
                        hiddenGo.gameObject.SetActive(true);
                        hiddenGo.transform.rotation = Quaternion.Euler(0f, i, 0f);
                        obj = hiddenGo;

                        visibleGo.gameObject.SetActive(false);
                    }
                    yield return new WaitForSeconds(0.03f);
                }
            }
            onComplete?.Invoke();
        }
    }
}

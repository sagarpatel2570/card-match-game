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

        private Tween punchScaleTween;
        private Tween punchRotateTween;
        private bool hideCardWithOutPunch = true;
        private WaitForSeconds rotationWait = new WaitForSeconds(0.03f);

        public void Init(CardInfo info)
        {
            image.sprite = info.visual;
            visibleGo.SetActive(true);
        }

        public void ShowCard(bool animate = true, Action onComplete = null)
        {
            if (punchScaleTween != null)
            {
                punchScaleTween.Kill();
            }

            punchScaleTween = transform.DOPunchScale(Vector3.one * 0.1f, 0.5f);
            
            if (animate)
            {
                StartCoroutine(RotateCardCoroutine(true, onComplete));
            }
            else
            {
                hiddenGo.gameObject.SetActive(false);
                visibleGo.gameObject.SetActive(true);
                visibleGo.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                onComplete?.Invoke();
            }
        }

        public void HideCard(bool animate = true, Action onComplete = null)
        {
            if (animate)
            {
                if (punchRotateTween != null)
                {
                    punchRotateTween.Kill();
                }

                if (!hideCardWithOutPunch)
                {
                    image.DOColor(Color.red, 0.25f).OnComplete(() => { image.DOColor(Color.white, 0.25f); });

                    punchRotateTween = transform.DOPunchRotation(Vector3.forward * 10f, 0.5f).OnComplete(() =>
                    {
                        StartCoroutine(RotateCardCoroutine(false, onComplete));
                    });
                }
                else
                {
                    hideCardWithOutPunch = false;
                    StartCoroutine(RotateCardCoroutine(false, onComplete));
                }
            }
            else
            {
                hideCardWithOutPunch = false;
                hiddenGo.gameObject.SetActive(true);
                hiddenGo.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                visibleGo.gameObject.SetActive(false);
                onComplete?.Invoke();
            }
        }

        public void MatchCard(bool animate = true, Action onComplete = null)
        {
            if (animate)
            {
                StartCoroutine(RotateAntiClockwiseCoroutine());
                canvasGroup.DOFade(0, 1).OnComplete(() => { onComplete?.Invoke(); });
            }
            else
            {
                canvasGroup.alpha = 0;
                onComplete?.Invoke();
            }
        }

        private IEnumerator RotateAntiClockwiseCoroutine()
        {
            hiddenGo.gameObject.SetActive(false);
            for (float i = 180f; i >= -180; i -= 10f)
            {
                visibleGo.transform.rotation = Quaternion.Euler(0f, i, 0f);
                yield return rotationWait;
            }
        }

        private IEnumerator RotateCardCoroutine(bool show, Action onComplete) 
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
                    yield return rotationWait;
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
                    yield return rotationWait;
                }
            }
            onComplete?.Invoke();
        }
    }
}

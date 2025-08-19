using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CardGame
{
    public class ScoreUI : MonoBehaviour
    {
        public ScoreHandler scoreHandler;
        
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI comboText;

        public TextMeshProUGUI currScoreTextPrefab;

        private int currPoint;
        private Tween pointTween;
        private Tween pointPunchTween;

        private void OnEnable()
        {
            scoreHandler.OnComboEvent += OnCombo;
            scoreHandler.OnScoreChangeEvent += OnScoreChanged;
            
            comboText.gameObject.SetActive(false);
            if (pointTween != null)
            {
                pointTween.Kill();
            }
            currPoint = 0;
            scoreText.text = scoreHandler.TotalPoint.ToString();
            comboText.text = scoreHandler.ComboNum.ToString();
        }

        private void OnScoreChanged(int totalPoint, int pointAdded)
        {
            if (pointTween != null)
            {
                pointTween.Kill();
            }
            pointTween = DOTween.To(() => currPoint, x => {
                currPoint = x;
            }, totalPoint, 1f).OnUpdate(() =>
            {
                scoreText.text = currPoint.ToString();
            });

            if (pointPunchTween != null)
            {
                pointPunchTween.Kill();
                scoreText.transform.localScale = Vector3.one;
            }

            pointPunchTween = scoreText.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f);

            var txt = Instantiate(currScoreTextPrefab, transform);
            txt.gameObject.SetActive(true);
            txt.transform.position = comboText.transform.position;
            if (pointAdded > 0)
            {
                txt.text = $"+{pointAdded}";
            }
            else
            {
                txt.text = $"{pointAdded}";
                txt.color = Color.red;
            }

            txt.transform.DOMove(scoreText.transform.position, 1).OnComplete(() =>
            {
                Destroy(txt.gameObject);
            });
            txt.DOFade(0, 2);

        }

        private void OnCombo(int comboNum)
        {
            comboText.gameObject.SetActive(comboNum > 0);
            if (comboNum > 1)
            {
                comboText.text = $"{comboNum}X Combo";
            }
            else
            {
                comboText.text = "";
            }
        }

        private void OnDisable()
        {
            scoreHandler.OnComboEvent -= OnCombo;
            scoreHandler.OnScoreChangeEvent -= OnScoreChanged;
        }
    }
}


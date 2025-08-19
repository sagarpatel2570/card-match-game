using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CardGame
{
    public class Card : MonoBehaviour,IPointerClickHandler
    {
        public enum CardState
        {
            Hidden,
            Shown,
            Matched,
        }
        
        public CardState State { get; private set; }
        public bool IsAnimating { get; private set; }
        public event Action<Card,CardState> OnCardStateChangeEvent;

        public CardInfo CardInfo => info;
        private CardInfo info;
        private ICardVisual cardVisual;

        public void Init(CardInfo info)
        {
            this.info = info;
            gameObject.SetActive(true);

            if (cardVisual == null)
            {
                cardVisual = GetComponent<ICardVisual>();
            }
            cardVisual.Init(this.info);
        }

        public void ChangeState(CardState state,bool animate = true)
        {
            State = state;
            switch (state)
            {
                case CardState.Hidden:
                    IsAnimating = true;
                    cardVisual.HideCard(animate,() =>
                    {
                        IsAnimating = false;
                    });
                    break;
                case CardState.Shown:
                    IsAnimating = true;
                    cardVisual.ShowCard(animate,() =>
                    {
                        IsAnimating = false;
                    });
                    break;
                case CardState.Matched:
                    IsAnimating = true;
                    cardVisual.MatchCard(animate,() =>
                    {
                        IsAnimating = false;
                    });
                    break;
            }
            
            OnCardStateChangeEvent?.Invoke(this,State);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsAnimating)
            {
                return;
            }

            if (State == CardState.Matched)
            {
                return;
            }

            if (State == CardState.Hidden)
            {
                ChangeState(CardState.Shown);
            }
        }
    }

    
}

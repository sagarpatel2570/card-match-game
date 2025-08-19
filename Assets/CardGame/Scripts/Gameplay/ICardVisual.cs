using System;

namespace CardGame
{
    public interface ICardVisual
    {
        public void Init(CardInfo info);
        public void ShowCard(Action onComplete);
        public void MatchCard(Action onComplete);
        public void HideCard(Action onComplete);
    }
}

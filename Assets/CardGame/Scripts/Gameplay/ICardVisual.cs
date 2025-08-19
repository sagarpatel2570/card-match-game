using System;

namespace CardGame
{
    public interface ICardVisual
    {
        public void Init(CardInfo info);
        public void ShowCard(bool animate = true, Action onComplete = null);
        public void MatchCard(bool animate = true, Action onComplete = null);
        public void HideCard(bool animate = true, Action onComplete = null);
    }
}


namespace CardSystem
{
    public class CardPair
    {
        public delegate void PairIsReady( CardModel left, CardModel right );

        public event PairIsReady OnPairIsReady;

        private CardModel left;
        private CardModel right;

        public void AddPair( CardModel cardView )
        {
            if( IsPairReadyToCheck() )
            {
                return;
            }

            if( left == null )
            {
                left = cardView;
            } else
            {
                right = cardView;
            }

            if( IsPairReadyToCheck() )
            {
                OnPairIsReady?.Invoke(left, right);
            }
        }

        public bool IsEqual()
        {
            return IsPairReadyToCheck() && left.Sprite.name == right.Sprite.name;
        }

        public bool IsPairReadyToCheck()
        {
            return left != null && right != null;
        }

        public void FlushPair()
        {
            left = null;
            right = null;
        }
    }
}
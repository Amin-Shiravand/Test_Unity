using UnityEngine;
using Utils;

namespace CardSystem
{
    public class CardManager : MonoBehaviorSingleton<CardManager>
    {
        private const string CARD_PATH = "Prefabs/Card";
        private CardView[] cardViews = null;
        
        public void SetupCards( Transform root, RectTransform panel, int gridSize = 2 )
        {
            Debug.Assert(gridSize > 1, "Grid size should  be greater than 1");
            // if game is odd, we should have 1 card less
            int isOdd = gridSize % 2;
            int totalCards = gridSize * gridSize - isOdd;
            cardViews = new CardView[totalCards];
            if( !PoolManager.Instance.HasPool(CARD_PATH) )
            {
                PoolManager.Instance.CreatePool(CARD_PATH, totalCards);
            }
        
            PoolManager.Instance.ReturnAllObjectToPool(CARD_PATH);
            // calculate position between each card & start position of each card based on the Panel
        
            Vector2 sizeDelta = panel.sizeDelta;
            float rowSize = sizeDelta.x;
            float colSize = sizeDelta.y;
            float scale = 1.0f / gridSize;
            float xInc = rowSize / gridSize;
            float yInc = colSize / gridSize;
            float curX = -xInc * ( float )( gridSize / 2 );
            float curY = -yInc * ( float )( gridSize / 2 );
        
            if( isOdd == 0 )
            {
                curX += xInc / 2;
                curY += yInc / 2;
            }
        
            float initialX = curX;
            // for each in y-axis
            for( int i = 0; i < gridSize; i++ )
            {
                curX = initialX;
                for( int j = 0; j < gridSize; j++ )
                {
                    GameObject cardObject;
                    if( isOdd == 1 && i == ( gridSize - 1 ) && j == ( gridSize - 1 ) )
                    {
                        int index = gridSize / 2 * gridSize + gridSize / 2;
                        cardObject = cardViews[index].gameObject;
                    } else
                    {
                        // create card prefab
                        cardObject = PoolManager.Instance.GetObjectFromPool(CARD_PATH);
                        // assign parent
                        cardObject.transform.SetParent(root.transform);
        
                        int index = i * gridSize + j;
                        CardView cardView = cardObject.GetComponent<CardView>();
                        cardView.Init();
                        cardView.CardModel.ID = index;
                        cardViews[index] = cardView;
        
                        // modify its size
                        cardObject.transform.localScale = new Vector3(scale, scale);
                    }
        
                    // assign location
                    cardObject.transform.localPosition = new Vector3(curX, curY, 0);
                    curX += xInc;
                }
        
                curY += yInc;
            }
        }
    }
}
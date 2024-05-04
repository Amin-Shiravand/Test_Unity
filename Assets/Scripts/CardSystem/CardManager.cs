using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace CardSystem
{
    public class CardManager : MonoBehaviorSingleton<CardManager>
    {
        private const string CARD_PATH = "Prefabs/Card";
        private CardView[] cardViews = null;
        private Sprite[] cardSprites = null;
        private Sprite cardBack = null;

        public void LoadCardSprites()
        {
            cardSprites = GameResourceManager.Instance.LoadAllCards();
            Debug.Assert(cardSprites!=null && cardSprites.Length!=0, "Card Icons collection is null or empty");
            cardBack = GameResourceManager.Instance.LoadSprite("Sprites/Cards/Background/Back");
            Debug.Assert(cardBack!=null, "Background of card is null");
        }

        public void SetupSpritesOnCards()
        {
            int maxIndex = cardSprites.Length;
            HashSet<int> cardWithSprite = new HashSet<int>();
    
            int pairCount = cardViews.Length / 2;
    
            for (int i = 0; i < pairCount; ++i)
            {
                int index = Random.Range(0, maxIndex); // Ensure it can pick the last sprite too
        
                int pair = 0;
                while (pair < 2 && cardWithSprite.Count < cardViews.Length)
                {
                    int randomPosition = Random.Range(0, cardViews.Length);
                    if (!cardWithSprite.Contains(randomPosition))
                    {
                        cardWithSprite.Add(randomPosition);
                        cardViews[randomPosition].CardModel.Sprite = cardSprites[index];
                        cardViews[randomPosition].SetCard();
                        pair++;
                    }
                }
            }
        }


        
        
        public void SetupCards( Transform root, RectTransform panel, int gridSize = 2 )
        {
            Debug.Assert(gridSize > 1, "Grid size should  be greater than 1");
            int isOdd = gridSize % 2;
            int totalCards = gridSize * gridSize - isOdd;
            
        
            cardViews = new CardView[totalCards];
            if( !PoolManager.Instance.HasPool(CARD_PATH) )
            {
                PoolManager.Instance.CreatePool(CARD_PATH, totalCards);
            }
        
            PoolManager.Instance.ReturnAllObjectToPool(CARD_PATH);
        
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
            for( int column = 0; column < gridSize; column++ )
            {
                curX = initialX;
                for( int row = 0; row < gridSize; row++ )
                {
                    GameObject cardObject;
                    if( isOdd == 1 && column == ( gridSize - 1 ) && row == ( gridSize - 1 ) )
                    {
                        int index = gridSize / 2 * gridSize + gridSize / 2;
                        cardObject = cardViews[index].gameObject;
                    } else
                    {
                        cardObject = PoolManager.Instance.GetObjectFromPool(CARD_PATH);
                        cardObject.transform.SetParent(root.transform);
                        int index = column * gridSize + row;
                        CardView cardView = cardObject.GetComponent<CardView>();
                        cardView.Init();
                        cardView.CardModel.Index = index;
                        cardViews[index] = cardView;
        
                        cardObject.transform.localScale = new Vector3(scale, scale);
                    }
        
                    cardObject.transform.localPosition = new Vector3(curX, curY, 0);
                    curX += xInc;
                }
        
                curY += yInc;
            }
        }
    }
}
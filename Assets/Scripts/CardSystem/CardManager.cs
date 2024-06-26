using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Random = UnityEngine.Random;

namespace CardSystem
{
    public class CardManager : MonoBehaviorSingleton<CardManager>
    {
        public delegate void MatchPaired();

        public delegate void CardsAreOver();

        public event MatchPaired onMatchPaired;
        
        public int LeftCards;
        public event CardsAreOver OnCardsAreOver;
        [HideInInspector] public Sprite CardBack = null;
        private const string CARD_PATH = "Prefabs/Card";
        public CardView[] cardViews = null;
        private Sprite[] cardSprites = null;
        private CardPair cardPair = null;
      

        public void AddListeners()
        {
            cardPair = new CardPair();
            cardPair.OnPairIsReady += OnPairIsReady;
            CardView.CardClicked += OnCardClicked;
        }

        public void SetCountOfCards(int count = 0)
        {
            LeftCards = count == 0 ? cardViews.Length : count;
        }

        public void FlushTheCards()
        {
            for( int i = 0; i < cardViews.Length; ++i )
            {
                CardView card = cardViews[i];
                card.ResetRotation();
            }

            PoolManager.Instance.ReturnAllObjectToPool(CARD_PATH);
        }

        public void RemoveListeners()
        {
            if( cardPair != null )
            {
                cardPair.OnPairIsReady -= OnPairIsReady;
            }

            CardView.CardClicked -= OnCardClicked;
        }


        public void LoadCardSprites()
        {
            cardSprites ??= GameResourceManager.Instance.LoadAllCards();
            Debug.Assert(cardSprites != null && cardSprites.Length != 0, "Card Icons collection is null or empty");
            CardBack ??= GameResourceManager.Instance.LoadSprite("Sprites/Cards/Background/Back");
            Debug.Assert(CardBack != null, "Background of card is null");
        }

        public void SetBoardState( CardModel[] cardsModel )
        {
            for( int i = 0; i < cardViews.Length; ++i )
            {
                int spritesIndex = Int32.Parse(cardsModel[i].SpriteName);
                cardViews[i].CardModel.Sprite = cardSprites[spritesIndex - 1];
                cardViews[i].CardModel.SpriteName = cardSprites[spritesIndex -1].name;
                if( !cardsModel[i].IsActive )
                {
                    cardViews[i].CardModel.IsActive = false;
                    cardViews[i].DisableCard();
                }
            }
        }

        public void SetupSpritesOnCards()
        {
            int maxIndex = cardSprites.Length;
            HashSet<int> cardWithSprite = new HashSet<int>();

            int pairCount = cardViews.Length / 2;

            for( int i = 0; i < pairCount; ++i )
            {
                int index = Random.Range(0, maxIndex);

                int pair = 0;
                while( pair < 2 && cardWithSprite.Count < cardViews.Length )
                {
                    int randomPosition = Random.Range(0, cardViews.Length);
                    if( !cardWithSprite.Contains(randomPosition) )
                    {
                        cardWithSprite.Add(randomPosition);
                        cardViews[randomPosition].CardModel.Sprite = cardSprites[index];
                        cardViews[randomPosition].CardModel.SpriteName = cardSprites[index].name;
                        pair++;
                    }
                }
            }

            cardWithSprite.Clear();
        }

        public IEnumerator BoardPreview( float showTime = 0.5f )
        {
            for( int i = 0; i < cardViews.Length; ++i )
            {
                if( !cardViews[i].CardModel.IsActive )
                {
                    continue;
                }
                cardViews[i].ChangeCardView();
            }

            yield return new WaitForSeconds(showTime);
            for( int i = 0; i < cardViews.Length; ++i )
            {
                if( !cardViews[i].CardModel.IsActive )
                {
                    continue;
                }
                cardViews[i].Flip();
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

        private void OnPairIsReady( CardModel left, CardModel right )
        {
            if( cardPair.IsEqual() )
            {
                onMatchPaired?.Invoke();
                AudioManager.Instance.PlayMatchPairs();
                cardViews[left.Index].CardModel.IsActive = false;
                cardViews[right.Index].CardModel.IsActive = false;
                cardViews[left.Index].DisableCard();
                cardViews[right.Index].DisableCard();
                CheckCardsAreOver();
            } else
            {
                AudioManager.Instance.PlayError();
                cardViews[left.Index].Flip();
                cardViews[right.Index].Flip();
            }

            cardPair.FlushPair();
        }

        private void OnCardClicked( CardModel cardmodel )
        {
            Debug.Assert(cardPair != null, "Card pair is null");
            cardPair.AddPair(cardmodel);
        }

        private void CheckCardsAreOver()
        {
            if( LeftCards < 1 )
            {
                return;
            }

            LeftCards -= 2;
            if( LeftCards == 0 )
            {
                OnCardsAreOver?.Invoke();
            }
        }
    }
}
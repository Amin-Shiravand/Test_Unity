using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardSystem
{
    public class CardView : MonoBehaviour
    {
        public CardModel CardModel;
        private Button cardButton;
        
        public void Init()
        {
            cardButton = transform.GetComponent<Button>();
            Debug.Assert(cardButton != null, "Can not find a button instance");
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(OnCardClicked);
            CardModel = new CardModel { Visible = false }; //This is for test
            ResetRotation();
        }

        //Test function to check the sprite images
        public void SetCard()
        {
            cardButton.image.sprite = CardModel.Sprite;
        }

        private void OnCardClicked()
        {
            if( CardModel.Visible || CardModel.Turning )
            {
                return;
            }
            Flip();
        }
        
        private void Flip()
        {
            if( CardModel.Turning )
            {
                return;
            }

            CardModel.Turning = true;
            StartCoroutine(FlipAnimation());
        }

        private IEnumerator FlipAnimation()
        {
            yield return RotateCard(CardModel.Visible ? 90f : 0);
            if( CardModel.Visible )
            {
            }

            CardModel.Visible = !CardModel.Visible;
            yield return RotateCard(CardModel.Visible ? 180f : 0);
            CardModel.Turning = false;
        }

        private IEnumerator RotateCard( float targetAngle = 90, float flipTimeDuration = 0.25f )
        {
            Quaternion startRotation = transform.rotation;
            Quaternion endRotation = Quaternion.Euler(0f, targetAngle, 0f);
            float rate = 1f / flipTimeDuration;
            float stepsCounter = 0f;

            while( stepsCounter < 1f )
            {
                stepsCounter += Time.deltaTime * rate;
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, stepsCounter);
                yield return null;
            }
        }


        private void ResetRotation()
        {
            if( transform.rotation.y == 0 )
            {
                return;
            }
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            CardModel.Visible = false;
        }
    }
}
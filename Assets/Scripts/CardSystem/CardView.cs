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
            CardModel = new CardModel { Hide = false };
            ResetRotation();
        }

        public void ChangeCardView()
        {
            cardButton.image.sprite = !CardModel.Hide ? CardModel.Sprite : CardManager.Instance.CardBack;
        }

        private void OnCardClicked()
        {
            if( !CardModel.Hide || CardModel.Turning )
            {
                return;
            }

            Flip();
        }

        public void Flip()
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
            yield return RotateCard(!CardModel.Hide ? 90f : 0);
            CardModel.Hide = !CardModel.Hide;
            ChangeCardView();
            yield return RotateCard(!CardModel.Hide ? 180f : 0);
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
            CardModel.Hide = true;
        }
    }
}
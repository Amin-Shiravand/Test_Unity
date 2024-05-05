using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CardSystem
{
    public delegate void CardClicked( CardModel cardModel );

    public class CardView : MonoBehaviour
    {
        public static event CardClicked CardClicked;
        public CardModel CardModel;
        private Button cardButton;

        public void Init()
        {
            cardButton = transform.GetComponent<Button>();
            Debug.Assert(cardButton != null, "Can not find a button instance");
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(OnCardClicked);
            cardButton.image.color = Color.white;
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

            Flip(() => CardClicked?.Invoke(CardModel));
        }

        public void Flip( Action onFinalized = null )
        {
            if( CardModel.Turning )
            {
                return;
            }
            
            CardModel.Turning = true;
            AudioManager.Instance.PlayButton();
            if( gameObject.activeInHierarchy )
            {
                StartCoroutine(FlipAnimation(onFinalized));
            }
        }

        private IEnumerator FlipAnimation( Action onFinalize = null )
        {
            yield return RotateCard(!CardModel.Hide ? 90f : 0);
            CardModel.Hide = !CardModel.Hide;
            ChangeCardView();
            yield return RotateCard(!CardModel.Hide ? 180f : 0);
            CardModel.Turning = false;
            onFinalize?.Invoke();
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


        public void ResetRotation()
        {
            if( transform.rotation.y == 0 )
            {
                return;
            }

            transform.rotation = Quaternion.Euler(0f, 0, 0f);
            CardModel.Hide = true;
        }

        public void DisableCard()
        {
            cardButton.onClick.RemoveAllListeners();
            if( gameObject.activeInHierarchy )
            {
                StartCoroutine(Fade());
            }
        }
        
        private IEnumerator Fade()
        {
            float fadeDuration = 2.5f;
            float elapsedTime = 0f;
            Image image = cardButton.image;
            Color startColor = image.color;
            Color endColor = Color.clear;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fadeDuration);
                image.color = Color.Lerp(startColor, endColor, t);
                yield return null;
            }

            image.color = endColor;
        }
    }
}
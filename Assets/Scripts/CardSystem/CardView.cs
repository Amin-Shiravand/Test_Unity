using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public class CardView : MonoBehaviour
    {
        public CardModel CardModel;

        private IEnumerator RotateCard(float targetAngle = 90, float flipTimeDuration = 0.25f )
        {
            Quaternion startRotation = transform.rotation;
            Quaternion endRotation = Quaternion.Euler(0f, targetAngle, 0f);
            float rate = 1f / flipTimeDuration;
            float stepsCounter = 0f;

            while (stepsCounter < 1f)
            {
                stepsCounter += Time.deltaTime * rate;
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, stepsCounter);
                yield return null;
            }
        }
    }
}
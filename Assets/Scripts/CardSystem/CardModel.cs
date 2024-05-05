using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardSystem
{
    [Serializable]
    public class CardModel
    {
        [System.NonSerialized] public Sprite Sprite;
        [System.NonSerialized] public bool Hide;
        [System.NonSerialized] public bool Turning;
        public int Index;
        public string SpriteName;
        public bool IsActive;

        public CardModel()
        {
        }

        public CardModel( int index, Sprite sprite, bool hide, bool turning, bool isActive )
        {
            Index = index;
            Sprite = sprite;
            Hide = hide;
            Turning = turning;
            IsActive = isActive;
        }
    }
}
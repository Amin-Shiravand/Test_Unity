using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardSystem
{
    public class CardModel
    {
        public int Index;
        public Sprite Sprite;
        public bool Visible;
        public bool Turning;

        public CardModel()
        {
        }

        public CardModel( int index, Sprite sprite, bool visible, bool turning )
        {
            Index = index;
            Sprite = sprite;
            Visible = visible;
            Turning = turning;
        }
    }
}
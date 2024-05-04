using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardSystem
{
    public class CardModel
    {
        public int ID;
        public Image Image;
        public bool Visible;
        public bool Turning;

        public CardModel()
        {
        }
        
        public CardModel( int id, Image image, bool visible, bool turning )
        {
            ID = id;
            Image = image;
            Visible = visible;
            Turning = turning;
        }
    }
}
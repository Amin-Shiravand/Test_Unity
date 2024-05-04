using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardSystem
{
    public class CardInfo
    {
        public int ID;
        public Image Image;
        public bool Visible;
        public bool Turning;

        public CardInfo()
        {
        }
        
        public CardInfo( int id, Image image, bool visible, bool turning )
        {
            ID = id;
            Image = image;
            Visible = visible;
            Turning = turning;
        }
    }
}
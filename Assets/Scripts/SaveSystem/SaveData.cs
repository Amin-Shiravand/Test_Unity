using System;
using System.Collections;
using System.Collections.Generic;
using CardSystem;
using UnityEngine;

namespace  SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public int LeftCards;
        public int Score;
        public int BoardSize;
        public float Time;
        public CardModel[] CardModels;
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using CardSystem;
using UnityEngine;
using Utils;

public class GameManager : MonoBehaviorSingleton<GameManager>
{
    [Range(2,9)]
    public int GridSize = 8;
    public RectTransform Board;
    public Transform Root;

    public void Start()
    {
        CardManager.Instance.SetupCards(Root, Board, GridSize);
    }
}
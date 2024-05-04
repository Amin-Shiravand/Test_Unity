using System;
using System.Collections;
using System.Collections.Generic;
using CardSystem;
using UnityEngine;
using Utils;

public class GameManager : MonoBehaviorSingleton<GameManager>
{
    [Range(2, 9)] public int GridSize = 8;
    [Range(0, 5)] public float PreviewTime = 1f;
    public RectTransform Board;
    public Transform Root;

    public void Start()
    {
        CardManager.Instance.LoadCardSprites();
        CardManager.Instance.SetupCards(Root, Board, GridSize);
        CardManager.Instance.SetupSpritesOnCards();
        StartCoroutine(CardManager.Instance.BoardPreview(PreviewTime));
        CardManager.Instance.AddListeners();
    }
}
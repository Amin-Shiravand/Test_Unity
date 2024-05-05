using System;
using System.Collections;
using System.Collections.Generic;
using CardSystem;
using UnityEngine;
using Utils;

public class GameManager : MonoBehaviorSingleton<GameManager>
{
    [Range(0, 5)] public float PreviewTime = 1f;
    public RectTransform Board;
    public Transform Root;

    public void Start()
    {
    }

    public void InitCardSystem(int boardSize =2)
    {
        CardManager.Instance.LoadCardSprites();
        CardManager.Instance.SetupCards(Root, Board, boardSize);
        CardManager.Instance.SetupSpritesOnCards();
        StartCoroutine(CardManager.Instance.BoardPreview(PreviewTime));
        CardManager.Instance.AddListeners();
        CardManager.Instance.OnCardsAreOver += WinGame;
        CardManager.Instance.SetCountOfCards();
    }

    public void DeinitCardSystem()
    {
        CardManager.Instance.OnCardsAreOver -= WinGame;
        CardManager.Instance.RemoveListeners();
    }

    private void WinGame()
    {
        Debug.LogWarning("U win the Game");
    }
}
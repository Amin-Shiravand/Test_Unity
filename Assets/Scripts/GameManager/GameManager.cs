using System;
using System.Collections;
using System.Collections.Generic;
using CardSystem;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class GameManager : MonoBehaviorSingleton<GameManager>
{
    [Range(0, 5)] public float PreviewTime = 1f;
    public RectTransform Board;
    public Transform Root;
    public Button BackToMenu;

    private void Awake()
    {
        Debug.Assert(Board != null, "Board transform is null");             
        Debug.Assert(Root != null, "Board root panel is null");           
        Debug.Assert(BackToMenu != null, "Back to menu is null");      
        BackToMenu.onClick.AddListener(OnBackToMenuClick);
    }

    private void OnBackToMenuClick()
    {
        Menu.Instance.SetMenuState();
        DeinitCardSystem();
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
        CardManager.Instance.FlushTheCards();
    }

    private void WinGame()
    {
        OnBackToMenuClick();
    }
}
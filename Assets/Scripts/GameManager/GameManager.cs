using System;
using System.Collections;
using System.Collections.Generic;
using CardSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class GameManager : MonoBehaviorSingleton<GameManager>
{
    [Range(0, 5)] public float PreviewTime = 1f;
    public RectTransform Board;
    public Transform Root;
    public Button BackToMenu;
    private int score = 0;
    public TMP_Text ScoreText;

    private void Awake()
    {
        Debug.Assert(Board != null, "Board transform is null");
        Debug.Assert(Root != null, "Board root panel is null");
        Debug.Assert(BackToMenu != null, "Back to menu is null");
        Debug.Assert(ScoreText != null, "ScoreText is null");
        AudioManager.Instance.Init();
        BackToMenu.onClick.AddListener(OnBackToMenuClick);
    }

    private void OnBackToMenuClick()
    {
        AudioManager.Instance.PlayButton();
        FinishGame();
    }

    private void FinishGame()
    {
        Menu.Instance.SetMenuState();
        DeinitCardSystem();
    }

    public void InitCardSystem( int boardSize = 2 )
    {
        SetScore(0);
        CardManager.Instance.LoadCardSprites();
        CardManager.Instance.SetupCards(Root, Board, boardSize);
        CardManager.Instance.SetupSpritesOnCards();
        StartCoroutine(CardManager.Instance.BoardPreview(PreviewTime));
        CardManager.Instance.AddListeners();
        CardManager.Instance.onMatchPaired += OnMatchPaired;
        CardManager.Instance.OnCardsAreOver += WinGame;
        CardManager.Instance.SetCountOfCards();
    }
    
    public void DeinitCardSystem()
    {
        CardManager.Instance.OnCardsAreOver -= WinGame;
        CardManager.Instance.RemoveListeners();
        CardManager.Instance.FlushTheCards();
        CardManager.Instance.onMatchPaired -= OnMatchPaired;
    }

    private void WinGame()
    {
        FinishGame();
    }

    private void OnMatchPaired()
    {
        AddScore(1);
    }
    
    private void AddScore( int value )
    {
        score += value;
        ScoreText.text = $"Score:{score}";
    }

    private void SetScore( int value )
    {
        score = value;
        ScoreText.text = $"Score:{value}";
    }
}
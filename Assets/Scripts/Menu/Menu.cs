using System;
using System.Collections;
using System.Collections.Generic;
using SaveSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class Menu : MonoBehaviour
{
    public static Menu Instance = null;
    public Button StartGame;

    public Button Continue;

    public TMP_Text BoardSizeText;

    public Slider BoardSizeSlider;

    private void Awake()
    {
        Debug.Assert(StartGame != null, "start button is null");
        Debug.Assert(Continue != null, "continue button is null");
        Debug.Assert(BoardSizeText != null, "Board size text is null");
        Debug.Assert(BoardSizeSlider != null, "Board Size Slide is null");
        BoardSizeSlider.onValueChanged.AddListener(OnBoardSizeTextChanged);
        StartGame.onClick.AddListener(OnGameStarted);
        Continue.onClick.AddListener(OnContinueGame);
        BoardSizeSlider.minValue = 2;
        BoardSizeSlider.maxValue = 12;
        BoardSizeSlider.wholeNumbers = true;
        Instance = this;
    }

    public void OnEnable()
    {
        Continue.interactable = SaveManager.Instance.IsSaveFileExist();
    }

    public void SetMenuState()
    {
        this.transform.gameObject.SetActive(!this.transform.gameObject.activeSelf);
    }

    private void OnGameStarted()
    {
        AudioManager.Instance.PlayButton();
        SetMenuState();
        GameManager.Instance.InitCardSystem(( int )BoardSizeSlider.value);
    }

    private void OnBoardSizeTextChanged( float value )
    {
        BoardSizeText.text = $"{value}*{value}";
    }

    private void OnContinueGame()
    {
        SaveData saveData = SaveManager.Instance.LoadGameData();
        GameManager.Instance.LoadGame(saveData);
        SetMenuState();
    }
}
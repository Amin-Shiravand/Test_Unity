using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class Menu : MonoBehaviorSingleton<Menu>
{
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
        BoardSizeSlider.minValue = 2;
        BoardSizeSlider.maxValue = 12;
        BoardSizeSlider.wholeNumbers = true;
    }

    public void SetMenuState()
    {
        this.transform.gameObject.SetActive(!this.transform.gameObject.activeSelf);
    }

    private void OnGameStarted()
    {
        SetMenuState();
        GameManager.Instance.InitCardSystem(( int )BoardSizeSlider.value);
    }

    private void OnBoardSizeTextChanged( float value )
    {
        BoardSizeText.text = $"{value}*{value}";
    }

    // Update is called once per frame
    void Update()
    {
    }
}
using CardSystem;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using GameManagerTime = UnityEngine.Time;


public class GameManager : MonoBehaviorSingleton<GameManager>
{
    [Range(1, 120)] public float Time = 30f;
    [Range(0, 5)] public float BasePreviewTime = 1f;
    public RectTransform Board;
    public Transform Root;
    public Button BackToMenu;
    private int score = 0;
    private int boardSize = 0;
    private float timeKeeper;
    private float nextSecond;
    public TMP_Text ScoreText;
    public TMP_Text TimeText;
    private bool GameStarts = false;

    private void Awake()
    {
        Debug.Assert(Board != null, "Board transform is null");
        Debug.Assert(Root != null, "Board root panel is null");
        Debug.Assert(BackToMenu != null, "Back to menu is null");
        Debug.Assert(ScoreText != null, "ScoreText is null");
        SaveManager.Instance.Init();
        AudioManager.Instance.Init();
        BackToMenu.onClick.AddListener(OnBackToMenuClick);
    }

    private void OnBackToMenuClick()
    {
        AudioManager.Instance.PlayButton();
        FinishGame(false, true);
    }

    private void FinishGame( bool looseGame, bool saveData )
    {
        if( looseGame )
        {
            AudioManager.Instance.LooseGame();
        }

        SaveManager.Instance.DeleteSaveData();
        if( saveData )
        {
            CardModel[] cardsModel = new CardModel[CardManager.Instance.cardViews.Length];
            for( int i = 0; i < cardsModel.Length; ++i )
            {
                CardModel cardModel = CardManager.Instance.cardViews[i].CardModel;
                cardsModel[i] = cardModel;
            }

            SaveManager.Instance.SaveData(new SaveData()
            {
                Score = score,
                LeftCards = CardManager.Instance.LeftCards,
                Time = timeKeeper,
                BoardSize = boardSize,
                CardModels = cardsModel
            });
        }

        GameStarts = false;
        Menu.Instance.SetMenuState();
        DeinitCardSystem();
    }

    public void InitCardSystem( int boardSize = 2 )
    {
        this.boardSize = boardSize;
        float additionalPreviewTime = BasePreviewTime * ( boardSize * 0.1f );
        float additionalTime = Time * ( boardSize * 0.1f );
        SetScore(0);
        SetTime(Time + additionalTime);
        CardManager.Instance.LoadCardSprites();
        CardManager.Instance.SetupCards(Root, Board, boardSize);
        CardManager.Instance.SetupSpritesOnCards();
        StartCoroutine(CardManager.Instance.BoardPreview(BasePreviewTime + additionalPreviewTime));
        CardManager.Instance.AddListeners();
        CardManager.Instance.onMatchPaired += OnMatchPaired;
        CardManager.Instance.OnCardsAreOver += WinGame;
        CardManager.Instance.SetCountOfCards();
        GameStarts = true;
    }

    public void LoadGame( SaveData saveData )
    {
        this.boardSize = saveData.BoardSize;
        SetScore(saveData.Score);
        SetTime(saveData.Time);
        CardManager.Instance.LoadCardSprites();
        CardManager.Instance.SetupCards(Root, Board, boardSize);
        CardManager.Instance.SetBoardState(saveData.CardModels);
        StartCoroutine(CardManager.Instance.BoardPreview(BasePreviewTime));
        CardManager.Instance.AddListeners();
        CardManager.Instance.onMatchPaired += OnMatchPaired;
        CardManager.Instance.OnCardsAreOver += WinGame;
        CardManager.Instance.SetCountOfCards(saveData.LeftCards);
        GameStarts = true;
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
        FinishGame(false, false);
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

    private void SetTime( float value )
    {
        timeKeeper = value;
        UpdateTimeText();
        nextSecond = timeKeeper - 1;
    }

    private void UpdateTimeText()
    {
        TimeText.text = $"Time: {Mathf.CeilToInt(timeKeeper)}";
    }

    private void Update()
    {
        if( !GameStarts )
        {
            return;
        }

        timeKeeper -= GameManagerTime.deltaTime;
        if( nextSecond > timeKeeper )
        {
            nextSecond = timeKeeper - 1;
            UpdateTimeText();
        }

        if( timeKeeper < 0 )
        {
            FinishGame(true, false);
        }
    }
}
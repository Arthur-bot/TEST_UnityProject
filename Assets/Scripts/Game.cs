using System.Collections;
using DG.Tweening;
using UnityEngine;

public enum GameState
{
    PuckWaiting,
    PuckThrown,
    Victory,
    Defeat,
    Menu
}

public class Game : MonoBehaviour
{
    #region Constantes

    private const int BaseCoinVillage = 9;
    private const int BonusCoinPerRank = 3;
    private const int CoinPerRankUp = 150;

    private const int BaseStreak = 3;

    #endregion

    #region Fields

    [SerializeField] private PuckSpawner _puckSpawner;
    [SerializeField] private TrajectoryLine _trajectoryLine;

    private GameState _state;
    
    private int _coin;
    private int _currentRank;
    private int _currentStreak;

    private bool _rankUp;

    public Village _currentVillage;

    #endregion

    #region Properties

    public static Game Instance { get; private set; }
    
    private int StreakNeeded => BaseStreak + _currentRank;

    private int Coin
    {
        get => _coin;
        set
        {
            if (_coin == value) return;

            _coin = value;
            GameUI.Instance.CoinText.text = _coin.ToString();
        }
    }
    
    public GameState State
    {
        get => _state;
        set
        {
            if (_state == value) return;

            _state = value;

            StateChanged?.Invoke(this);
        }
    }

    public PuckSpawner PuckSpawner => _puckSpawner;

    public TrajectoryLine TrajectoryLine => _trajectoryLine;

    public int RemainingWin => StreakNeeded - _currentStreak;

    #endregion
    
    #region Event
    
    public delegate void EventHandler(Game sender);
    public event EventHandler StateChanged;

    #endregion

    #region Public Methods

    public void LoadRandomMap()
    {
        var randomVillage = GameResources.Instance.Villages[_currentRank].GetRandomVillage(_currentVillage);
        
        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            yield return GameUI.Instance.Fade(true)
                .WaitForCompletion();

            yield return null;

            Village.Load(randomVillage);

            _currentVillage = randomVillage;

            GameUI.Instance.Fade(false)
                .SetDelay(0.5f); 

            PuckSpawner.OnVillageLoad();
            State = GameState.PuckWaiting;
        }
    }

    public int GetCoinOnVillageVictory() => BaseCoinVillage + _currentRank * BonusCoinPerRank;

    public bool IsStreakNotFinished => _currentStreak < StreakNeeded;

    public void OnVictory()
    {
        Coin += GetCoinOnVillageVictory();
        _currentStreak += 1;
        State = GameState.Victory;
        
        if (!IsStreakNotFinished)
        {
            _currentStreak = 0;
            _currentRank += 1;
            _rankUp = true;
        }
    }

    public void TryDefeat()
    {
        if(_puckSpawner.NumberOfPuckLeft > 0) return;
        
        _currentStreak = 0;
        State = GameState.Defeat;
    }

    public void GoToMenu()
    {
        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            yield return GameUI.Instance.Fade(true)
                .WaitForCompletion();

            yield return null;

            Village.DestroyCurrent();
            _currentVillage = null;

            GameUI.Instance.MainMenu.Show(true);
            State = GameState.Menu;

            GameUI.Instance.Fade(false)
                .SetDelay(0.5f);

            if (_rankUp)
            {
                Coin += CoinPerRankUp;
            }
        }
    }

    #endregion

    #region Unity Event Function

    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    protected void Start()
    {
        Coin = 0;
        State = GameState.Menu;
    }

    #endregion
}
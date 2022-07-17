using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    #region Fields

    [SerializeField] private HUD _hud;
    [SerializeField] private UIPlayerInput _playerInput;
    [SerializeField] private Image _fade;
    [SerializeField] private UIEndScreenPopup _endScreen;
    [SerializeField] private UIMainMenu _mainMenu;
    [SerializeField] private TextMeshProUGUI _coinText;

    private Game _game;

    #endregion

    #region Properties
    
    public static GameUI Instance { get; private set; }

    public HUD HUD => _hud;

    public UIEndScreenPopup EndScreen => _endScreen;
    
    public UIMainMenu MainMenu => _mainMenu;
    
    public TextMeshProUGUI CoinText => _coinText;

    private bool IsHUDHidden
    {
        get => !_hud.gameObject.activeSelf;
        set
        {
            _hud.gameObject.SetActive(!value);
            
            _playerInput.enabled = !value;
        }
    }

    #endregion

    #region Public Methods

    public TweenerCore<Color, Color, ColorOptions> Fade(bool isIn, float duration = 0.3f)
        => _fade.DOFade(isIn ? 1f : 0f, duration)
            .SetEase(isIn ? Ease.OutSine : Ease.InQuart);

    #endregion

    #region Unity Event Functions

    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
        
        _game = Game.Instance;
        _game.StateChanged += GameOnStateChanged;
        
        Fade(false, 0.5f).From(1f);
    }

    protected void OnDestroy()
    {
        _game.StateChanged -= GameOnStateChanged;
    }

    #endregion
    
    #region Event Handler

    private void GameOnStateChanged(Game sender)
    {
        switch (sender.State)
        {
            case GameState.PuckWaiting:
                IsHUDHidden = false;
                break;
            case GameState.PuckThrown:
                IsHUDHidden = false;
                break;
            case GameState.Victory:
                _endScreen.ShowVictoryScreen(sender.IsStreakNotFinished);
                IsHUDHidden = true;
                break;
            case GameState.Defeat:
                _endScreen.ShowDefeatScreen();
                IsHUDHidden = true;
                break;
            case GameState.Menu:
                _mainMenu.Show(true);
                IsHUDHidden = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}

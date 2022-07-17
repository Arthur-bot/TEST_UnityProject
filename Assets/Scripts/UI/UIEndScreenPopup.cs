using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEndScreenPopup : MonoBehaviour
{
    #region Fields

    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _streakText;
    [SerializeField] private TextMeshProUGUI _rewardText;
    
    private readonly Color _rewardVictoryColor = Color.green;
    private readonly Color _rewardDefeatColor = Color.red;

    #endregion

    #region Public Methods

    public void ShowVictoryScreen(bool nextVillage)
    {
        var game = Game.Instance;
        
        _headerText.text = "Victory";
        _streakText.text = $"Still {game.RemainingWin} games in a row to rank up";
        _rewardText.text = $"+{game.GetCoinOnVillageVictory()}";
        _rewardText.color = _rewardVictoryColor;
        
        _nextButton.gameObject.SetActive(nextVillage);
        
        gameObject.SetActive(true);
    }

    public void ShowDefeatScreen()
    {
        _headerText.text = "Defeat";
        _streakText.text = "Reseat streak";
        _rewardText.text = "+0";
        _rewardText.color = _rewardDefeatColor;
        
        _nextButton.gameObject.SetActive(false);
        
        gameObject.SetActive(true);
    }

    #endregion

    #region Unity Event Functions

    protected void Awake()
    {
        _menuButton.onClick.AddListener(() =>
        {
            Game.Instance.GoToMenu();
            gameObject.SetActive(false);
        });
        
        _nextButton.onClick.AddListener(() =>
        {
            Game.Instance.LoadRandomMap();
            gameObject.SetActive(false);
        });
    }

    #endregion
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private TextMeshProUGUI _streakText;
    [SerializeField] private Button _playButton;

    #endregion
    
    #region Public Methods

    public void Show(bool show)
    {
        if (show) Refresh();
        
        gameObject.SetActive(show);
    }

    #endregion

    #region Unity Event Functions

    protected void Awake()
    {
        _playButton.onClick.AddListener(() =>
        {
            Show(false);
            Game.Instance.LoadRandomMap();
        });
    }

    #endregion

    #region Private Methods

    private void Refresh()
    {
        _streakText.text = $"Win {Game.Instance.RemainingWin} games in a row to rank up";
    }

    #endregion
}

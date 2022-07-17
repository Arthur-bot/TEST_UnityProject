using UnityEngine;

public class UIHealthBar : MonoBehaviour
{
    #region Fields

    [SerializeField] private RectTransform _healthFill;
    [SerializeField] private CanvasGroup _canvasGroup;

    private Obstacle _obstacle;
    
    #endregion

    #region Public Methods

    public void InitializeHealthBar(Obstacle obstacle)
    {
        _obstacle = obstacle;
        Refresh();
    }

    public void Show(bool show)
    {
        if (show) Refresh();
        
        _canvasGroup.alpha = show ? 1f : 0f;
    }

    #endregion

    #region Unity Event Functions

    protected void Awake()
    {
        _healthFill.anchorMax = X(_healthFill.anchorMax, 1f);
    }

    #endregion

    #region Private Methods
    
    private void Refresh()
    {
        var hpRatio = (float)_obstacle.CurrentHp / _obstacle.MaxMaxHp;
        
        if (Mathf.Abs(_healthFill.anchorMax.x - hpRatio) < 0.01f) return;
        
        _healthFill.anchorMax = X(_healthFill.anchorMax, hpRatio);
    }

    private static Vector2 X(Vector2 v, float x)
    {
        v.x = x;
        return v;
    }

    #endregion
}

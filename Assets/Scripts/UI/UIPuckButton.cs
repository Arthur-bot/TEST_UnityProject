using UnityEngine;
using UnityEngine.UI;

public class UIPuckButton : MonoBehaviour
{
    #region Fields

    [SerializeField] private Button _button;
    [SerializeField] private Image _icon;

    #endregion

    #region Public Methods

    public void Initialize(PuckSpawner spawner, Puck puck)
    {
        _icon.sprite = puck.Icon;
        _button.onClick.AddListener(() => spawner.SpawnPuck(puck));
    }

    #endregion
}

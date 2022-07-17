using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    #region Fields

    [SerializeField] private int _maxHp;
    [SerializeField] private ParticleSystem _fireFX;
    [SerializeField] private GameObject _dummyPrefab;
    [SerializeField] private bool _invicible;

    private readonly List<Renderer> _renderers = new List<Renderer>();
    private int _currentHP;
    private bool _killed;

    private Tweener _punchTweener;
    
    private UIHealthBar _healthBar;

    #endregion

    #region Properties

    public int CurrentHp
    {
        get => _currentHP;
        private set
        {
            if (_currentHP == value) return;

            _currentHP = value;

            _healthBar.Show(true);
        }
    }

    public int MaxMaxHp => _maxHp;

    public bool IsAlive => CurrentHp > 0;

    public GameObject DummyPrefabObstacle => _dummyPrefab;
    
    public GameObject LinkedDummy { get; set; }

    #endregion

    #region Public Methods

    public void TakeDamage(int damage)
    {
        if (_invicible) return;
        
        CurrentHp -= damage;
        
        _punchTweener = transform.DOPunchScale(Vector3.one * 0.2f, 0.2f);
            
        if (_fireFX != null && !_fireFX.gameObject.activeSelf)
        {
            _fireFX.gameObject.SetActive(true);
        }

        if (CurrentHp > 0 || _killed) return;
        
        _killed = true;
        OnKill();
    }

    #endregion

    #region Protected Methods

    protected virtual void OnKill()
    {
        Game.Instance.TrajectoryLine.DestroyObstacle(LinkedDummy);
        Destroy(gameObject);
    }

    #endregion

    #region Unity Event Functions

    protected void Awake()
    {
        GetComponentsInChildren(_renderers);
        
        _healthBar = GetComponentInChildren<UIHealthBar>(includeInactive: true);
        _healthBar.InitializeHealthBar(this);
        _healthBar.Show(false);

        CurrentHp = _maxHp;
        
        Game.Instance.StateChanged += GameOnStateChanged;
    }

    protected void OnDestroy()
    {
        Game.Instance.StateChanged -= GameOnStateChanged;

        DOTween.Kill(_punchTweener);
    }

    #endregion

    #region Event Handler

    private void GameOnStateChanged(Game sender)
    {
        if (sender.State == GameState.PuckWaiting && !_invicible)
        {
            _healthBar.Show(true);
        }
        else
        {
            _healthBar.Show(false);
        }
    }

    #endregion
}

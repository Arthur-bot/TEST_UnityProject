using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class PuckSpawner : MonoBehaviour
{
    #region Fields

    [SerializeField] private List<Puck> _pucks;
    [SerializeField] private LayerMask _layerMask;

    private Ray _ray;
    private RaycastHit _hitData;

    private TrajectoryLine _trajectoryLine;
    private Camera _mainCamera;

    private Puck _currentPuck;
    private int _numberOfPuckLeft;
    private Vector3 _direction;
    private bool _thrown;

    #endregion

    #region Properties

    public int NumberOfPuckLeft
    {
        get => _numberOfPuckLeft;
        private set
        {
            if (_numberOfPuckLeft == value) return;

            _numberOfPuckLeft = value;
            GameUI.Instance.HUD.UpdatePuckText(value);
        }
    }

    #endregion

    #region Public Methods

    public void Throw(Vector2 screenPosition)
    {
        if (_thrown) return;

        _ray = _mainCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(_ray, out _hitData, 1000, _layerMask))
        {
            _direction = -_hitData.point.normalized;
        }

        _thrown = true;
        NumberOfPuckLeft -= 1;
        _trajectoryLine.ShowDots(false);
        _currentPuck.Init(_direction);

        Game.Instance.State = GameState.PuckThrown;
    }

    public void Predict(Vector2 screenPosition)
    {
        if (_thrown) return;

        _ray = _mainCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(_ray, out _hitData, 1000, _layerMask))
        {
            _direction = -_hitData.point.normalized;
        }

        _trajectoryLine.SimulateTrajectory(_currentPuck, Vector3.zero, _direction);
    }

    public void SpawnPuck(Puck puck)
    {
        if (_currentPuck != null)
        {
            Destroy(_currentPuck.gameObject);
        }

        _thrown = false;
        _currentPuck = Instantiate(puck, Vector3.zero, quaternion.identity);
        _currentPuck.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f);

        Game.Instance.State = GameState.PuckWaiting;
    }

    public void OnVillageLoad()
    {
        NumberOfPuckLeft = Village.Current.InitialNumberOfPuck;

        SpawnPuck(_pucks[0]);
    }

    #endregion

    #region Unity event Function

    protected void Awake()
    {
        _mainCamera = Camera.main;
        _trajectoryLine = Game.Instance.TrajectoryLine;

        GameUI.Instance.HUD.InitializePuckButtons(this, _pucks);
    }

    #endregion
}
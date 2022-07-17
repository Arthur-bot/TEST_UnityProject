using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrajectoryLine : MonoBehaviour
{
    #region Fields

    [SerializeField] private List<SpriteRenderer> _dots = new List<SpriteRenderer>();

    private Scene _currentScene;
    private Scene _simulationScene;

    private PhysicsScene _currentPhysicsScene;
    private PhysicsScene _simulationPhysicsScene;

    private Puck _dummyPuck;
    private readonly List<GameObject> _dummyObstacles = new List<GameObject>();

    #endregion

    #region properties

    private int MaxPhysicsFrameIterations => _dots.Count;

    #endregion

    #region Public Methods

    public void InitSimulation(List<Obstacle> obstacles)
    {
        foreach (var obj in obstacles)
        {
            var dummyObj = Instantiate(obj.DummyPrefabObstacle, obj.transform.position, obj.transform.rotation);
            dummyObj.transform.localScale = obj.transform.localScale;
            obj.LinkedDummy = dummyObj;
            
            SceneManager.MoveGameObjectToScene(dummyObj, _simulationScene);
            _dummyObstacles.Add(dummyObj);
        }
    }

    public void SimulateTrajectory(Puck puck, Vector3 position, Vector3 velocity)
    {
        if (!_currentPhysicsScene.IsValid() || !_simulationScene.IsValid()) return;
        
        if (_dummyPuck == null)
        {
            _dummyPuck = Instantiate(puck);
            SceneManager.MoveGameObjectToScene(_dummyPuck.gameObject, _simulationScene);

            _dummyPuck.Renderer.enabled = false;
        }

        _dummyPuck.transform.position = position;
        _dummyPuck.Init(velocity);
        ShowDots(true);

        for (var i = 0; i < MaxPhysicsFrameIterations; i++)
        {
            _simulationPhysicsScene.Simulate(Time.fixedDeltaTime);

            var nextPosition = _dummyPuck.transform.position;
            nextPosition.y = 0.1f;
                
            _dots[i].transform.position = nextPosition;
        }
    }
    
    public void DestroyAllObstacles()
    {
        for (var i = _dummyObstacles.Count - 1; i >= 0; i--)
        {
            var dummy = _dummyObstacles[i];
            
            _dummyObstacles.Remove(dummy);
            Destroy(dummy);
        }
    }

    public void DestroyObstacle(GameObject dummy)
    {
        if (!_dummyObstacles.Contains(dummy)) return;
        
        _dummyObstacles.Remove(dummy);
        Destroy(dummy);
    }

    public void ShowDots(bool show)
    {
        foreach (var dot in _dots)
        {
            dot.gameObject.SetActive(show);
        }
    }
    
    #endregion
    
    #region Protected Methods

    protected void Awake()
    {
        Physics.autoSimulation = false;

        _currentScene = SceneManager.GetActiveScene();
        _currentPhysicsScene = _currentScene.GetPhysicsScene();

        var parameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        _simulationScene = SceneManager.CreateScene("Prediction", parameters);
        _simulationPhysicsScene = _simulationScene.GetPhysicsScene();
        
        ShowDots(false);
    }

    protected void FixedUpdate()
    {
        if (_currentPhysicsScene.IsValid())
        {
            _currentPhysicsScene.Simulate(Time.fixedDeltaTime);
        }
    }

    protected void OnDestroy()
    {
        DestroyAllObstacles();
    }

    #endregion
}
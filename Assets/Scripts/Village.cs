using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    #region Fields

    [SerializeField] private int _initialNumberOfPuck;

    private readonly List<Obstacle> _obstacles = new List<Obstacle>();
    private int _index;

    #endregion
    
    #region Properties

    public static Village Current { get; private set; }

    public int InitialNumberOfPuck => _initialNumberOfPuck;
    
    public int Index => _index < 0
        ? _index = int.Parse(name) - 1
        : _index;

    #endregion

    #region Public Methods

    public static void Load(Village village)
    {
        DestroyCurrent();

        var map = Instantiate(village);
        map.name = village.name;
    }

    public static void DestroyCurrent()
    {
        if (Current != null)
        {
            Game.Instance.TrajectoryLine.DestroyAllObstacles();
            
            DestroyImmediate(Current.gameObject);
            Current = null;
        }
    }

    #endregion

    #region Unity Event Functions

    protected void Awake()
    {
        if (Current != null)
        {
            Destroy(gameObject);
            return;
        }

        Current = this;

        GetComponentsInChildren(includeInactive: true, _obstacles);
        
        Game.Instance.TrajectoryLine.InitSimulation(_obstacles);
    }

    #endregion
}

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameResources : MonoBehaviour
{
    #region Struct

    [Serializable]
    public struct VillagesAtRank
    {
        public List<Village> _villages;

        public Village GetRandomVillage(Village currentVillage = null)
        {
            var villageCopy = new List<Village>(_villages);

            if (currentVillage != null)
            {
                villageCopy.Remove(currentVillage);
            }
            
            return villageCopy[Random.Range(0, villageCopy.Count - 1)];
        }
    }

    #endregion
    
    #region Fields

    [SerializeField] private List<VillagesAtRank> _villages;

    #endregion
    
    #region Properties

    public static GameResources Instance { get; private set; }

    public List<VillagesAtRank> Villages => _villages;

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
    }

    #endregion
}

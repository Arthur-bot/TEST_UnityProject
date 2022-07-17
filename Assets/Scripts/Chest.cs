using System.Collections;
using UnityEngine;

public class Chest : Obstacle
{
    #region Protected Methods

    protected override void OnKill()
    {
        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            // Animate chest
            // Camera movement
            // slow down game
            yield return new WaitForSeconds(0.5f);
            
            Game.Instance.OnVictory();
        }
    }

    #endregion
}

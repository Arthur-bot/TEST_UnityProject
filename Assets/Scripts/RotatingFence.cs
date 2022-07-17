using DG.Tweening;
using UnityEngine;

public class RotatingFence : MonoBehaviour
{
    #region Fields

    [SerializeField] private Vector3 _targetRotation;
    [SerializeField] private float _rotationDuration;

    #endregion
    
    #region Unity Event Functions

    protected void OnEnable()
    {
        var seq = DOTween.Sequence();
        seq.SetDelay(0.5f)
            .Append(transform.DORotate(_targetRotation, _rotationDuration))
            .SetEase(Ease.InOutFlash, 2f, 0f)
            .SetLoops(-1);
    }

    #endregion
}

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayerInput : MonoBehaviour
{
    #region Fields

    [SerializeField] private EventTrigger _trigger;
    
    private PuckSpawner _puckSpawner;

    private Vector2 _screenPosition;

    #endregion

    #region Unity Event Functions

    protected void Awake()
    {
        var triggerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        triggerDown.callback.AddListener(OnPointerDown);
        _trigger.triggers.Add(triggerDown);
        
        var triggerDrag = new EventTrigger.Entry { eventID = EventTriggerType.Drag };
        triggerDrag.callback.AddListener(OnDrag);
        _trigger.triggers.Add(triggerDrag);
        
        var triggerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        triggerUp.callback.AddListener(OnPointerUp);
        _trigger.triggers.Add(triggerUp);

        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            yield return null;
            
            _puckSpawner = Game.Instance.PuckSpawner;
        }
    }

    #endregion

    #region Private Methods

    private void OnPointerDown(BaseEventData data)
    {
        var pointerEventData = (PointerEventData)data;
        _screenPosition = pointerEventData.position;
        _puckSpawner.Predict(_screenPosition);
    }
    
    private void OnDrag(BaseEventData data)
    {
        var pointerEventData = (PointerEventData)data;
        _screenPosition = pointerEventData.position;
        _puckSpawner.Predict(_screenPosition);
    }

    private void OnPointerUp(BaseEventData data)
    {
        _puckSpawner.Throw(_screenPosition);
    }

    #endregion
}
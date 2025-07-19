using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static DrawingManager;

public class GameEvent : MonoBehaviour
{
    [SerializeField] protected DrawingCompleteTrigger eventTrigger;
    [SerializeField] private float executionDelay = 0;
    protected bool destroyParent = false;

    private void OnEnable()
    {
        EventSystem.OnCameraLookChange += OnLookChanging;
    }

    private void OnDisable()
    {
        EventSystem.OnCameraLookChange -= OnLookChanging;
    }

    protected virtual void Awake()
    {
        this.enabled = false; 
    }

    public void SetDelayTimer(float delay)
    {
        executionDelay = delay;
    }

    public void SetIndipendentEventNotDestroyParent()
    {
        destroyParent = false;
    }

    public void SetEventTrigger(DrawingCompleteTrigger eventTrigger)
    {
        this.eventTrigger = eventTrigger;
    }

    public bool GetDestroyParent()
    {
        return destroyParent;
    }

    protected virtual void OnLookChanging(bool lookingUp)
    {
        if ((eventTrigger == DrawingCompleteTrigger.LOOKING_UP && lookingUp) || (eventTrigger == DrawingCompleteTrigger.LOOKING_DOWN && !lookingUp))
        {
            eventTrigger = DrawingCompleteTrigger.NONE;
            Execute();
        }

    }

    public virtual void Begin() 
    {
        switch (eventTrigger)
        {
            case DrawingCompleteTrigger.NONE:
                Execute();
                break;
            case DrawingCompleteTrigger.EXECUTE_AFTER_SET_TIME:
                StartCoroutine(Co_DelayExecution());
                break;
            default:
                break;
        }
    }

    public virtual void Execute() { }

    public static event UnityAction<GameEvent> OnGameEventCompleted;
    public virtual void GameEventCompleted(GameEvent eventCompleted) { OnGameEventCompleted?.Invoke(eventCompleted); }

    public virtual void Cleanup(bool destroyParentComponent)
    {
        //Destroys the parent component to this game event if it is the last event attached to it
        if (destroyParentComponent && gameObject.GetComponentsInChildren<GameEvent>().Length == 1)
        {
            Destroy(gameObject);
        }

        Destroy(this);
    }

    private IEnumerator Co_DelayExecution()
    {
        yield return new WaitForSeconds(executionDelay);
        Execute();
    }
}

using UnityEngine;
using static DrawingManager;

public class GameEvent : MonoBehaviour
{
    private DrawingCompleteTrigger currTrigger;

    private void OnEnable()
    {
        EventSystem.OnCameraLookChange += OnLookChanging;
    }

    private void OnDisable()
    {
        EventSystem.OnCameraLookChange -= OnLookChanging;
    }

    private void OnLookChanging(bool lookingUp)
    {
        if ((currTrigger == DrawingCompleteTrigger.LOOKING_UP && lookingUp) || (currTrigger == DrawingCompleteTrigger.LOOKING_DOWN && !lookingUp))
        {
            currTrigger = DrawingCompleteTrigger.NONE;
            Execute();
        }

    }

    public virtual void Begin(DrawingCompleteTrigger trigger) 
    {
        switch (trigger)
        {
            case DrawingCompleteTrigger.NONE:
                Execute();
                break;
            default:
                currTrigger = trigger;
                break;
        }
    }

    public virtual void Execute() { }

    public virtual void Completed() { }
}

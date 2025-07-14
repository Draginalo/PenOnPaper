using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    private GameEventChain currGameEvents;

    public static GameEventManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void OnEnable()
    {
        GameEvent.OnGameEventCompleted += HandleGameEventCompleted;
    }

    private void OnDisable()
    {
        GameEvent.OnGameEventCompleted -= HandleGameEventCompleted;
    }

    public void LoadAndExecuteEventChain(GameEventChain gameEventsToExecute)
    {
        //The last check is for if the same reference is being loaded (because if you load the reference that already exists it will just remove all events instead of preserving and starting the chain)
        if (currGameEvents != null && currGameEvents.GetEventChain().Count > 0 && gameEventsToExecute != currGameEvents)
        {
            currGameEvents.CleanupChain();
            //Destroy(currGameEvents);
        }

        currGameEvents = gameEventsToExecute;
        ExecuteNextEventChain();
    }

    public void ExecuteNextEventChain()
    {
        ExcecuteNextEvent();
    }

    public void ExcecuteNextEvent()
    {
        if (currGameEvents.GetEventChain().Count > 0)
        {
            currGameEvents.GetEventChain()[0].enabled = true;
            currGameEvents.GetEventChain()[0].Begin();

            return;
        }

        currGameEvents.CleanupChain();
        //Destroy(currGameEvents);
    }

    private void HandleGameEventCompleted(GameEvent eventCompleted)
    {
        if (currGameEvents != null)
        {
            bool isCurentEvent = currGameEvents.GetCurrEvent() == eventCompleted;

            //This is to handle event chains only and not indipendent events
            if (isCurentEvent)
            {
                currGameEvents.RemoveCurrentEvent();
                ExcecuteNextEvent();
                return;
            }
        }

        eventCompleted.Cleanup(eventCompleted.GetDestroyParent());
    }
}

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
        if (currGameEvents != null && currGameEvents.GetEventChain().Count > 0)
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
        //This is to handle event chains only and not indipendent events
        if (currGameEvents != null && currGameEvents.GetCurrEvent() == eventCompleted)
        {
            currGameEvents.RemoveCurrentEvent();
            ExcecuteNextEvent();
        }
    }
}

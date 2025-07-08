using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameEventChain
{
    [SerializeField] private List<GameEvent> eventChain;
    [SerializeField] private GameObject componentParent;
    public bool destroyParentComponent = true;

    public void SetEvents(List<GameEvent> events)
    {
        eventChain = events;
    }

    public void AddEventToEnd(GameEvent gameEvent)
    {
        if (eventChain == null)
        {
            eventChain = new();
        }

        eventChain.Add(gameEvent);
    }

    public List<GameEvent> GetEventChain()
    {
        return eventChain;
    }

    public GameObject ComponentParent
    { 
        get { return componentParent; } 
    }

    public GameEvent GetCurrEvent()
    {
        if (eventChain.Count > 0)
        {
            return eventChain[0];
        }

        return null;
    }

    public void RemoveCurrentEvent()
    {
        if (eventChain.Count > 0)
        {
            eventChain[0].Cleanup(destroyParentComponent);
            eventChain.RemoveAt(0);
        }
    }

    public void CleanupChain()
    {
        if (eventChain != null)
        {
            int originalCount = eventChain.Count;
            for (int i = 0; i < originalCount; i++)
            {
                RemoveCurrentEvent();
            }

            //Destroy(this);
        }
    }
}

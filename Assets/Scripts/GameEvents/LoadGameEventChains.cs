using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameEventChains : GameEvent
{
    [SerializeField] List<GameEventChain> gameEventChains;
    [SerializeField] GameObject gameEventChainsParent;

    private void Start()
    {
        GameEventChain t = new GameEventChain();
        t.SetEvents(new List<GameEvent> { this });

        GameEventManager.instance.LoadAndExecuteEventChain(t);
    }

    public override void Execute()
    {
        base.Execute();
        EventSystem.LoadEventChains(gameEventChains, gameEventChainsParent);
        GameEventCompleted(this);
    }
}

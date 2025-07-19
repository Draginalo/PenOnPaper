using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameEventChains : GameEvent
{
    [SerializeField] private List<GameEventChain> gameEventChains;
    [SerializeField] private GameObject gameEventChainsParent;
    [SerializeField] private bool excecuteOnStart = false;

    protected override void Awake()
    {
        if (excecuteOnStart)
        {
            this.enabled = true;
            return;
        }

        base.Awake();
    }

    private void Start()
    {
        if (excecuteOnStart)
        {
            LoadSelf();
        }
    }

    public void LoadSelf()
    {
        GameEventChain t = new GameEventChain();
        t.SetEvents(new List<GameEvent> { this });

        GameEventManager.instance.LoadAndExecuteEventChain(t);
    }

    public override void Execute()
    {
        base.Execute();
        gameEventChainsParent.SetActive(true);
        EventSystem.LoadEventChains(gameEventChains, gameEventChainsParent);
        GameEventCompleted(this);
    }
}

using UnityEngine;

public class StartCreditsEvent : GameEvent
{
    public override void Execute()
    {
        base.Execute();

        EventSystem.StartCredits();
        
        GameEventCompleted(this);
    }
}

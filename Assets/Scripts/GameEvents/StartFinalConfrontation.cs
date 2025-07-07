using UnityEngine;

public class StartFinalConfrontation : GameEvent
{
    public override void Execute()
    {
        base.Execute();
        EventSystem.StartFinalConfrontation();
        GameEventCompleted(this);
    }
}

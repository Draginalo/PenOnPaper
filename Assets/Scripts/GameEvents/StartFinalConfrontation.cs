using UnityEngine;

public class StartFinalConfrontation : GameEvent
{
    public bool intro = false;

    public override void Execute()
    {
        base.Execute();
        EventSystem.StartFinalConfrontation(intro);
        GameEventCompleted(this);
    }
}

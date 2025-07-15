using UnityEngine;

public class ChangeSketchEvent : GameEvent
{
    public override void Execute()
    {
        base.Execute();

        EventSystem.ChangeSketch();
        
        GameEventCompleted(this);
    }
}

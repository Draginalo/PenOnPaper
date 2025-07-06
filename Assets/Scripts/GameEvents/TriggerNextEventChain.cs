using UnityEngine;

public class TriggerNextEventChain : GameEvent
{
   public override void Execute()
    {
        base.Execute();

        EventSystem.TriggerNextEventChain();

        GameEventCompleted(this);
    }
}

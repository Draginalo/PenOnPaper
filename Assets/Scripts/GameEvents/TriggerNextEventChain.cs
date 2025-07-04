using UnityEngine;

public class TriggerNextEventChain : GameEvent
{
   public override void Execute()
    {
        base.Execute();

        EventSystem.TriggerNextEventChain();

        //Make this timed exactly when the environment has swapped
        GameEventCompleted(this);
    }
}

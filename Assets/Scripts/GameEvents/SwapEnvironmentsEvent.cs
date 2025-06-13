using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DrawingManager;

public class SwapEnvironmentsEvent : GameEvent
{
    public override void Execute()
    {
        base.Execute();
        EventSystem.SwapEnvironment(EnvironmentSwitchManager.Environments.None);
    }

    public override void Completed()
    {
        base.Completed();
        //EventSystem.TriggerNextSketch(DrawingCompleteTrigger.LOOKING_DOWN);
    }
}

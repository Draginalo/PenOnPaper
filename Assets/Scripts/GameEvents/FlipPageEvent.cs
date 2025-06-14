using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DrawingManager;

public class FlipPageEvent : GameEvent
{
    public override void Execute()
    {
        base.Execute();
        EventSystem.FlipNotepadPage(this);
    }

    public override void Completed()
    {
        base.Completed();
        EventSystem.TriggerNextSketch(DrawingCompleteTrigger.LOOKING_UP);
    }
}

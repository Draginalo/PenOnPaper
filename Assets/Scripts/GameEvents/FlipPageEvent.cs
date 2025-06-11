using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DrawingManager;

public class FlipPageEvent : GameEvent
{
    public override void Execute()
    {
        EventSystem.FlipNotepadPage(this);
    }

    public override void Completed()
    {
        EventSystem.TriggerNextSketch(DrawingCompleteTrigger.NONE);
    }
}

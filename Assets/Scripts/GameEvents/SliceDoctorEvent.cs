using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceDoctorEvent : GameEvent
{
    public override void Execute()
    {
        base.Execute();
        EventSystem.SliceDoctor();
        GameEventCompleted(this);

        EventSystem.ActivateSketchChoosing();
    }
}

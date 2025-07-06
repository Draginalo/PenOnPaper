using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCameraShake : GameEvent
{
    public override void Execute()
    {
        base.Execute();
        CameraHandler.instance.InitCameraShake(0, 0);

        GameEventCompleted(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairLightDrawingHandler : DrawHandler
{
    protected override void HandleCompletion()
    {
        base.HandleCompletion();
        EventSystem.RepairLight();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDrawingScript : DrawHandler
{
    protected override void HandleCompletion()
    {
        base.HandleCompletion();
        EventSystem.FinishFinalConfrontation();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardUpDrawingHandler : DrawHandler
{
    protected override void HandleCompletion()
    {
        base.HandleCompletion();
        EventSystem.StopOpening();
    }
}

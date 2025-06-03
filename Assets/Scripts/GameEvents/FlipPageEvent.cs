using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipPageEvent : GameEvent
{
    public override void Execute()
    {
        EventSystem.FlipNotepadPage();
    }
}

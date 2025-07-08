using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleHospitalVoidEnv : GameEvent
{
    public override void Execute()
    {
        base.Execute();

        EventSystem.ToggleActivateHospitalVoidEnv();

        GameEventCompleted(this);
    }
}

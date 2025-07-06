using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealHospitalVoidEnv : GameEvent
{
    public override void Execute()
    {
        base.Execute();

        EventSystem.ActivateHospitalVoidEnv();

        GameEventCompleted(this);
    }
}

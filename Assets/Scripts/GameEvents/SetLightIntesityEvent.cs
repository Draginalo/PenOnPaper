using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightIntesityEvent : GameEvent
{
    public AnimationCurve curve;
    public float maxIntensity;
    public float newRange;
    public float curveEvaluationSpeed;
    public float markedAsCompletedTimeOnCurve = 0.0f;

    public override void Execute()
    {
        base.Execute();
        
        EventSystem.SetLightIntensity(this);

        //GameEventCompleted(this);
    }
}

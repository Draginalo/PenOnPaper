using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightIntesityEvent : GameEvent
{
    public AnimationCurve curve;
    public float maxIntensity;
    public float newRange;
    public float curveEvaluationSpeed;

    public override void Execute()
    {
        base.Execute();
        
        EventSystem.SetLightIntensity(curve, maxIntensity, newRange, curveEvaluationSpeed);

        GameEventCompleted(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightIntesityEvent : GameEvent
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float newRange;
    [SerializeField] private float curveEvaluationSpeed;

    public override void Execute()
    {
        base.Execute();
        
        EventSystem.SetLightIntensity(curve, maxIntensity, newRange, curveEvaluationSpeed);

        GameEventCompleted(this);
    }
}

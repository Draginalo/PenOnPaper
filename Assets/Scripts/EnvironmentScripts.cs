using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScripts : MonoBehaviour
{
    [SerializeField] private Light light;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float curveEvaluationSpeed;
    [SerializeField] private AnimationCurve lightIntensityCurve;
    private float currTime = 0;

    private void FixedUpdate()
    {
        light.intensity = maxIntensity * lightIntensityCurve.Evaluate(currTime * curveEvaluationSpeed);
        currTime += Time.deltaTime;

        if (currTime * curveEvaluationSpeed > 1)
        {
            currTime = 0;
        }
    }
}

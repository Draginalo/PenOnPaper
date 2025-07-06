using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireScript : MonoBehaviour
{
    [SerializeField] private Light campfireLight;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float curveEvaluationSpeed;
    [SerializeField] private AnimationCurve lightIntensityCurve;
    [SerializeField] private bool preserveRange = false;
    [SerializeField] private bool preserveIntensity = false;
    private float currTime = 0;
    private bool runningOtherLightIntensity = false;

    private float otherMaxIntensity;
    private float originalIntensity;
    private float prevRange;
    private float newRange;
    private float otherCurveEvaluationSpeed;
    private AnimationCurve otherLightIntensityCurve;

    private float transitionTime = 0.2f;

    private void OnEnable()
    {
        EventSystem.OnSetLightIntensity += HandleOtherLightIntensity;
    }

    private void OnDisable()
    {
        EventSystem.OnSetLightIntensity -= HandleOtherLightIntensity;
    }

    private void FixedUpdate()
    {
        if (!runningOtherLightIntensity)
        {
            campfireLight.intensity = maxIntensity * lightIntensityCurve.Evaluate(currTime * curveEvaluationSpeed);
            currTime += Time.deltaTime;

            if (currTime * curveEvaluationSpeed > 1)
            {
                currTime = 0;
            }
        }
    }

    private void HandleOtherLightIntensity(AnimationCurve newCurve, float newMaxIntensity, float newRange, float newEvaluationSpeed)
    {
        runningOtherLightIntensity = true;
        otherCurveEvaluationSpeed = newEvaluationSpeed;
        otherLightIntensityCurve = newCurve;

        if (!preserveIntensity)
        {
            otherMaxIntensity = newMaxIntensity;
        }

        this.newRange = newRange;
        prevRange = campfireLight.range;
        originalIntensity = campfireLight.intensity;
        currTime = 0;
        StartCoroutine(Co_TransitionToOtherLightIntensity());
    }

    private bool RunLightIntensity()
    {
        campfireLight.intensity = otherMaxIntensity * otherLightIntensityCurve.Evaluate(currTime * otherCurveEvaluationSpeed);
        if (!preserveRange)
        {
            campfireLight.range = Mathf.Lerp(prevRange, newRange, currTime * otherCurveEvaluationSpeed);
        }
        currTime += Time.deltaTime;

        return currTime * otherCurveEvaluationSpeed >= 1;
    }

    private bool RunToOtherLightIntensity()
    {
        campfireLight.intensity = Mathf.Lerp(originalIntensity, otherMaxIntensity * otherLightIntensityCurve.Evaluate(0), currTime / transitionTime);
        currTime += Time.deltaTime;

        return currTime >= transitionTime;
    }

    private bool RunFromOtherLightIntensity()
    {
        campfireLight.intensity = Mathf.Lerp(otherMaxIntensity * otherLightIntensityCurve.Evaluate(1), maxIntensity * lightIntensityCurve.Evaluate(0), currTime / transitionTime);
        currTime += Time.deltaTime;

        return currTime >= transitionTime;
    }

    private IEnumerator Co_ExecuteOtherLightIntensity()
    {
        yield return new WaitUntil(RunLightIntensity);
        currTime = 0;

        StartCoroutine(Co_TransitionToFromLightIntensity());
    }

    private IEnumerator Co_TransitionToOtherLightIntensity()
    {
        yield return new WaitUntil(RunToOtherLightIntensity);
        currTime = 0;

        StartCoroutine(Co_ExecuteOtherLightIntensity());
    }

    private IEnumerator Co_TransitionToFromLightIntensity()
    {
        yield return new WaitUntil(RunFromOtherLightIntensity);
        currTime = 0;
        runningOtherLightIntensity = false;
    }
}

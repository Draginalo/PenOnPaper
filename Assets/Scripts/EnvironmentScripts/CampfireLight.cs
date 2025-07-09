using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireScript : MonoBehaviour
{
    [SerializeField] private Light campfireLight;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float curveEvaluationSpeed;
    [SerializeField] private AnimationCurve lightIntensityCurve;
    [SerializeField] private bool preserveMaxRange = false;
    [SerializeField] private bool preserveIntensity = false;
    private float currTime = 0;
    private bool runningOtherLightIntensity = false;

    private float otherMaxIntensity;
    private float originalIntensity;
    private float prevRange;
    private float newRange;
    private float originalRange;
    private float otherCurveEvaluationSpeed;
    private AnimationCurve otherLightIntensityCurve;

    private float transitionTime = 0.2f;
    private bool destroyAfterSwitch = false;
    private SetLightIntesityEvent currEvent;

    private void OnEnable()
    {
        EventSystem.OnSetLightIntensity += HandleOtherLightIntensity;
        EventSystem.OnFinishFinalConfrontation += DestroyOnConfrontationFinish;
    }

    private void OnDisable()
    {
        EventSystem.OnSetLightIntensity -= HandleOtherLightIntensity;
        EventSystem.OnFinishFinalConfrontation -= DestroyOnConfrontationFinish;
    }

    private void Start()
    {
        originalIntensity = campfireLight.range;
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

    private void HandleOtherLightIntensity(SetLightIntesityEvent gameEvent)
    {
        runningOtherLightIntensity = true;
        otherCurveEvaluationSpeed = gameEvent.curveEvaluationSpeed;
        otherLightIntensityCurve = gameEvent.curve;

        if (!preserveIntensity)
        {
            otherMaxIntensity = gameEvent.maxIntensity;
        }

        if (preserveMaxRange)
        {
            if (newRange < originalIntensity)
            {
                this.newRange = gameEvent.newRange;
            }
            else
            {
                this.newRange = originalRange;
            }
        }
        else
        {
            this.newRange = gameEvent.newRange;
        }

        prevRange = campfireLight.range;
        originalIntensity = campfireLight.intensity;
        currTime = 0;
        currEvent = gameEvent;

        if (currEvent.markedAsCompletedTimeOnCurve == 0)
        {
            currEvent.GameEventCompleted(currEvent);
            currEvent = null;
        }

        StartCoroutine(Co_TransitionToOtherLightIntensity());
    }

    private bool RunLightIntensity()
    {
        campfireLight.intensity = otherMaxIntensity * otherLightIntensityCurve.Evaluate(currTime * otherCurveEvaluationSpeed);
        campfireLight.range = Mathf.Lerp(prevRange, newRange, currTime * otherCurveEvaluationSpeed);
        currTime += Time.deltaTime;

        if (currEvent != null && currTime * otherCurveEvaluationSpeed >= currEvent.markedAsCompletedTimeOnCurve)
        {
            currEvent.GameEventCompleted(currEvent);
            currEvent = null;
        }

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

        StartCoroutine(Co_TransitionFromLightIntensity());
    }

    private IEnumerator Co_TransitionToOtherLightIntensity()
    {
        yield return new WaitUntil(RunToOtherLightIntensity);
        currTime = 0;

        if (destroyAfterSwitch)
        {
            Destroy(gameObject);
        }

        StartCoroutine(Co_ExecuteOtherLightIntensity());
    }

    private IEnumerator Co_TransitionFromLightIntensity()
    {
        yield return new WaitUntil(RunFromOtherLightIntensity);
        currTime = 0;
        runningOtherLightIntensity = false;
    }

    private void DestroyOnConfrontationFinish()
    {
        destroyAfterSwitch = true;
    }
}

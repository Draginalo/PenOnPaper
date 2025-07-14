using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class ColorFadeEvent : GameEvent
{
    [SerializeField] private AnimationCurve fadeCurve;
    [SerializeField] private Color fadeColor;
    [SerializeField] private float curveEvaluationSpeed;
    [SerializeField] private float markedAsCompletedTimeOnCurve = 1.0f;
    [SerializeField] private Material fadeMat;
    private float currTime = 0;
    ColorFadeSettings colorFadeSettings;
    private bool markedDone = false;

    public override void Execute()
    {
        base.Execute();

        Volume volume = GameObject.FindGameObjectWithTag("MainGlobalVolume").GetComponent<Volume>();
        if (volume.profile.TryGet(out colorFadeSettings))
        {
            colorFadeSettings.active = true;
            colorFadeSettings.fadeColor = fadeColor;
        }

        fadeMat.SetColor("_ColorToFadeTo", fadeColor);
        StartCoroutine(Co_RunFade());
    }

    public override void Cleanup(bool destroyParentComponent)
    {
        //Handles its own cleanup
        //base.Cleanup(destroyParentComponent);
    }

    public override void GameEventCompleted(GameEvent eventCompleted)
    {
        base.GameEventCompleted(eventCompleted);
        markedDone = true;
    }

    public void HandleEffectDone()
    {
        if (colorFadeSettings)
        {
            colorFadeSettings.active = false;
        }

        base.Cleanup(destroyParent);
    }

    private bool FaintFinished()
    {
        colorFadeSettings.lerpValue.value = fadeCurve.Evaluate(currTime * curveEvaluationSpeed);

        currTime += Time.deltaTime;

        //This is to stop the event so another event can be triggered while this one is still running
        if (!markedDone && currTime * curveEvaluationSpeed >= markedAsCompletedTimeOnCurve)
        {
            GameEventCompleted(this);
        }

        return currTime * curveEvaluationSpeed >= 1;
    }

    private IEnumerator Co_RunFade()
    {
        yield return new WaitUntil(FaintFinished);
        HandleEffectDone();
    }

    public bool GetIsDone()
    {
        return markedDone;
    }
}

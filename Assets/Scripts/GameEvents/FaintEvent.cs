using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class FaintEvent : GameEvent
{
    [SerializeField] private AnimationCurve VignetteCurve;
    [SerializeField] private float blurrMultiple = 15.0f;
    [SerializeField] private float colorMultiple = 0.1f;
    //[SerializeField] private float vignetteMultiple = 100.0f;
    [SerializeField] private float curveEvaluationSpeed;
    BlurSettings blurSettings;
    private float currTime = 0;

    private Material vignetteMat;

    public override void Execute()
    {
        base.Execute();

        Volume volume = GameObject.FindGameObjectWithTag("MainGlobalVolume").GetComponent<Volume>();
        if (volume.profile.TryGet(out blurSettings))
        {
            blurSettings.active = true;
            blurSettings.blurrStrength.value = 10.0f;
        }


        StartCoroutine(Co_RunFaintEffect());
    }

    public override void GameEventCompleted()
    {
        if (blurSettings)
        {
            blurSettings.active = false;
        }

        base.GameEventCompleted();
    }

    private bool FaintFinished()
    {
        blurSettings.vignetteSpread.value = VignetteCurve.Evaluate(currTime * curveEvaluationSpeed);
        //blurSettings.vignetteStrength.value = blurSettings.vignetteStrength.max - (1 - VignetteCurve.Evaluate(currTime * curveEvaluationSpeed)) * vignetteMultiple;
        blurSettings.colorStrength.value = VignetteCurve.Evaluate(currTime * curveEvaluationSpeed) * colorMultiple;
        blurSettings.blurrStrength.value = VignetteCurve.Evaluate(currTime * curveEvaluationSpeed) * blurrMultiple;

        currTime += Time.deltaTime;

        return currTime * curveEvaluationSpeed >= 1;
    }

    private IEnumerator Co_RunFaintEffect()
    {
        yield return new WaitUntil(FaintFinished);
        GameEventCompleted();
    }
}

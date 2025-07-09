using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class ColorFadeEvent : GameEvent
{
    [SerializeField] private AnimationCurve fadeCurve;
    [SerializeField] private Color fadeColor;
    [SerializeField] private float curveEvaluationSpeed;
    [SerializeField] private Material fadeMat;
    private float currTime = 0;
    ColorFadeSettings colorFadeSettings;

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

    public void HandleEffectDone()
    {
        if (colorFadeSettings)
        {
            colorFadeSettings.active = false;
        }

        GameEventCompleted(this);
    }

    private bool FaintFinished()
    {
        colorFadeSettings.lerpValue.value = fadeCurve.Evaluate(currTime * curveEvaluationSpeed);

        currTime += Time.deltaTime;

        return currTime * curveEvaluationSpeed >= 1;
    }

    private IEnumerator Co_RunFade()
    {
        yield return new WaitUntil(FaintFinished);
        HandleEffectDone();
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static DrawingManager;

public class FaintEvent : GameEvent
{
    [SerializeField] private AnimationCurve VignetteCurve;
    [SerializeField] private float curveEvaluationSpeed;
    BlurSettings blurSettings;
    private float currTime = 0;
    [SerializeField] private Volume v;

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

    public override void Completed()
    {
        base.Completed();
        EventSystem.TriggerNextSketch(DrawingCompleteTrigger.LOOKING_DOWN);
    }

    private bool FaintFinished()
    {
        blurSettings.SetVignetteStrength(VignetteCurve.Evaluate(currTime * curveEvaluationSpeed));
        blurSettings.SetBlurStrength(VignetteCurve.Evaluate(currTime * curveEvaluationSpeed));

        currTime += Time.deltaTime;

        return currTime * curveEvaluationSpeed >= 1;
    }

    private IEnumerator Co_RunFaintEffect()
    {
        yield return new WaitUntil(FaintFinished);

    }
}

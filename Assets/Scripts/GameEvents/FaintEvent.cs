using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class FaintEvent : GameEvent
{
    [SerializeField] private AnimationCurve VignetteCurve;
    [SerializeField] private float blurrMultiple = 15.0f;
    [SerializeField] private float colorMultiple = 0.1f;
    [SerializeField] private float vignetteMultiple = 100.0f;
    [SerializeField] private float curveEvaluationSpeed;
    [SerializeField] private float markedAsCompletedTimeOnCurve = 1.0f;
    [SerializeField] private AudioClip faintSound;
    [SerializeField] private float timeToFadeOut = 1.0f;
    [SerializeField] private float fadeOutTime = 1.0f;
    [SerializeField] private AudioSource audioSource;
    BlurSettings blurSettings;
    private float currTime = 0;
    private bool markedDone = false;
    private bool fadeOutStarted = false;
    public bool overideCleanup = false;

    private Coroutine coroutine;

    private Material vignetteMat;

    public float BlurrMultiple { get { return blurrMultiple; } set { blurrMultiple = value; } }
    public float ColorMultiple { get { return colorMultiple; } set { colorMultiple = value; } }
    public float VignetteMultiple { get { return vignetteMultiple; } set { vignetteMultiple = value; } }
    public float CurveEvaluationSpeed { get { return curveEvaluationSpeed; } set { curveEvaluationSpeed = value; } }

    protected override void Awake()
    {
        base.Awake();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public float GetCurrTime()
    {
        return currTime;
    }

    public float GetCurrValue()
    {
        return VignetteCurve.Evaluate(currTime * curveEvaluationSpeed);
    }

    public void SetCurve(AnimationCurve newCurve)
    {
        VignetteCurve = newCurve;
    }

    public override void Execute()
    {
        base.Execute();

        Volume volume = GameObject.FindGameObjectWithTag("MainGlobalVolume").GetComponent<Volume>();
        if (volume.profile.TryGet(out blurSettings))
        {
            blurSettings.active = true;
            blurSettings.blurrStrength.value = 10.0f;
        }

        SoundManager.instance.LoadAndPlaySound(faintSound, 1.0f, false, audioSource);
        coroutine = StartCoroutine(Co_RunFaintEffect());
    }

    public override void GameEventCompleted(GameEvent eventCompleted)
    {
        base.GameEventCompleted(eventCompleted);
        markedDone = true;
    }

    public void ResetEvent()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        currTime = 0;
    }

    public override void Cleanup(bool destroyParentComponent)
    {
        //Handles its own cleanup
        //base.Cleanup(destroyParentComponent);
    }

    public void HandleEffectDone()
    {
        if (blurSettings)
        {
            blurSettings.active = false;
        }

        Destroy(audioSource);

        if (!overideCleanup)
        {
            base.Cleanup(true);
        }
    }

    private bool FaintFinished()
    {
        blurSettings.vignetteSpread.value = VignetteCurve.Evaluate(currTime * curveEvaluationSpeed);
        blurSettings.vignetteStrength.value = VignetteCurve.Evaluate(currTime * curveEvaluationSpeed) * vignetteMultiple;
        blurSettings.colorStrength.value = VignetteCurve.Evaluate(currTime * curveEvaluationSpeed) * colorMultiple;
        blurSettings.blurrStrength.value = VignetteCurve.Evaluate(currTime * curveEvaluationSpeed) * blurrMultiple;

        currTime += Time.deltaTime;

        //This is to stop the event so another event can be triggered while this one is still running
        if (!markedDone && currTime * curveEvaluationSpeed >= markedAsCompletedTimeOnCurve)
        {
            GameEventCompleted(this);
        }

        if (!fadeOutStarted && currTime * curveEvaluationSpeed >= timeToFadeOut)
        {
            SoundManager.instance.FadeOutLoadedSound(fadeOutTime);
            fadeOutStarted = true;
        }

            return currTime * curveEvaluationSpeed >= 1;
    }

    private IEnumerator Co_RunFaintEffect()
    {
        yield return new WaitUntil(FaintFinished);
        HandleEffectDone();
    }
}

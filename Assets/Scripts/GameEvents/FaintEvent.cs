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
    [SerializeField] private bool playOneShot = false;
    [SerializeField] private float fadeInTime = 0.0f;
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

        currTime = 0;

        Volume volume = GameObject.FindGameObjectWithTag("MainGlobalVolume").GetComponent<Volume>();
        if (volume.profile.TryGet(out blurSettings))
        {
            blurSettings.active = true;
            blurSettings.blurrStrength.value = 10.0f;
        }

        coroutine = StartCoroutine(Co_RunFaintEffect());

        if (playOneShot)
        {
            SoundManager.instance.PlayOneShotSound(faintSound, 1.0f);
            fadeOutStarted = true;
            return;
        }

        if (faintSound != null)
        {
            if (fadeInTime == 0.0f)
            {
                SoundManager.instance.LoadAndPlaySound(faintSound, 1.0f, false, audioSource);
                return;
            }

            SoundManager.instance.LoadAndFadeInSound(faintSound, 1.0f, false, fadeInTime, audioSource);
        }
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

        fadeOutStarted = false;
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
        float normalizedTime = currTime * curveEvaluationSpeed;

        blurSettings.vignetteSpread.value = VignetteCurve.Evaluate(normalizedTime);
        blurSettings.vignetteStrength.value = VignetteCurve.Evaluate(normalizedTime) * vignetteMultiple;
        blurSettings.colorStrength.value = VignetteCurve.Evaluate(normalizedTime) * colorMultiple;
        blurSettings.blurrStrength.value = VignetteCurve.Evaluate(normalizedTime) * blurrMultiple;

        currTime += Time.deltaTime;
        normalizedTime = currTime * curveEvaluationSpeed;

        //This is to stop the event so another event can be triggered while this one is still running
        if (!markedDone && normalizedTime >= markedAsCompletedTimeOnCurve)
        {
            GameEventCompleted(this);
        }

        if (normalizedTime >= timeToFadeOut)
        {
            TriggerFadeOut();
        }

            return normalizedTime >= 1;
    }

    private IEnumerator Co_RunFaintEffect()
    {
        yield return new WaitUntil(FaintFinished);
        HandleEffectDone();
    }

    public void TriggerFadeOut()
    {
        if (!fadeOutStarted)
        {
            if (faintSound != null)
            {
                SoundManager.instance.FadeOutLoadedSound(fadeOutTime, audioSource);
            }

            fadeOutStarted = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightIntesityEvent : GameEvent
{
    public AnimationCurve curve;
    public float maxIntensity;
    public float newRange;
    public float curveEvaluationSpeed;
    public float markedAsCompletedTimeOnCurve = 0.0f;
    [SerializeField] private AudioClip sfx;

    public void SetSFX(AudioClip sfx)
    {
        this.sfx = sfx;
    }

    public override void Execute()
    {
        base.Execute();

        if (sfx != null)
        {
            SoundManager.instance.PlayOneShotSound(sfx);
        }

        EventSystem.SetLightIntensity(this);

        //GameEventCompleted(this);
    }
}

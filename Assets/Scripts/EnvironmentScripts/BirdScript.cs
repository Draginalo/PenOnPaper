using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : ClickableObject
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip birdChirp;
    [SerializeField] private AudioClip birdWings;
    [SerializeField] private AudioClip birdScreach;

    public override void Clicked()
    {
        animator.SetBool("ScareAway", true);
        CameraHandler.instance.SetActiveCameraActivators(false, false);
    }

    public void TriggerNextEventChain()
    {
        EventSystem.TriggerNextEventChain();
    }

    public void PlayWingsSound()
    {
        SoundManager.instance.PlayOneShotSound(birdWings, 1.0f, transform.position);
    }

    public void PlayChirpSound()
    {
        SoundManager.instance.PlayOneShotSound(birdChirp, 1.0f, transform.position);
    }

    public void PlayScreachSound()
    {
        SoundManager.instance.PlayOneShotSound(birdScreach, 1.0f, transform.position);
    }
}

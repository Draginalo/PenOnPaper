using System.Collections;
using UnityEngine;

public class SpawnDoctorWithAnimation : GameEvent
{
    public GameObject m_Doctor;
    public string animationToTrigger;
    [SerializeField] private bool enableRootMotion;
    [SerializeField] private Vector3 startingPosition;
    [SerializeField] private Vector3 startingRotation;
    [SerializeField] private bool useAnimationToTriggerCompletion;
    [SerializeField] private bool deactivateSmokeEffect;
    [SerializeField] private AudioClip sfxToPlay;
    private GameObject mDoctorInstance;

    public override void Execute()
    {
        base.Execute();
        mDoctorInstance = Instantiate(m_Doctor, startingPosition, Quaternion.identity);
        mDoctorInstance.transform.eulerAngles = startingRotation;

        Animator animator = mDoctorInstance.GetComponent<Animator>();

        if (animator != null && animationToTrigger != "")
        {
            animator.SetBool(animationToTrigger, true);
        }

        if (sfxToPlay != null)
        {
            SoundManager.instance.PlayOneShotSound(sfxToPlay, 1.0f);
        }

        animator.applyRootMotion = enableRootMotion;

        if (deactivateSmokeEffect)
        {
            mDoctorInstance.GetComponent<DoctorScript>().DeactivateSmokeEffect();
        }

        EventSystem.SpawnDoctor();

        if (!useAnimationToTriggerCompletion)
        {
            GameEventCompleted(this);
            return;
        }

        mDoctorInstance.GetComponent<AnimationHandler>().sourceGameEvent = this;
    }
}

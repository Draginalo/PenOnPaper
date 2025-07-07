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
    private GameObject mDoctorInstance;

    public override void Execute()
    {
        base.Execute();
        mDoctorInstance = Instantiate(m_Doctor, startingPosition, Quaternion.identity);
        mDoctorInstance.transform.eulerAngles = startingRotation;

        Animator animator = mDoctorInstance.GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetBool(animationToTrigger, true);
        }

        animator.applyRootMotion = enableRootMotion;

        EventSystem.SpawnDoctor();

        if (!useAnimationToTriggerCompletion)
        {
            GameEventCompleted(this);
            return;
        }

        mDoctorInstance.GetComponent<AnimationHandler>().sourceGameEvent = this;
    }
}

using System.Collections;
using UnityEngine;

public class SpawnDoctorGrave : GameEvent
{
    [SerializeField] private GameObject m_Doctor;
    private GameObject mDoctorInstance;

    public override void Execute()
    {
        base.Execute();
        mDoctorInstance = Instantiate(m_Doctor);

        Animator animator = mDoctorInstance.GetComponent<Animator>();

        animator.SetBool("StartGraveAni", true);
        animator.applyRootMotion = false;

        //EventSystem.SpawnDoctor();

        //GameEventCompleted(this);
        mDoctorInstance.GetComponent<AnimationHandler>().sourceGameEvent = this;
    }
}

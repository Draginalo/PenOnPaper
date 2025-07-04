using UnityEngine;

public class SpawnDoctorLake : GameEvent
{
    [SerializeField] private GameObject m_Doctor;

    public override void Execute()
    {
        base.Execute();
        GameObject doctor = Instantiate(m_Doctor);

        Animator animator = doctor.GetComponent<Animator>();
        animator.applyRootMotion = true;

        doctor.transform.position = new Vector3(-40.35f, 2.41f, 90.39f);
        doctor.transform.eulerAngles = new Vector3(0, 155.34f, 0);

        EventSystem.SpawnDoctor();

        //Make this timed exactly when the environment has swapped
        GameEventCompleted(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalDoorScript : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;

    private void Awake()
    {
        m_Animator.enabled = false;
    }

    private void OnEnable()
    {
        EventSystem.OnOpenHospitalDoor += HandleOpenDoor;
        EventSystem.OnStopOpening += ResetDoor;
    }

    private void OnDisable()
    {
        EventSystem.OnOpenHospitalDoor -= HandleOpenDoor;
        EventSystem.OnStopOpening -= ResetDoor;
    }

    private void HandleOpenDoor(bool slowOpen)
    {
        m_Animator.enabled = true;

        m_Animator.SetBool("SlowOpen", slowOpen);
    }

    private void ResetDoor()
    {
        m_Animator.Rebind();
        m_Animator.Update(0f);
        m_Animator.enabled = false;
    }
}

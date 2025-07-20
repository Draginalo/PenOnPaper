using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalDoorScript : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private AudioClip m_DoorSFX;

    private void Awake()
    {
        m_Animator.enabled = false;
    }

    private void OnEnable()
    {
        EventSystem.OnOpenHospitalDoor += HandleOpenDoor;
        EventSystem.OnFixConfrontationIssue += ResetDoor;
    }

    private void OnDisable()
    {
        EventSystem.OnOpenHospitalDoor -= HandleOpenDoor;
        EventSystem.OnFixConfrontationIssue -= ResetDoor;
    }

    private void HandleOpenDoor(bool slowOpen)
    {
        if (m_DoorSFX != null)
        {
            SoundManager.instance.PlayOneShotSound(m_DoorSFX, 1.0f, transform.position);
        }

        m_Animator.enabled = true;

        m_Animator.SetBool("SlowOpen", slowOpen);
    }

    private void ResetDoor(FinalConfrontationManager.Issues issueFixed)
    {
        m_Animator.Rebind();
        m_Animator.Update(0f);
        m_Animator.enabled = false;
    }
}

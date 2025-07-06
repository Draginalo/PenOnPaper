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
    }

    private void OnDisable()
    {
        EventSystem.OnOpenHospitalDoor -= HandleOpenDoor;
    }

    private void HandleOpenDoor()
    {
        m_Animator.enabled = true;
    }
}

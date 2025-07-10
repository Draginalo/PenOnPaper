using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalSignScript : MonoBehaviour
{
    [SerializeField] private Material m_SignMat;

    private void Start()
    {
        m_SignMat.EnableKeyword("_EMISSION");
    }

    private void OnEnable()
    {
        EventSystem.OnTurnOffHospitalSign += HandleSignOff;
    }

    private void OnDisable()
    {
        EventSystem.OnTurnOffHospitalSign -= HandleSignOff;
    }

    private void HandleSignOff()
    {
        m_SignMat.DisableKeyword("_EMISSION");
    }
}

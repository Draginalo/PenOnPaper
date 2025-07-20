using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalSignScript : MonoBehaviour
{
    [SerializeField] private Material m_SignMat;
    [SerializeField] private AudioClip m_SFX;

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
        if (m_SignMat != null)
        {
            SoundManager.instance.PlayOneShotSound(m_SFX);
        }

        m_SignMat.DisableKeyword("_EMISSION");
    }
}

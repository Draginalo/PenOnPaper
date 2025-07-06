using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalVoidScript : MonoBehaviour
{
    [SerializeField] private GameObject envParent;

    private void OnEnable()
    {
        EventSystem.OnActivateHospitalVoidEnv += ActivateEnv;
    }

    private void OnDisable()
    {
        EventSystem.OnActivateHospitalVoidEnv -= ActivateEnv;
    }

    private void Start()
    {
        envParent.SetActive(false);
    }

    private void ActivateEnv()
    {
        envParent.SetActive(true);
    }
}

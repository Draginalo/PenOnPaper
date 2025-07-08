using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalVoidScript : MonoBehaviour
{
    [SerializeField] private GameObject envParent;

    private void OnEnable()
    {
        EventSystem.OnToggleActivateHospitalVoidEnv += ToggleActivateEnv;
    }

    private void OnDisable()
    {
        EventSystem.OnToggleActivateHospitalVoidEnv -= ToggleActivateEnv;
    }

    private void Start()
    {
        envParent.SetActive(false);
    }

    private void ToggleActivateEnv()
    {
        envParent.SetActive(!envParent.activeInHierarchy);
    }

}

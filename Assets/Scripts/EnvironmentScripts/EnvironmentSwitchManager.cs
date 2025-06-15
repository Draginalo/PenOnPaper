using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSwitchManager : MonoBehaviour
{
    [SerializeField] private EnvironmentData[] environments;
    private GameObject currEnv;

    [SerializeField] private float envDelayTime = 0.15f;

    [SerializeField] private Material skyboxMat;

    [Serializable]
    private struct EnvironmentData
    {
        public GameObject envOBJ;
        public Texture skybox;
    }

    public enum Environments
    {
        None,
        Campsite
    }

    private void OnEnable()
    {
        EventSystem.OnSwapEnvironments += HandleEnvironmentSwap;
    }

    private void OnDisable()
    {
        EventSystem.OnSwapEnvironments -= HandleEnvironmentSwap;
    }

    private void Start()
    {
        //Gets first active environment when starting
        currEnv = GetComponentInChildren<Transform>().gameObject;
    }

    private void HandleEnvironmentSwap(Environments newEnv)
    {
        StartCoroutine(Co_DelaySwap(newEnv));
    }

    private IEnumerator Co_DelaySwap(Environments newEnv)
    {
        if (newEnv != Environments.None)
        {
            yield return new WaitForSeconds(envDelayTime);
            environments[(int)newEnv].envOBJ.SetActive(true);
        }

        currEnv.SetActive(false);

        skyboxMat.SetTexture("_Tex", environments[(int)newEnv].skybox);
    }
}

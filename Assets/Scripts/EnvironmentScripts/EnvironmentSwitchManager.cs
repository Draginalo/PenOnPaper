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
        NONE,
        CAMPSITE,
        LAKE,
        HOSPITAL,
        GRAVEYARD
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
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i) != transform && transform.GetChild(i).gameObject.activeInHierarchy)
            {
                currEnv = transform.GetChild(i).gameObject;
            }

            break;
        }
    }

    private void HandleEnvironmentSwap(Environments newEnv)
    {
        StartCoroutine(Co_DelaySwap(newEnv));
    }

    private IEnumerator Co_DelaySwap(Environments newEnv)
    {
        Debug.Log((int)newEnv);
        if (newEnv != Environments.NONE)
        {
            yield return new WaitForSeconds(envDelayTime);

            if (currEnv != null)
            {
                currEnv.SetActive(false);
            }
            currEnv = environments[(int)newEnv].envOBJ;
            currEnv.SetActive(true);
        }
        else
        {
            if (currEnv != null)
            {
                currEnv.SetActive(false);
            }
        }

        skyboxMat.SetTexture("_Tex", environments[(int)newEnv].skybox);
    }
}

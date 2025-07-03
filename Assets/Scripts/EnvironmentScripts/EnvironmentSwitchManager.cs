using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnvironmentSwitchManager : MonoBehaviour
{
    [SerializeField] private EnvironmentData[] environments;
    private GameObject currEnv;

    [SerializeField] private float envDelayTime = 0.15f;

    [SerializeField] private Material skyboxMat;

    private ReflectionProbe baker;

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
                break;
            }
        }

        baker = gameObject.AddComponent<ReflectionProbe>();
        baker.cullingMask = 0;
        baker.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
        baker.mode = ReflectionProbeMode.Realtime;
        baker.timeSlicingMode = ReflectionProbeTimeSlicingMode.NoTimeSlicing;

        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
    }

    private void HandleEnvironmentSwap(Environments newEnv, GameEvent gameEvent)
    {
        StartCoroutine(Co_DelaySwap(newEnv, gameEvent));
    }

    private IEnumerator Co_DelaySwap(Environments newEnv, GameEvent gameEvent)
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

        gameEvent.GameEventCompleted(gameEvent);
        DynamicGI.UpdateEnvironment();

        baker.RenderProbe();
        yield return new WaitForEndOfFrame();
        RenderSettings.customReflectionTexture = baker.texture;
    }
}

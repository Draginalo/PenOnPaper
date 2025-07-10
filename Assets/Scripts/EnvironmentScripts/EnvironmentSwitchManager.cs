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
    [SerializeField] private float envCrossFadeTime = 0.15f;

    [SerializeField] private Material skyboxMat;
    [SerializeField] private Material mat;

    private ReflectionProbe baker;
    private bool takeScreenshot = false;
    private Texture2D screenShot;
    private float totalTime = 0;

    [Serializable]
    private struct EnvironmentData
    {
        public GameObject envOBJ;
        public Texture skybox;
        public float transitionDarkness;
    }

    public enum Environments
    {
        NONE,
        CAMPSITE,
        LAKE,
        HOSPITAL,
        GRAVEYARD,
        HOSPITAL2,
        HOSPITAL_VOID,
        END_GAME_HALLWAYS
    }

    private void OnEnable()
    {
        EventSystem.OnSwapEnvironments += HandleEnvironmentSwap;
        RenderPipelineManager.endCameraRendering += TakeSnapshotOfCamView;
    }

    private void OnDisable()
    {
        EventSystem.OnSwapEnvironments -= HandleEnvironmentSwap;
        RenderPipelineManager.endCameraRendering -= TakeSnapshotOfCamView;
    }

    private void TakeSnapshotOfCamView(ScriptableRenderContext context, Camera cam)
    {
        if (takeScreenshot) //Figure out a better and more efficient way to do this
        {
            screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);

            // Define the parameters for the ReadPixels operation
            Rect regionToReadFrom = new Rect(0, 0, Screen.width, Screen.height);
            int xPosToWriteTo = 0;
            int yPosToWriteTo = 0;
            bool updateMipMapsAutomatically = false;

            // Copy the pixels from the Camera's render target to the texture
            screenShot.ReadPixels(regionToReadFrom, xPosToWriteTo, yPosToWriteTo, updateMipMapsAutomatically);
            screenShot.Apply();
        }
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

        //Sets up baker to reset reflections when switching
        baker = gameObject.AddComponent<ReflectionProbe>();
        baker.cullingMask = 0;
        baker.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
        baker.mode = ReflectionProbeMode.Realtime;
        baker.timeSlicingMode = ReflectionProbeTimeSlicingMode.NoTimeSlicing;

        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
    }

    private void HandleEnvironmentSwap(Environments newEnv, GameEvent gameEvent)
    {
        if (newEnv != Environments.NONE && newEnv != Environments.HOSPITAL_VOID && newEnv != Environments.END_GAME_HALLWAYS)
        {
            takeScreenshot = true;
        }

        StartCoroutine(Co_DelaySwap(newEnv, gameEvent));
    }

    private bool Delayed()
    {
        totalTime += Time.deltaTime;

        mat.SetFloat("_LerpValue", totalTime / envCrossFadeTime);

        return totalTime > envCrossFadeTime;
    }

    private IEnumerator Co_DelaySwap(Environments newEnv, GameEvent gameEvent)
    {
        if (newEnv != Environments.NONE && newEnv != Environments.HOSPITAL_VOID && newEnv != Environments.END_GAME_HALLWAYS)
        {
            yield return new WaitForSeconds(envDelayTime);

            mat.SetFloat("_Darkness", environments[(int)newEnv].transitionDarkness);
            StartCoroutine(Co_HandleSwapEffect());

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

            if (newEnv != Environments.NONE)
            {
                currEnv = environments[(int)newEnv].envOBJ;
                currEnv.SetActive(true);
            }
        }

        skyboxMat.SetTexture("_Tex", environments[(int)newEnv].skybox);

        //Resets ambient lighting for new skybox texture
        DynamicGI.UpdateEnvironment();

        //Resets reflections for new skybox texture
        baker.RenderProbe();
        yield return new WaitForEndOfFrame();
        RenderSettings.customReflectionTexture = baker.texture;

        gameEvent.GameEventCompleted(gameEvent);
    }

    private IEnumerator Co_HandleSwapEffect()
    {
        takeScreenshot = false;
        mat.SetTexture("_Texture", screenShot);
        mat.SetFloat("_LerpValue", 1.0f);
        totalTime = 0.0f;
        yield return new WaitUntil(Delayed);

        mat.SetFloat("_LerpValue", 1);
    }
}

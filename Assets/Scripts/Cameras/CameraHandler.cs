using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _UpCam;
    [SerializeField] private CinemachineVirtualCamera _DowmCam;
    [SerializeField] private CinemachineVirtualCamera _EndCam;
    [SerializeField] private GameObject _UpCamActivator;
    [SerializeField] private GameObject _DownCamActivator;
    [SerializeField] private AudioClip _CamSwooshSFX;
    [SerializeField] private float cameraSwitchTime = 0.3f;
    private bool currentlylookingDown = false;
    private CinemachineBrain camBrain;

    private Vector3 originalCamPos;
    private Vector3 targetPos;
    private Vector3 prevPos;
    private float shakeStrength;
    private float shakeSpeed = 0.1f;
    private float shakeTime;

    public static CameraHandler instance;
    private Coroutine swapCoroutine;

    public bool CurrentlyLookingDown { get { return currentlylookingDown; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        _UpCamActivator.SetActive(false);
        camBrain = GetComponent<CinemachineBrain>();
    }

    private void OnDisable()
    {
        
    }

    public void SwitchToUpCam()
    {
        SoundManager.instance.PlayOneShotSound(_CamSwooshSFX);
        _UpCam.MoveToTopOfPrioritySubqueue();
        currentlylookingDown = false;
        _UpCamActivator.SetActive(false);
        swapCoroutine = StartCoroutine(Co_DelayEnableButton(_DownCamActivator));
        EventSystem.CameraLookChange(true);
    }

    public void SwitchToDownCam()
    {
        SoundManager.instance.PlayOneShotSound(_CamSwooshSFX);
        _DowmCam.MoveToTopOfPrioritySubqueue();
        currentlylookingDown = true;
        _DownCamActivator.SetActive(false);
        swapCoroutine = StartCoroutine(Co_DelayEnableButton(_UpCamActivator));
        EventSystem.CameraLookChange(false);
    }

    private IEnumerator Co_DelayEnableButton(GameObject buttonToEnable)
    {
        yield return new WaitForSeconds(cameraSwitchTime);
        buttonToEnable.SetActive(true);
    }

    public void InitCameraShake(float shakeStrength, float shakeSpeed)
    {
        originalCamPos = transform.position;
        targetPos = originalCamPos + Random.insideUnitSphere * shakeStrength;
        prevPos = originalCamPos;
        this.shakeStrength = shakeStrength;
        this.shakeSpeed = shakeSpeed;

        StartCoroutine(Co_MoveCamToNewPos(false));
    }

    public void StopCameraShake()
    {
        shakeStrength = 0.0f;
    }

    private bool MovedToNewPos()
    {
        shakeTime += Time.deltaTime;

        transform.position = Vector3.Lerp(prevPos, targetPos, shakeTime / shakeSpeed);

        return shakeTime >= shakeSpeed;
    }

    private IEnumerator Co_MoveCamToNewPos(bool finished)
    {      
        shakeTime = 0;
        yield return new WaitUntil(MovedToNewPos);

        prevPos = transform.position;

        if (!finished)
        {
            if (shakeStrength > 0)
            {
                targetPos = originalCamPos + Random.insideUnitSphere * shakeStrength;
                StartCoroutine(Co_MoveCamToNewPos(false));
            }
            else
            {
                targetPos = originalCamPos;
                StartCoroutine(Co_MoveCamToNewPos(true));
            }
        }
    }

    public void HandleEndCamSequence(GameEvent possibleGameEvent)
    {
        _EndCam.MoveToTopOfPrioritySubqueue();
        _EndCam.GetComponent<Animator>().enabled = true;
        _EndCam.GetComponent<AnimationHandler>().sourceGameEvent = possibleGameEvent;
    }

    public bool IsTransitioning()
    {
        return camBrain.IsBlending;
    }

    public void SetActiveCameraActivators(bool topActive, bool bottomActive)
    {
        if (swapCoroutine != null)
        {
            StopCoroutine(swapCoroutine);
        }

        _UpCamActivator.transform.parent.parent.gameObject.SetActive(true);
        _UpCamActivator.SetActive(topActive);
        _DownCamActivator.SetActive(bottomActive);
    }
}

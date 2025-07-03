using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _UpCam;
    [SerializeField] private CinemachineVirtualCamera _DowmCam;
    [SerializeField] private GameObject _UpCamActivator;
    [SerializeField] private GameObject _DownCamActivator;
    [SerializeField] private float cameraSwitchTime = 0.3f;
    private bool currentlylookingDown = false;

    public static CameraHandler instance;

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
    }

    public void SwitchToUpCam()
    {
        _UpCam.MoveToTopOfPrioritySubqueue();
        EventSystem.CameraLookChange(true);
        currentlylookingDown = false;
        _UpCamActivator.SetActive(false);
        StartCoroutine(Co_DelayEnableButton(_DownCamActivator));
    }

    public void SwitchToDownCam()
    {
        _DowmCam.MoveToTopOfPrioritySubqueue();
        EventSystem.CameraLookChange(false);
        currentlylookingDown = true;
        _DownCamActivator.SetActive(false);
        StartCoroutine(Co_DelayEnableButton(_UpCamActivator));
    }

    private IEnumerator Co_DelayEnableButton(GameObject buttonToEnable)
    {
        yield return new WaitForSeconds(cameraSwitchTime);
        buttonToEnable.SetActive(true);
    }
}

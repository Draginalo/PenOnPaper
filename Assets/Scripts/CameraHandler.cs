using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _UpCam;
    [SerializeField] private CinemachineVirtualCamera _DowmCam;
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
    }

    public void SwitchToUpCam()
    {
        _UpCam.MoveToTopOfPrioritySubqueue();
        EventSystem.CameraLookChange(true);
        currentlylookingDown = false;
    }

    public void SwitchToDownCam()
    {
        _DowmCam.MoveToTopOfPrioritySubqueue();
        EventSystem.CameraLookChange(false);
        currentlylookingDown = true;
    }
}

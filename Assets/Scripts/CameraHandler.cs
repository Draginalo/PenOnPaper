using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _UpCam;
    [SerializeField] private CinemachineVirtualCamera _DowmCam;

    public void SwitchToUpCam()
    {
        _UpCam.MoveToTopOfPrioritySubqueue();
        EventSystem.CameraLookChange(true);
    }

    public void SwitchToDownCam()
    {
        _DowmCam.MoveToTopOfPrioritySubqueue();
        EventSystem.CameraLookChange(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera mainMenuCam;
    public LoadGameEventChains startingEventsToLoad;
    private float referenceAspect = 1.777f;
    private float aspectMultiplyer = 2f;

    private void Awake()
    {
        transform.position -= new Vector3(0, 0, (1 - (referenceAspect / Camera.main.aspect)) * aspectMultiplyer);
    }

    public void HandleStartGame()
    {
        mainMenuCam.Priority = 0;
        CameraHandler.instance.SwitchToUpCam();
        StartCoroutine(Co_WaitUntilCameraTransitioned());
    }

    private bool IsNotDoneTransitioning()
    {
        return CameraHandler.instance.IsTransitioning();
    }

    private IEnumerator Co_WaitUntilCameraTransitioned()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitWhile(IsNotDoneTransitioning);
        CameraHandler.instance.SetActiveCameraActivators(false, true);
        startingEventsToLoad.LoadSelf();
        Destroy(gameObject);
    }
}

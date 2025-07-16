using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera mainMenuCam;
    public LoadGameEventChains startingEventsToLoad;
    public GameObject mMainMenu;
    public GameObject mModeMenu;
    public TextMeshProUGUI explinationText;
    public GameObject explinationPanel;
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
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitWhile(IsNotDoneTransitioning);
        CameraHandler.instance.SetActiveCameraActivators(false, true);
        startingEventsToLoad.LoadSelf();
        Destroy(gameObject);
    }

    public void SwitchToModeMenu()
    {
        mMainMenu.SetActive(false);
        mModeMenu.SetActive(true);
    }

    public void SwitchToMainMenu()
    {
        mMainMenu.SetActive(true);
        mModeMenu.SetActive(false);
    }

    public void HandleHoverFreeModeButton()
    {
        explinationText.text = "Lets you freely sketch without many constraints.";
        explinationPanel.SetActive(true);
    }

    public void HandleHoverOutlineModeButton()
    {
        explinationText.text = "Only allows you to sketch on an outline to guide the sketch (Though the limitations might be a bit annoying)";
        explinationPanel.SetActive(true);
    }

    public void HandlePointerExit()
    {
        explinationPanel.SetActive(false);
    }

    public void SelectFreeMode()
    {
        GameManager.instance.IsFreeMode = true;
        SwitchToMainMenu();
    }

    public void SelectNotFreeMode()
    {
        GameManager.instance.IsFreeMode = false;
        SwitchToMainMenu();
    }

    public void Quit()
    {
        Application.Quit();
    }
}

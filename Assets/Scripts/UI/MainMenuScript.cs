using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera mainMenuCam;
    public LoadGameEventChains startingEventsToLoad;
    public GameObject mMainMenu;
    public GameObject mModeMenu;
    public TextMeshProUGUI explinationText;
    public GameObject explinationPanel;
    public AudioClip buttonSound;
    public AudioClip startSound;
    public AudioClip hoverSound;
    public AudioClip mainMenuMusic;
    private float referenceAspect = 1.777f;
    private float aspectMultiplyer = 2f;

    private void Awake()
    {
        transform.position -= new Vector3(0, 0, (1 - (referenceAspect / Camera.main.aspect)) * aspectMultiplyer);
    }

    private void Start()
    {
        SoundManager.instance.LoadAndPlayMusic(mainMenuMusic);
    }

    public void HandleStartGame()
    {
        SoundManager.instance.PlayOneShotSound(startSound);
        mainMenuCam.Priority = 0;
        CameraHandler.instance.SwitchToUpCam();
        DeactivateButtons();
        StartCoroutine(Co_WaitUntilCameraTransitioned());
    }

    private void DeactivateButtons()
    {
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.enabled = false;
        }

        EventTrigger[] triggers = gameObject.GetComponentsInChildren<EventTrigger>();
        foreach (EventTrigger trigger in triggers)
        {
            trigger.enabled = false;
        }
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
        SoundManager.instance.PlayOneShotSound(hoverSound);
        mMainMenu.SetActive(false);
        mModeMenu.SetActive(true);
    }

    public void SwitchToMainMenu()
    {
        SoundManager.instance.PlayOneShotSound(buttonSound);
        mMainMenu.SetActive(true);
        mModeMenu.SetActive(false);
    }

    public void HandleHoverFreeModeButton()
    {
        OnPointerEnter();
        explinationText.text = "Lets you freely sketch without many constraints.";
        explinationPanel.SetActive(true);
    }

    public void HandleHoverOutlineModeButton()
    {
        OnPointerEnter();
        explinationText.text = "Only allows you to sketch on an outline to guide the sketch (Though the limitations might be a bit annoying)";
        explinationPanel.SetActive(true);
    }

    public void OnPointerEnter()
    {
        SoundManager.instance.PlayOneShotSound(hoverSound);
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

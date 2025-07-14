using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
    [SerializeField] private GameObject[] creditImages;
    [SerializeField] private ColorFadeEvent finalFadeEvent;
    private int creditsIndex = 0;
    private float creditDuration = 3;

    private void OnEnable()
    {
        EventSystem.OnStartCredits += BeginCredits;
    }

    private void OnDisable()
    {
        EventSystem.OnStartCredits -= BeginCredits;
    }

    private void Start()
    {
        ////For testing
        //BeginCredits();
    }

    private void BeginCredits()
    {
        EventSystem.SpawnSketch(creditImages[creditsIndex], false);
        creditsIndex++;

        StartCoroutine(Co_DelayNextCredit());
    }

    private void HandleNextCreditPage()
    {
        EventSystem.FlipNotepadPage(null);
        EventSystem.SpawnSketch(creditImages[creditsIndex], false);
        creditsIndex++;

        StartCoroutine(Co_DelayNextCredit());
    }

    private IEnumerator Co_DelayNextCredit()
    {
        yield return new WaitForSeconds(creditDuration);
        if (creditsIndex < creditImages.Length)
        {
            HandleNextCreditPage();
        }
        else
        {
            finalFadeEvent.SetIndipendentEventNotDestroyParent();
            finalFadeEvent.enabled = true;
            finalFadeEvent.Begin();
            StartCoroutine(Co_WaitUntilFadeFinished());
        }
    }

    private bool FadeDone()
    {
        return finalFadeEvent.GetIsDone();
    }

    private IEnumerator Co_WaitUntilFadeFinished()
    {
        yield return new WaitUntil(FadeDone);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

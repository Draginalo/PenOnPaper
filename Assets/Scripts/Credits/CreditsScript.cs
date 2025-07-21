using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
    [SerializeField] private CreditPackage[] creditImages;
    [SerializeField] private ColorFadeEvent finalFadeEvent;
    [SerializeField] private Material skyboxMat;
    [SerializeField] private Texture startingSkybox;
    private int creditsIndex = 0;

    [Serializable]
    private struct CreditPackage
    {
        public GameObject creditsOBJ;
        public float timeToPauseFor;
    }

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
        EventSystem.SpawnSketch(creditImages[creditsIndex].creditsOBJ, false);
        StartCoroutine(Co_DelayNextCredit(creditImages[creditsIndex].timeToPauseFor));
        creditsIndex++;
    }

    private void HandleNextCreditPage()
    {
        EventSystem.FlipNotepadPage(null);
        SpawnNextSketch nextCreditEvent = gameObject.AddComponent<SpawnNextSketch>();
        nextCreditEvent.SetSketch(creditImages[creditsIndex].creditsOBJ);
        nextCreditEvent.isIndipendent = false;
        nextCreditEvent.SetDelayTimer(0.3f);
        nextCreditEvent.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.EXECUTE_AFTER_SET_TIME);

        nextCreditEvent.SetIndipendentEventNotDestroyParent();
        nextCreditEvent.enabled = true;
        nextCreditEvent.Begin();

        StartCoroutine(Co_DelayNextCredit(creditImages[creditsIndex].timeToPauseFor));
        creditsIndex++;
    }

    private IEnumerator Co_DelayNextCredit(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (creditsIndex < creditImages.Length)
        {
            HandleNextCreditPage();
        }
        else
        {
            SoundManager.instance.FadeOutLoadedMusic(3.5f);
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
        skyboxMat.SetTexture("_Tex", startingSkybox);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

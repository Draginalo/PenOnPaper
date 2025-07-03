using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotepadManager : MonoBehaviour
{
    [SerializeField] private GameObject _NotepadPage;
    [SerializeField] private GameObject currPage;
    [SerializeField] private Animator pageAnimator;

    public GameObject CurrentPage {  get { return currPage; } }

    private void OnEnable()
    {
        EventSystem.OnFlipNotepadPage += HandleFlipPage;
        EventSystem.OnSwapEnvironments += HandleResetNotepad;
    }

    private void OnDisable()
    {
        EventSystem.OnFlipNotepadPage -= HandleFlipPage;
        EventSystem.OnSwapEnvironments -= HandleResetNotepad;
    }

    private void Start()
    {
        //HandleFlipPage();
    }

    private void HandleFlipPage(GameEvent eventData)
    {
        pageAnimator.GetComponent<AnimationHandler>().sourceGameEvent = eventData;
        pageAnimator.SetBool("StartPageFlip", true);

        GameObject newPageParent = Instantiate(_NotepadPage, transform);
        currPage = newPageParent.transform.GetChild(0).gameObject;
        pageAnimator = newPageParent.GetComponent<Animator>();
    }

    private void HandleResetNotepad(EnvironmentSwitchManager.Environments env, GameEvent gameEvent)
    {
        Destroy(currPage);

        GameObject newPageParent = Instantiate(_NotepadPage, transform);
        currPage = newPageParent.transform.GetChild(0).gameObject;
        pageAnimator = newPageParent.GetComponent<Animator>();
    }
}

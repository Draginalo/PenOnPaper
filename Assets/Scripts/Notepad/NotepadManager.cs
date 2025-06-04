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
    }

    private void OnDisable()
    {
        EventSystem.OnFlipNotepadPage -= HandleFlipPage;
    }

    private void Start()
    {
        //HandleFlipPage();
    }

    private void HandleFlipPage(GameEvent eventData)
    {
        pageAnimator.GetComponent<AnimationHandler>().sourceGameEvent = eventData;
        pageAnimator.SetBool("StartPageFlip", true);

        currPage = Instantiate(_NotepadPage, transform).transform.GetChild(0).gameObject;
        pageAnimator = currPage.GetComponent<Animator>();
    }
}

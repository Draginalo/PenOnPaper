using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotepadManager : MonoBehaviour
{
    [SerializeField] private GameObject _NotepadPage;
    [SerializeField] private GameObject currPage;
    [SerializeField] private Animator pageAnimator;
    [SerializeField] private GameObject indipendentSketchOBJ;
    [SerializeField] private GameObject _DateOBJ;
    [SerializeField] private AudioClip flipSFX;

    public GameObject CurrentPage {  get { return currPage; } }

    private void OnEnable()
    {
        EventSystem.OnFlipNotepadPage += HandleFlipPage;
        EventSystem.OnClearNotepadPage += HandleResetNotepad;
        EventSystem.OnChangeDate += HandleChangeDate;
    }

    private void OnDisable()
    {
        EventSystem.OnFlipNotepadPage -= HandleFlipPage;
        EventSystem.OnClearNotepadPage -= HandleResetNotepad;
        EventSystem.OnChangeDate -= HandleChangeDate;
    }

    private void Start()
    {
        //HandleFlipPage();
    }

    private void HandleFlipPage(GameEvent eventData)
    {
        SoundManager.instance.PlayOneShotSound(flipSFX, 1.0f, transform.position);

        pageAnimator.GetComponent<AnimationHandler>().sourceGameEvent = eventData;
        pageAnimator.SetBool("StartPageFlip", true);

        GameObject newPageParent = Instantiate(_NotepadPage, transform);
        currPage = newPageParent.transform.GetChild(0).gameObject;
        pageAnimator = newPageParent.GetComponent<Animator>();
    }

    private void HandleResetNotepad(bool clearIndipendentSketches, GameObject initiatingSketch)
    {
        Destroy(currPage);

        GameObject newPageParent = Instantiate(_NotepadPage, transform);
        currPage = newPageParent.transform.GetChild(0).gameObject;
        pageAnimator = newPageParent.GetComponent<Animator>();

        if (clearIndipendentSketches)
        {
            int childCount = indipendentSketchOBJ.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                //Check for not clearing the initiating sketch if it is assigned
                if (initiatingSketch == null || indipendentSketchOBJ.transform.GetChild(i).gameObject != initiatingSketch)
                {
                    Destroy(indipendentSketchOBJ.transform.GetChild(i).gameObject);
                }
            }
        }
    }

    private void HandleChangeDate(string newDate)
    {
        Instantiate(_DateOBJ, currPage.transform).GetComponentInChildren<TextMeshProUGUI>().text = newDate;
    }
}

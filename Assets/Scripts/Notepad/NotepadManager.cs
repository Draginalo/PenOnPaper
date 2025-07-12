using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotepadManager : MonoBehaviour
{
    [SerializeField] private GameObject _NotepadPage;
    [SerializeField] private GameObject currPage;
    [SerializeField] private Animator pageAnimator;
    [SerializeField] private GameObject indipendentSketchOBJ;

    public GameObject CurrentPage {  get { return currPage; } }

    private void OnEnable()
    {
        EventSystem.OnFlipNotepadPage += HandleFlipPage;
        EventSystem.OnClearNotepadPage += HandleResetNotepad;
    }

    private void OnDisable()
    {
        EventSystem.OnFlipNotepadPage -= HandleFlipPage;
        EventSystem.OnClearNotepadPage -= HandleResetNotepad;
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
}

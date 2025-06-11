using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingManager : MonoBehaviour
{
    public enum Sketches
    {
        TREE_SKETCH,
        MOON_SKETCH
    }


    public enum DrawingCompleteTrigger
    {
        NONE,
        LOOKING_DOWN,
        LOOKING_UP
    }

    [SerializeField] private GameObject[] sketches;
    private DrawingCompleteTrigger currTrigger = DrawingCompleteTrigger.NONE;
    private int nextSketch = 0;
    [SerializeField] private Camera _MainCamera;
    [SerializeField] private NotepadManager _NotepadManager;

    private void OnEnable()
    {
        EventSystem.OnTriggerNextSketch += HandleNextSketch;
        EventSystem.OnCameraLookChange += OnLookChanging;
    }

    private void OnDisable()
    {
        EventSystem.OnTriggerNextSketch -= HandleNextSketch;
        EventSystem.OnCameraLookChange -= OnLookChanging;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnNextSketch();
    }

    private void HandleNextSketch(DrawingCompleteTrigger trigger)
    {
        switch (trigger)
        {
            case DrawingCompleteTrigger.NONE:
                SpawnNextSketch();
                break;
            default:
                currTrigger = trigger;
                break;
        }
    }

    private void OnLookChanging(bool lookingUp)
    {
        if ((currTrigger == DrawingCompleteTrigger.LOOKING_UP && lookingUp) || (currTrigger == DrawingCompleteTrigger.LOOKING_DOWN && !lookingUp))
        {
            currTrigger = DrawingCompleteTrigger.NONE;
            SpawnNextSketch();
        }

    }

    //Add a way to trigger the OnLookChange for this function (perhaps passing a variable into the complete event)
    private void SpawnNextSketch()
    {
        /*nextSketch++;
        GameObject sketchOBJ;
        switch (nextSketch)
        {
            case 1:
                sketchOBJ = Instantiate(sketches[nextSketch], _NotepadManager.CurrentPage.transform);
                sketchOBJ.GetComponentInChildren<DrawHandler>().MainCam = _MainCamera;
                break;
            case 2:
                sketchOBJ = Instantiate(sketches[nextSketch], _NotepadManager.CurrentPage.transform);
                sketchOBJ.GetComponentInChildren<DrawHandler>().MainCam = _MainCamera;
                break;
        }*/

        if (nextSketch < sketches.Length)
        {
            GameObject sketchOBJ = Instantiate(sketches[nextSketch], _NotepadManager.CurrentPage.transform);
            sketchOBJ.GetComponentInChildren<DrawHandler>().MainCam = _MainCamera;
            nextSketch++;
        }
    }

    private void HandleNotepadChange()
    {

    }

    private void HandleWorldChange()
    {

    }
}

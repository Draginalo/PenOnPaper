using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingManager : MonoBehaviour
{
    public enum DrawingCompleteTrigger
    {
        NONE,
        LOOKING_DOWN,
        LOOKING_UP
    }

    [Serializable]
    public struct Sketches
    {
        public GameObject sketchOBJ;
        public HighlightScript objectToDraw;
    }

    [SerializeField] private Sketches[] sketchStore;
    private DrawingCompleteTrigger currTrigger = DrawingCompleteTrigger.NONE;
    private int currSketch = -1;
    [SerializeField] private Camera _MainCamera;
    [SerializeField] private NotepadManager _NotepadManager;

    private void OnEnable()
    {
        EventSystem.OnTriggerNextSketch += HandleNextSketch;
        EventSystem.OnCameraLookChange += OnLookChanging;
        EventSystem.OnSketchCompleted += TurnOffHighlight;
    }

    private void OnDisable()
    {
        EventSystem.OnTriggerNextSketch -= HandleNextSketch;
        EventSystem.OnCameraLookChange -= OnLookChanging;
        EventSystem.OnSketchCompleted -= TurnOffHighlight;
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
        /*currSketch++;
        GameObject sketchOBJ;
        switch (currSketch)
        {
            case 1:
                sketchOBJ = Instantiate(sketches[currSketch], _NotepadManager.CurrentPage.transform);
                sketchOBJ.GetComponentInChildren<DrawHandler>().MainCam = _MainCamera;
                break;
            case 2:
                sketchOBJ = Instantiate(sketches[currSketch], _NotepadManager.CurrentPage.transform);
                sketchOBJ.GetComponentInChildren<DrawHandler>().MainCam = _MainCamera;
                break;
        }*/

        if (currSketch < sketchStore.Length)
        {
            currSketch++;
            GameObject sketchOBJ = Instantiate(sketchStore[currSketch].sketchOBJ, _NotepadManager.CurrentPage.transform);
            if (sketchStore[currSketch].objectToDraw != null)
            {
                sketchStore[currSketch].objectToDraw.SetHighlightStrength(1.0f);
            }


            sketchOBJ.GetComponentInChildren<DrawHandler>().MainCam = _MainCamera;
        }
    }

    private void TurnOffHighlight()
    {
        if (sketchStore[currSketch].objectToDraw != null)
        {
            sketchStore[currSketch].objectToDraw.SetHighlightStrength(0.0f);
        }
    }
}

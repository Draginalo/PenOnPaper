using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingManager : MonoBehaviour
{
    public enum Sketch
    {
        TREE_SKETCH,
        MOON_SKETCH
    }

    public enum GameEvent
    {
        FLIP_PAGE
    }

    [SerializeField] private GameObject[] sketches;
    [SerializeField] private GameEvent gameEvents;
    private bool currSketchCompleted = false;
    private int nextSketch = 0;
    private int nextGameEvent = 0;
    [SerializeField] private Camera _MainCamera;
    [SerializeField] private NotepadManager _NotepadManager;

    private void OnEnable()
    {
        EventSystem.OnSketchComplete += HandleSketchCompleted;
        EventSystem.OnCameraLookChange += OnLookChanging;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject sketchOBJ = Instantiate(sketches[nextSketch], _NotepadManager.CurrentPage.transform);
        sketchOBJ.GetComponentInChildren<DrawHandler>().MainCam = _MainCamera;
    }

    private void OnLookChanging(bool lookingUp)
    {
        if (lookingUp && currSketchCompleted)
        {
            HandleSketchCompleted();
        }
    }

    private void HandleSketchCompleted()
    {
        nextSketch++;
        switch (nextSketch)
        {
            case 1:
                GameObject sketchOBJ = Instantiate(sketches[nextSketch], _NotepadManager.CurrentPage.transform);
                sketchOBJ.GetComponentInChildren<DrawHandler>().MainCam = _MainCamera;
                break;
        }
    }

    private void HandleNotepadChange()
    {

    }

    private void HandleWorldChange()
    {

    }
}

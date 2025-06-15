using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private int currSketch = -1;
    [SerializeField] private Camera _MainCamera;
    [SerializeField] private NotepadManager _NotepadManager;
    [SerializeField] private List<Sketches> thingsToDraw;
    [SerializeField] private List<GameEventChain> currGameEventChains;
    [SerializeField] private GameObject gameEventChainHolder;
    private float _RaycastDistence = 100.0f;
    private bool ThingsToDrawActivated = false;

    private void OnEnable()
    {
        EventSystem.OnTriggerNextSketch += ActivateThingsToDraw;
        EventSystem.OnTriggerNextEventChain += HandleNextEventChainTriger;
        EventSystem.OnLoadEventChains += LoadGameEventChains;
        //EventSystem.OnSketchCompleted += TurnOffHighlight;
    }

    private void OnDisable()
    {
        EventSystem.OnTriggerNextSketch -= ActivateThingsToDraw;
        EventSystem.OnTriggerNextEventChain -= HandleNextEventChainTriger;
        EventSystem.OnLoadEventChains -= LoadGameEventChains;
        //EventSystem.OnSketchCompleted -= TurnOffHighlight;
    }

    // Start is called before the first frame update
    void Start()
    {
        ActivateThingsToDraw();
    }

    private void LoadThingsToDraw(List<Sketches> thingsToDraw)
    {
        this.thingsToDraw = thingsToDraw;
    }

    private void LoadGameEventChains(List<GameEventChain> gameEventChains, GameObject chainsParent)
    {
        if (currGameEventChains.Count > 0)
        {
            for (int i = 0; i < currGameEventChains.Count; i++)
            {
                currGameEventChains[i].CleanupChain();
            }
        }

        currGameEventChains.Clear();

        currGameEventChains = gameEventChains;

        chainsParent.transform.SetParent(transform);
    }

    private void ActivateThingsToDraw()
    {
        foreach (Sketches thingToDraw in thingsToDraw)
        {
            if (thingToDraw.objectToDraw != null)
            {
                thingToDraw.objectToDraw.SetHighlightStrength(1.0f);
            }
        }

        ThingsToDrawActivated = true;
    }

    private void DeactivateThingsToDraw()
    {
        foreach (Sketches thingToDraw in thingsToDraw)
        {
            if (thingToDraw.objectToDraw != null)
            {
                thingToDraw.objectToDraw.SetHighlightStrength(0.0f);
            }
        }

        ThingsToDrawActivated = false;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed && ThingsToDrawActivated)
        {
            Ray drawRay = _MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(drawRay, out hit, _RaycastDistence))
            {
                Sketches possibleSketch = CheckForClickedObject(hit);
                if (possibleSketch.sketchOBJ != null)
                {
                    DeactivateThingsToDraw();

                    GameObject sketchOBJ = Instantiate(possibleSketch.sketchOBJ, _NotepadManager.CurrentPage.transform);
                    sketchOBJ.GetComponentInChildren<DrawHandler>().MainCam = _MainCamera;

                    Destroy(possibleSketch.objectToDraw);
                    thingsToDraw.Remove(possibleSketch);
                }

            }
        }
    }

    private Sketches CheckForClickedObject(RaycastHit hitOBJ)
    {
        HighlightScript thingToDrawScript = hitOBJ.collider.GetComponent<HighlightScript>();

        if (thingToDrawScript != null) {
            foreach (Sketches thingsToDraw in thingsToDraw)
            {
                if (thingsToDraw.objectToDraw == thingToDrawScript)
                {
                    return thingsToDraw;
                }
            }
        }

        return new Sketches();
    }

    private void HandleNextEventChainTriger()
    {
        if (currGameEventChains != null && currGameEventChains.Count > 0)
        {
            GameEventManager.instance.LoadAndExecuteEventChain(currGameEventChains[0]);
            currGameEventChains.RemoveAt(0);
        }
    }

    //Add a way to trigger the OnLookChange for this function (perhaps passing a variable into the complete event)
    //private void SpawnNextSketch()
    //{
    //    /*currSketch++;
    //    GameObject sketchOBJ;
    //    switch (currSketch)
    //    {
    //        case 1:
    //            sketchOBJ = Instantiate(sketches[currSketch], _NotepadManager.CurrentPage.transform);
    //            sketchOBJ.GetComponentInChildren<DrawHandler>().MainCam = _MainCamera;
    //            break;
    //        case 2:
    //            sketchOBJ = Instantiate(sketches[currSketch], _NotepadManager.CurrentPage.transform);
    //            sketchOBJ.GetComponentInChildren<DrawHandler>().MainCam = _MainCamera;
    //            break;
    //    }*/

    //    if (currSketch + 1 < sketchStore.Length)
    //    {
    //        currSketch++;
    //        GameObject sketchOBJ = Instantiate(sketchStore[currSketch].sketchOBJ, _NotepadManager.CurrentPage.transform);
    //        if (sketchStore[currSketch].objectToDraw != null)
    //        {
    //            sketchStore[currSketch].objectToDraw.SetHighlightStrength(1.0f);
    //        }


    //        sketchOBJ.GetComponentInChildren<DrawHandler>().MainCam = _MainCamera;
    //    }
    //}

    //private void TurnOffHighlight()
    //{
    //    if (sketchStore[currSketch].objectToDraw != null)
    //    {
    //        sketchStore[currSketch].objectToDraw.SetHighlightStrength(0.0f);
    //    }
    //}
}

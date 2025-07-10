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

    //[Serializable]
    //public struct Sketches
    //{
    //    public GameObject sketchOBJ;
    //    public HighlightScript objectToDraw;
    //}

    //[SerializeField] private GameObject[] sketchStore;
    //private int currSketch = -1;
    [SerializeField] private Camera _MainCamera;
    [SerializeField] private NotepadManager _NotepadManager;
    [SerializeField] private List<GameObject> sketchesToDraw;
    [SerializeField] private List<GameEventChain> currGameEventChains;
    [SerializeField] private GameObject gameEventChainHolder;

    [SerializeField] private Vector2[] sketchPositions;
    private int sketchPosIndex = 0;

    [SerializeField] private GameObject indipendentSketchOBJ;

    private float _RaycastDistence = 1000.0f;
    private bool ThingsToDrawActivated = false;

    private void OnEnable()
    {
        EventSystem.OnActivateSketchChoosing += ActivateThingsToDraw;
        EventSystem.OnDeActivateSketchChoosing += DeactivateThingsToDraw;
        EventSystem.OnTriggerNextEventChain += HandleNextEventChainTriger;
        EventSystem.OnLoadEventChains += LoadGameEventChains;
        EventSystem.OnLoadSketchesToDraw += LoadSketchesToDraw;
        EventSystem.OnFlipNotepadPage += HandleResetSketchPos;
        EventSystem.OnClearNotepadPage += HandleResetNotepadPos;
        EventSystem.OnSpawnSketch += HandleSpawnNextSketch;
    }

    private void OnDisable()
    {
        EventSystem.OnActivateSketchChoosing -= ActivateThingsToDraw;
        EventSystem.OnDeActivateSketchChoosing -= DeactivateThingsToDraw;
        EventSystem.OnTriggerNextEventChain -= HandleNextEventChainTriger;
        EventSystem.OnLoadEventChains -= LoadGameEventChains;
        EventSystem.OnLoadSketchesToDraw -= LoadSketchesToDraw;
        EventSystem.OnFlipNotepadPage -= HandleResetSketchPos;
        EventSystem.OnClearNotepadPage -= HandleResetNotepadPos;
        EventSystem.OnSpawnSketch -= HandleSpawnNextSketch;
    }

    private static Dictionary<DrawHandler, HighlightScript> sketchToObjectDictionary = new();

    public static void AddObjectToDraw(DrawHandler sketchScript, HighlightScript thingToDrawScript)
    {
        if (!sketchToObjectDictionary.ContainsKey(sketchScript))
        {
            sketchToObjectDictionary.Add(sketchScript, thingToDrawScript);
        }
    }

    public static void RemoveObjectToDraw(DrawHandler sketchScript)
    {
        sketchToObjectDictionary.Remove(sketchScript);
    }

    public static HighlightScript CheckForConnectingObject(DrawHandler sketchScript)
    {
        if (sketchToObjectDictionary.ContainsKey(sketchScript))
        {
            return sketchToObjectDictionary[sketchScript];
        }

        return null;
    }

    private void LoadSketchesToDraw(List<GameObject> sketchesToDraw)
    {
        this.sketchesToDraw.Clear();
        this.sketchesToDraw = sketchesToDraw;
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

        HandleNextEventChainTriger();
    }

    private void ActivateThingsToDraw()
    {
        //Handles if all the current spots to draw sketches in the notebook are filled up
        bool spotsOccupied = HandleDrawingSpotsOccupied();

        if (!spotsOccupied)
        {
            foreach (GameObject thingToDraw in sketchesToDraw)
            {
                HighlightScript possibleScript = CheckForConnectingObject(thingToDraw.GetComponentInChildren<DrawHandler>());
                if (possibleScript != null)
                {
                    possibleScript.SetHighlightStrength(1.0f);
                }
            }

            ThingsToDrawActivated = true;
        }
    }

    private void DeactivateThingsToDraw()
    {
        foreach (GameObject thingToDraw in sketchesToDraw)
        {
            HighlightScript possibleScript = CheckForConnectingObject(thingToDraw.GetComponentInChildren<DrawHandler>());
            if (possibleScript != null)
            {
                possibleScript.SetHighlightStrength(0.0f);
            }
        }

        ThingsToDrawActivated = false;
    }

    private bool HandleDrawingSpotsOccupied()
    {
        if (sketchPosIndex == sketchPositions.Length)
        {
            FlipPageEvent flipPageEvent = gameObject.AddComponent<FlipPageEvent>();
            ActivateSketchesToChooseEvent spawnNextSketchEvent = gameObject.AddComponent<ActivateSketchesToChooseEvent>();
            spawnNextSketchEvent.SetEventTrigger(DrawingCompleteTrigger.LOOKING_UP);

            ////Maybe add a way to have the activate sketch choosing be without looking down and up 
            /// again and just fade in instead of snap in
            /// 
            GameEventChain newEventsChain = new GameEventChain();
            newEventsChain.SetEvents(new List<GameEvent> { flipPageEvent, spawnNextSketchEvent });
            GameEventManager.instance.LoadAndExecuteEventChain(newEventsChain);

            sketchPosIndex = 0;
            return true;
        }

        return false;
    }

    private void HandleResetSketchPos(GameEvent gameEvent = null)
    {
        sketchPosIndex = 0;
    }

    private void HandleResetNotepadPos(bool clearIndipendentSketches)
    {
        HandleResetSketchPos();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed && ThingsToDrawActivated)
        {
            HandleSelectThingToDraw();
        }
    }

    private void HandleSelectThingToDraw()
    {
        Ray drawRay = _MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(drawRay, out hit, _RaycastDistence))
        {
            GameObject possibleSketch = CheckForClickedObject(hit);
            if (possibleSketch != null)
            {
                DeactivateThingsToDraw();

                HandleSpawnNextSketch(possibleSketch, false);
            }
        }
    }

    private void HandleSpawnNextSketch(GameObject nextSketch, bool isIndipendent)
    {
        GameObject sketchOBJ = null;
        if (!isIndipendent)
        {
            if (sketchPosIndex < sketchPositions.Length)
            {
                sketchOBJ = Instantiate(nextSketch, _NotepadManager.CurrentPage.transform);
                sketchOBJ.transform.localPosition = new Vector3(sketchPositions[sketchPosIndex].x, sketchOBJ.transform.localPosition.y, sketchPositions[sketchPosIndex].y);
                sketchOBJ.GetComponentInChildren<DrawHandler>().MainCam = _MainCamera;
                sketchPosIndex++;

                HighlightScript possibleScript = CheckForConnectingObject(nextSketch.GetComponentInChildren<DrawHandler>());
                if (possibleScript != null)
                {
                    //DrawHandler.RemoveObjectToDraw(possibleSketch.GetComponentInChildren<DrawHandler>());
                    Destroy(possibleScript);
                }

                //To allow for adding of loaded sketches and new non-loaded sketches
                if (sketchesToDraw.Contains(nextSketch))
                {
                    sketchesToDraw.Remove(nextSketch);
                }
            }

            return;
        }

        sketchOBJ = Instantiate(nextSketch, indipendentSketchOBJ.transform);
        sketchOBJ.GetComponentInChildren<DrawHandler>().MainCam = _MainCamera;
    }

    private GameObject CheckForClickedObject(RaycastHit hitOBJ)
    {
        HighlightScript thingToDrawScript = hitOBJ.collider.GetComponent<HighlightScript>();

        if (thingToDrawScript != null) {
            foreach (GameObject sketchToDraw in sketchesToDraw)
            {
                HighlightScript possibleConnectingScript = CheckForConnectingObject(sketchToDraw.GetComponentInChildren<DrawHandler>());
                if (possibleConnectingScript == thingToDrawScript)
                {
                    return sketchToDraw;
                }
            }

            ClickableObject possibleClickableScript = thingToDrawScript.gameObject.GetComponent<ClickableObject>();
            if (possibleClickableScript != null)
            {
                possibleClickableScript.Clicked();
            }
        }

        return null;
    }

    private void HandleNextEventChainTriger()
    {
        if (currGameEventChains != null && currGameEventChains.Count > 0)
        {
            GameEventManager.instance.LoadAndExecuteEventChain(currGameEventChains[0]);
            currGameEventChains.RemoveAt(0);
        }
    }

    private void OnDestroy()
    {
        sketchToObjectDictionary.Clear();
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

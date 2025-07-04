using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static DrawingManager;

public static class EventSystem
{
    // Camera Events
    public static event UnityAction<bool> OnCameraLookChange;
    public static void CameraLookChange(bool lookingUp) => OnCameraLookChange?.Invoke(lookingUp);

    // Sketch Events
    public static event UnityAction OnActivateSketchChoosing;
    public static void ActivateSketchChoosing() => OnActivateSketchChoosing?.Invoke();

    public static event UnityAction OnSketchHalfComplete;
    public static void SketchHalfComplete() => OnSketchHalfComplete?.Invoke();

    public static event UnityAction OnSketchCompleted;
    public static void SketchCompleted() => OnSketchCompleted?.Invoke();

    //Notepad events
    public static event UnityAction<GameEvent> OnFlipNotepadPage;
    public static void FlipNotepadPage(GameEvent eventData) => OnFlipNotepadPage?.Invoke(eventData);

    //Environment events
    public static event UnityAction<EnvironmentSwitchManager.Environments, GameEvent> OnSwapEnvironments;
    public static void SwapEnvironment(EnvironmentSwitchManager.Environments newEnvironment, GameEvent gameEvent) => OnSwapEnvironments?.Invoke(newEnvironment, gameEvent);

    public static event UnityAction OnSpawnDoctor;
    public static void SpawnDoctor() => OnSpawnDoctor?.Invoke();

    //Possible combine this with the on sketch complete event with a bool perameter to only have one event
    public static event UnityAction OnTriggerNextEventChain;
    public static void TriggerNextEventChain() => OnTriggerNextEventChain?.Invoke();

    public static event UnityAction<List<GameEventChain>, GameObject> OnLoadEventChains;
    public static void LoadEventChains(List<GameEventChain> gameEventChains, GameObject chainsParent) => OnLoadEventChains?.Invoke(gameEventChains, chainsParent);

    public static event UnityAction<List<GameObject>> OnLoadSketchesToDraw;
    public static void LoadSketchesToDraw(List<GameObject> sketchesToDraw) => OnLoadSketchesToDraw?.Invoke(sketchesToDraw);
}

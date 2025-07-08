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

    public static event UnityAction OnDeActivateSketchChoosing;
    public static void DeActivateSketchChoosing() => OnDeActivateSketchChoosing?.Invoke();

    public static event UnityAction OnSketchHalfComplete;
    public static void SketchHalfComplete() => OnSketchHalfComplete?.Invoke();

    public static event UnityAction OnSketchCompleted;
    public static void SketchCompleted() => OnSketchCompleted?.Invoke();

    public static event UnityAction<GameObject> OnSpawnSketch;
    public static void SpawnSketch(GameObject nextSketch) => OnSpawnSketch?.Invoke(nextSketch);

    //Notepad events
    public static event UnityAction<GameEvent> OnFlipNotepadPage;
    public static void FlipNotepadPage(GameEvent eventData) => OnFlipNotepadPage?.Invoke(eventData);

    public static event UnityAction OnClearNotepadPage;
    public static void ClearNotepadPage() => OnClearNotepadPage?.Invoke();

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

    public static event UnityAction<bool> OnOpenHospitalDoor;
    public static void OpenHospitalDoor(bool slowOpen) => OnOpenHospitalDoor?.Invoke(slowOpen);

    public static event UnityAction<AnimationCurve, float, float, float> OnSetLightIntensity;
    public static void SetLightIntensity(AnimationCurve newCurve, float maxIntensity, float newRange, float evaluationSpeed) => OnSetLightIntensity?.Invoke(newCurve, maxIntensity, newRange, evaluationSpeed);

    public static event UnityAction OnActivateHospitalVoidEnv;
    public static void ActivateHospitalVoidEnv() => OnActivateHospitalVoidEnv?.Invoke();

    public static event UnityAction OnOpenWindow;
    public static void OpenWindow() => OnOpenWindow?.Invoke();

    public static event UnityAction OnStartFinalConfrontation;
    public static void StartFinalConfrontation() => OnStartFinalConfrontation?.Invoke();

    public static event UnityAction OnStopOpening;
    public static void StopOpening() => OnStopOpening?.Invoke();

    public static event UnityAction OnRepairLight;
    public static void RepairLight() => OnRepairLight?.Invoke();
}

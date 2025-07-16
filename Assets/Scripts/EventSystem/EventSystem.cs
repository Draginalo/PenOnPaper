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

    public static event UnityAction<GameObject, bool> OnSpawnSketch;
    public static void SpawnSketch(GameObject nextSketch, bool isIndipendent) => OnSpawnSketch?.Invoke(nextSketch, isIndipendent);

    public static event UnityAction<DrawHandler> OnDeactivateAllOtherSketches;
    public static void DeactivateAllOtherSketches(DrawHandler initiator) => OnDeactivateAllOtherSketches?.Invoke(initiator);

    public static event UnityAction OnChangeSketch;
    public static void ChangeSketch() => OnChangeSketch?.Invoke();

    //Notepad events
    public static event UnityAction<GameEvent> OnFlipNotepadPage;
    public static void FlipNotepadPage(GameEvent eventData) => OnFlipNotepadPage?.Invoke(eventData);

    public static event UnityAction<bool, GameObject> OnClearNotepadPage;
    public static void ClearNotepadPage(bool clearIndipendentSketches, GameObject initiatingSketch) => OnClearNotepadPage?.Invoke(clearIndipendentSketches, initiatingSketch);

    public static event UnityAction<string> OnChangeDate;
    public static void ChangeDate(string newDate) => OnChangeDate?.Invoke(newDate);

    //Environment events
    public static event UnityAction<EnvironmentSwitchManager.Environments, GameEvent> OnSwapEnvironments;
    public static void SwapEnvironment(EnvironmentSwitchManager.Environments newEnvironment, GameEvent gameEvent) => OnSwapEnvironments?.Invoke(newEnvironment, gameEvent);

    public static event UnityAction OnSpawnDoctor;
    public static void SpawnDoctor() => OnSpawnDoctor?.Invoke();

    public static event UnityAction<bool> OnOpenHospitalDoor;
    public static void OpenHospitalDoor(bool slowOpen) => OnOpenHospitalDoor?.Invoke(slowOpen);

    public static event UnityAction<SetLightIntesityEvent> OnSetLightIntensity;
    public static void SetLightIntensity(SetLightIntesityEvent gameEvent) => OnSetLightIntensity?.Invoke(gameEvent);

    public static event UnityAction OnToggleActivateHospitalVoidEnv;
    public static void ToggleActivateHospitalVoidEnv() => OnToggleActivateHospitalVoidEnv?.Invoke();

    public static event UnityAction OnOpenWindow;
    public static void OpenWindow() => OnOpenWindow?.Invoke();

    public static event UnityAction OnTurnOffHospitalSign;
    public static void TurnOffHospitalSign() => OnTurnOffHospitalSign?.Invoke();

    //Possible combine this with the on sketch complete event with a bool perameter to only have one event
    public static event UnityAction OnTriggerNextEventChain;
    public static void TriggerNextEventChain() => OnTriggerNextEventChain?.Invoke();

    public static event UnityAction<List<GameEventChain>, GameObject> OnLoadEventChains;
    public static void LoadEventChains(List<GameEventChain> gameEventChains, GameObject chainsParent) => OnLoadEventChains?.Invoke(gameEventChains, chainsParent);

    public static event UnityAction<List<GameObject>> OnLoadSketchesToDraw;
    public static void LoadSketchesToDraw(List<GameObject> sketchesToDraw) => OnLoadSketchesToDraw?.Invoke(sketchesToDraw);

    public static event UnityAction<bool> OnStartFinalConfrontation;
    public static void StartFinalConfrontation(bool intro) => OnStartFinalConfrontation?.Invoke(intro);

    public static event UnityAction OnFinishFinalConfrontation;
    public static void FinishFinalConfrontation() => OnFinishFinalConfrontation?.Invoke();

    public static event UnityAction<FinalConfrontationManager.Issues> OnFixConfrontationIssue;
    public static void FixConfrontationIssue(FinalConfrontationManager.Issues issueFixed) => OnFixConfrontationIssue?.Invoke(issueFixed);

    public static event UnityAction OnSliceDoctor;
    public static void SliceDoctor() => OnSliceDoctor?.Invoke();

    public static event UnityAction OnStartCredits;
    public static void StartCredits() => OnStartCredits?.Invoke();
}

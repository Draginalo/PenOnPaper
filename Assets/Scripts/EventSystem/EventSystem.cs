using UnityEngine;
using UnityEngine.Events;

public static class EventSystem
{
    // Camera Events
    public static event UnityAction<bool> OnCameraLookChange;
    public static void CameraLookChange(bool lookingUp) => OnCameraLookChange?.Invoke(lookingUp);

    // Sketch Events
    public static event UnityAction<GameObject> OnSketchComplete;
    public static void SketchComplete(GameObject sketch) => OnSketchComplete?.Invoke(sketch);

    public static event UnityAction OnSketchHalfComplete;
    public static void SketchHalfComplete() => OnSketchHalfComplete?.Invoke();

    //Notepad events
    public static event UnityAction<GameEvent> OnFlipNotepadPage;
    public static void FlipNotepadPage(GameEvent eventData) => OnFlipNotepadPage?.Invoke(eventData);

    //Game event events
    public static event UnityAction OnGameEventCompleted;
    public static void GameEventCompleted() => OnGameEventCompleted?.Invoke();
}

using UnityEngine;
using UnityEngine.Events;
using static DrawingManager;

public static class EventSystem
{
    // Camera Events
    public static event UnityAction<bool> OnCameraLookChange;
    public static void CameraLookChange(bool lookingUp) => OnCameraLookChange?.Invoke(lookingUp);

    // Sketch Events
    public static event UnityAction<DrawingCompleteTrigger> OnTriggerNextSketch;
    public static void TriggerNextSketch(DrawingCompleteTrigger trigger) => OnTriggerNextSketch?.Invoke(trigger);

    public static event UnityAction OnSketchHalfComplete;
    public static void SketchHalfComplete() => OnSketchHalfComplete?.Invoke();

    public static event UnityAction OnSketchCompleted;
    public static void SketchCompleted() => OnSketchCompleted?.Invoke();

    //Notepad events
    public static event UnityAction<GameEvent> OnFlipNotepadPage;
    public static void FlipNotepadPage(GameEvent eventData) => OnFlipNotepadPage?.Invoke(eventData);

    //Game event events
    //public static event UnityAction OnGameEventCompleted;
    //public static void GameEventCompleted() => OnGameEventCompleted?.Invoke();
}

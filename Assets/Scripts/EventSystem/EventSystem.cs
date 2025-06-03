using UnityEngine.Events;

public static class EventSystem
{
    // Camera Events
    public static event UnityAction<bool> OnCameraLookChange;
    public static void SketchComplete(bool lookingUp) => OnCameraLookChange?.Invoke(lookingUp);

    // Sketch Events
    public static event UnityAction OnSketchComplete;
    public static void SketchComplete() => OnSketchComplete?.Invoke();

    public static event UnityAction OnSketchHalfComplete;
    public static void SketchHalfComplete() => OnSketchHalfComplete?.Invoke();

}

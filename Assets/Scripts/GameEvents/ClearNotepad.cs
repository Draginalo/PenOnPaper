using UnityEngine;

public class ClearNotepad : GameEvent
{
    public override void Execute()
    {
        base.Execute();

        GameEventCompleted(this);
    }

    //This is because both completing the event and clearing the notepad can destroy this script before the both lines excecute
    private void OnDestroy()
    {
        EventSystem.ClearNotepadPage();
    }
}

using UnityEngine;

public class ClearNotepad : GameEvent
{
    public bool clearIndipendentSketches = false;
    public bool doNotClearSelf = false;
    private bool mHasBeenExcecuted = false;
    private bool mHasCompleted = false;
    public override void Execute()
    {
        base.Execute();
        mHasBeenExcecuted = true;

        GameObject parent = null;
        if (doNotClearSelf)
        {
            if (transform.parent != null)
            {
                parent = transform.parent.gameObject;
            }
            else
            {
                parent = gameObject;
            }
        }

        EventSystem.ClearNotepadPage(clearIndipendentSketches, parent);
        GameEventCompleted(this);
        mHasCompleted = true;
    }

    //This is because both completing the event and clearing the notepad can destroy this script
    //before the both lines excecute (meaning the GameEventCompleted function would not be called before the script is destroyed)
    private void OnDestroy()
    {
        ////THIS HAS BUGS IF ON A SKETCH THAT GETS DESTROYED. FIX
        if (!mHasCompleted && mHasBeenExcecuted)
        {
            GameEventCompleted(this);
        }
    }
}

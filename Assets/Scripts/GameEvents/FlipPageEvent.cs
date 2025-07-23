using System.Collections;
using UnityEngine;

public class FlipPageEvent : GameEvent
{
    public override void Execute()
    {
        base.Execute();
        EventSystem.FlipNotepadPage(this);
        StartCoroutine(Co_DelayDestroy());
    }

    //To destroy earlier than when the animation finished 
    private IEnumerator Co_DelayDestroy()
    {
        yield return new WaitForSeconds(1f);
        GameEventCompleted(this);
    }
}

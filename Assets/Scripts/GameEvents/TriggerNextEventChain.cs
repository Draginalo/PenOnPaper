using System.Collections;
using UnityEngine;

public class TriggerNextEventChain : GameEvent
{
    [SerializeField] private float delay = 0.0f;

   public override void Execute()
    {
        base.Execute();
        StartCoroutine(Co_Delay(delay));
    }

    private IEnumerator Co_Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EventSystem.TriggerNextEventChain();
        GameEventCompleted(this);
    }
}

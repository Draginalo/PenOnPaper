using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSketchesToChooseEvent : GameEvent
{
    //[SerializeField] private float spawnDelayTime = 0.15f;

    public override void Begin()
    {
        base.Begin();

        if ((eventTrigger == DrawingManager.DrawingCompleteTrigger.LOOKING_DOWN) || CameraHandler.instance.CurrentlyLookingDown)
        {
            Execute();
        }
    }

    public override void Execute()
    {
        base.Execute();

        EventSystem.ActivateSketchChoosing();
        GameEventCompleted(this);
        //StartCoroutine(Co_DelaySpawn());
    }

    //private IEnumerator Co_DelaySpawn()
    //{
    //    yield return new WaitForSeconds(spawnDelayTime);
    //    EventSystem.TriggerNextSketch();
    //    GameEventCompleted(this);
    //}
}

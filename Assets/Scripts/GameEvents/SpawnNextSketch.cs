using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNextSketch : GameEvent
{
    [SerializeField] private float spawnDelayTime = 0.15f;

    public override void Execute()
    {
        base.Execute();
        StartCoroutine(Co_DelaySpawn());
    }

    private IEnumerator Co_DelaySpawn()
    {
        yield return new WaitForSeconds(spawnDelayTime);
        EventSystem.TriggerNextSketch();
        GameEventCompleted(this);
    }
}

using System.Collections;
using UnityEngine;

public class SpawnNextSketch : GameEvent
{
    [SerializeField] private GameObject m_NextSketchToSpawn;

   public override void Execute()
    {
        base.Execute();
        EventSystem.SpawnSketch(m_NextSketchToSpawn);
        GameEventCompleted(this);
    }
}

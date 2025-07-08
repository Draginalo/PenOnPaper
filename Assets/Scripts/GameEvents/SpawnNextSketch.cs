using System.Collections;
using UnityEngine;

public class SpawnNextSketch : GameEvent
{
    [SerializeField] private GameObject m_NextSketchToSpawn;
    public bool isIndipendent = false;

    public void SetSketch(GameObject sketch)
    {
        m_NextSketchToSpawn = sketch;
    }

   public override void Execute()
    {
        base.Execute();
        EventSystem.SpawnSketch(m_NextSketchToSpawn, isIndipendent);
        GameEventCompleted(this);
    }
}

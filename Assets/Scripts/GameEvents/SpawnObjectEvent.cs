using System.Collections;
using UnityEngine;

public class SpawnObjectEvent : GameEvent
{
    [SerializeField] private GameObject m_ObjectToSpawn;

   public override void Execute()
    {
        base.Execute();
        Instantiate(m_ObjectToSpawn);
        GameEventCompleted(this);
    }
}

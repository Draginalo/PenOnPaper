using UnityEngine;

public class SpawnDeerEvent : GameEvent
{
    [SerializeField] private GameObject m_DeerToSpawn;

    public override void Execute()
    {
        base.Execute();
        Instantiate(m_DeerToSpawn);

        GameEventCompleted(this);
    }
}

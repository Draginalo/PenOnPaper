using UnityEngine;

public class SpawnBirdEvent : GameEvent
{
    [SerializeField] private GameObject m_BirdToSpawn;

    public override void Cleanup(bool destroyParentComponent)
    {
        //base.Cleanup(destroyParentComponent);
    }

    public override void Execute()
    {
        base.Execute();
        Instantiate(m_BirdToSpawn);

        GameEventCompleted(this);
    }
}

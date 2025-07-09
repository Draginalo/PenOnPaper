using System.Collections;
using UnityEngine;

public class SpawnObjectEvent : GameEvent
{
    [SerializeField] private GameObject m_ObjectToSpawn;
    [SerializeField] private bool mDeactivateParentAnimation;
    [SerializeField] private bool useAnimationToTriggerCompletion;

    public override void Execute()
    {
        base.Execute();
        GameObject obj = Instantiate(m_ObjectToSpawn);

        if (mDeactivateParentAnimation)
        {
            obj.GetComponent<Animator>().enabled = false;
        }

        if (!useAnimationToTriggerCompletion)
        {
            GameEventCompleted(this);
            return;
        }

        obj.GetComponent<AnimationHandler>().sourceGameEvent = this;
    }
}

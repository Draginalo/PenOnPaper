using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public GameEvent sourceGameEvent;

    private void DestroyAnimationObject()
    {
        if (sourceGameEvent != null)
        {
            sourceGameEvent.GameEventCompleted();
        }

        Destroy(gameObject);
    }
}

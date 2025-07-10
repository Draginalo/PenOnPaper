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
            sourceGameEvent.GameEventCompleted(sourceGameEvent);
        }

        Destroy(gameObject);
    }

    private void DestroyAnimationComponent()
    {
        if (sourceGameEvent != null)
        {
            sourceGameEvent.GameEventCompleted(sourceGameEvent);
        }

        Destroy(gameObject.GetComponent<Animator>());
        Destroy(this);
    }

    private void SetClickableActive()
    {
        HighlightScript possibleScript = gameObject.GetComponent<HighlightScript>();
        if (possibleScript != null)
        {
            possibleScript.SetHighlightStrength(1.0f);
        }

        EventSystem.ActivateSketchChoosing();
    }

    private void SetClickableDeActive()
    {
        HighlightScript possibleScript = gameObject.GetComponent<HighlightScript>();
        if (possibleScript != null)
        {
            possibleScript.SetHighlightStrength(0.0f);
        }

        EventSystem.DeActivateSketchChoosing();
    }
}

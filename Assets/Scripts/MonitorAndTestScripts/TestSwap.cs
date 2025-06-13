using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DrawingManager;

public class TestSwap : MonoBehaviour
{
    public GameObject sceneToDisable;
    public float delay = 0.1f;
    private DrawingCompleteTrigger currTrigger = DrawingCompleteTrigger.LOOKING_DOWN;

    private void OnEnable()
    {
        EventSystem.OnCameraLookChange += OnLookChanging;
    }

    private void OnDisable()
    {
        EventSystem.OnCameraLookChange -= OnLookChanging;
    }

    private void OnLookChanging(bool lookingUp)
    {
        if ((currTrigger == DrawingCompleteTrigger.LOOKING_UP && lookingUp) || (currTrigger == DrawingCompleteTrigger.LOOKING_DOWN && !lookingUp))
        {
            currTrigger = DrawingCompleteTrigger.NONE;
            StartCoroutine(Co_DelaySwap());
        }
    }

    private IEnumerator Co_DelaySwap()
    {
        yield return new WaitForSeconds(delay);
        sceneToDisable.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorScript : MonoBehaviour
{
    private void OnEnable()
    {
        EventSystem.OnSwapEnvironments += DestroyDoc;
        EventSystem.OnStopOpening += DestroyDoc;
    }

    private void OnDisable()
    {
        EventSystem.OnSwapEnvironments -= DestroyDoc;
        EventSystem.OnStopOpening -= DestroyDoc;
    }

    private void DestroyDoc(EnvironmentSwitchManager.Environments env, GameEvent gameEvent)
    {
        Destroy(gameObject);
    }

    private void DestroyDoc()
    {
        Destroy(gameObject);
    }

    private void StealHeart()
    {
        GameObject heart = GameObject.FindGameObjectWithTag("Heart");
        if (heart != null)
        {
            heart.transform.SetParent(transform);
        }
    }
}

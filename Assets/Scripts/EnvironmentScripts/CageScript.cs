using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour
{
    private void OnEnable()
    {
        EventSystem.OnSwapEnvironments += HandleDestroy;
    }

    private void OnDisable()
    {
        EventSystem.OnSwapEnvironments -= HandleDestroy;
    }

    private void HandleDestroy(EnvironmentSwitchManager.Environments env, GameEvent gameEvent)
    {
        Destroy(gameObject);
    }
}

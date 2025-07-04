using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerScript : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;

    private void OnEnable()
    {
        EventSystem.OnSpawnDoctor += HandleDestroy;
    }

    private void OnDisable()
    {
        EventSystem.OnSpawnDoctor -= HandleDestroy;
    }

    private void HandleSetActivateThingsToDraw()
    {
        EventSystem.ActivateSketchChoosing();
    }

    private void HandleDestroy()
    {
        Destroy(gameObject);
    }
}

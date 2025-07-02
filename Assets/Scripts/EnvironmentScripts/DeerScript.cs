using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerScript : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;

    private void HandleSetActivateThingsToDraw()
    {
        EventSystem.ActivateSketchChoosing();
    }
}

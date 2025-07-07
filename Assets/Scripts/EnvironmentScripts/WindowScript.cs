using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowScript : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;

    private void OnEnable()
    {
        EventSystem.OnOpenWindow += OpenWindow;
    }

    private void OnDisable()
    {
        EventSystem.OnOpenWindow -= OpenWindow;
    }

    private void OpenWindow()
    {
        m_Animator.enabled = true;
    }

    private void ResetWindow()
    {
        m_Animator.enabled = false;
    }
}

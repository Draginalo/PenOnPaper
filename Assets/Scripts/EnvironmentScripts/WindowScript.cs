using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FinalConfrontationManager;

public class WindowScript : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;

    private void OnEnable()
    {
        EventSystem.OnOpenWindow += OpenWindow;
        EventSystem.OnFixConfrontationIssue += ResetWindow;
    }

    private void OnDisable()
    {
        EventSystem.OnOpenWindow -= OpenWindow;
        EventSystem.OnFixConfrontationIssue -= ResetWindow;
    }

    private void OpenWindow()
    {
        m_Animator.enabled = true;
    }

    private void ResetWindow(FinalConfrontationManager.Issues issueFixed)
    {
        m_Animator.Rebind();
        m_Animator.Update(0f);
        m_Animator.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixIssueDrawingHandler : DrawHandler
{
    [SerializeField] private FinalConfrontationManager.Issues issueToFix;

    protected override void HandleCompletion()
    {
        base.HandleCompletion();
        EventSystem.FixConfrontationIssue(issueToFix);
    }
}

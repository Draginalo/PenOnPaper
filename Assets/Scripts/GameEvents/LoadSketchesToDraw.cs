using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSketchesToDraw : GameEvent
{
    [SerializeField] List<GameObject> newSketchesToDraw;

    private void Start()
    {
        Begin();
    }

    public override void Execute()
    {
        base.Execute();
        EventSystem.LoadSketchesToDraw(newSketchesToDraw);
        GameEventCompleted();
    }
}

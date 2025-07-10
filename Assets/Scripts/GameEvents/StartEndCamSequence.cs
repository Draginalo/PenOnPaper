using UnityEngine;

public class StartEndCamSequence : GameEvent
{
    public override void Execute()
    {
        base.Execute();

        CameraHandler.instance.HandleEndCamSequence(this);
      
    }
}

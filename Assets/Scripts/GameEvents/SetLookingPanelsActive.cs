using UnityEngine;

public class SetLookingPanelsActive : GameEvent
{
    [SerializeField] private bool enableUpPanel;
    [SerializeField] private bool enableDownPanel;

    public override void Execute()
    {
        base.Execute();

        CameraHandler.instance.SetActiveCameraActivators(enableUpPanel, enableDownPanel);
        
        GameEventCompleted(this);
    }
}

using UnityEngine;

public class OpenHospitalDoor : GameEvent
{
    public override void Execute()
    {
        base.Execute();

        EventSystem.OpenHospitalDoor(false);
        
        GameEventCompleted(this);
    }
}

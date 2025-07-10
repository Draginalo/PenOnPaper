using UnityEngine;

public class TurnOffHospitalSignEvent : GameEvent
{
    public override void Execute()
    {
        base.Execute();

        EventSystem.TurnOffHospitalSign();
        
        GameEventCompleted(this);
    }
}

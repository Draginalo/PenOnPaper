using UnityEngine;

public class ChangeDateEvent : GameEvent
{
    [SerializeField] private string newDate;

    public override void Execute()
    {
        base.Execute();

        EventSystem.ChangeDate(newDate);
        
        GameEventCompleted(this);
    }
}

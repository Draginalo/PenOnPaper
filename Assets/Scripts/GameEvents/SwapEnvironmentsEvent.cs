using UnityEngine;

public class SwapEnvironmentsEvent : GameEvent
{
    [SerializeField] private EnvironmentSwitchManager.Environments envToSwitchTo;

    public override void Execute()
    {
        base.Execute();

        EventSystem.SwapEnvironment(envToSwitchTo, this);
        EventSystem.ClearNotepadPage(true, null);
    }
}

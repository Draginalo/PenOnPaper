using UnityEngine;

public class SwapEnvironmentsEvent : GameEvent
{
    [SerializeField] private EnvironmentSwitchManager.Environments envToSwitchTo;

    public override void Execute()
    {
        base.Execute();
        Debug.Log(envToSwitchTo);
        EventSystem.SwapEnvironment(envToSwitchTo);

        //Make this timed exactly when the environment has swapped
        GameEventCompleted(this);
    }
}

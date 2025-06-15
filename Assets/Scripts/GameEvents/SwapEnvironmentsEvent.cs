public class SwapEnvironmentsEvent : GameEvent
{
    public override void Execute()
    {
        base.Execute();
        EventSystem.SwapEnvironment(EnvironmentSwitchManager.Environments.None);

        //Make this timed exactly when the environment has swapped
        GameEventCompleted();
    }
}

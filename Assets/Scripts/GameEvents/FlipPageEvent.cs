public class FlipPageEvent : GameEvent
{
    public override void Execute()
    {
        base.Execute();
        EventSystem.FlipNotepadPage(this);
    }
}

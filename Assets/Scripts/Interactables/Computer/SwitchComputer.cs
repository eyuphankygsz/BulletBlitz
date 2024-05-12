public sealed class SwitchComputer : ComputerBehaviorBase
{

    public override bool GrantAction()
    {
       return true;
    }
    protected override void OnActivate()
    {
        ComputerSettings(true);
        for (int i = 0; i < _activateEvents.Length; i++)
            _activateEvents[i].Invoke();
    }

    protected override void OnDeactivate()
    {
        ComputerSettings(false);
        for (int i = 0; i < _deactivateEvents.Length; i++)
            _deactivateEvents[i].Invoke();
    }
}

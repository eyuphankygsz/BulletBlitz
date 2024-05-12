public sealed class SwitchComputer : ComputerBehaviorBase
{
    public override bool GrantAction()
    {
       return true;
    }
    public override void ComputerBehavior()
    {
        _isActive = !_isActive;
        _animator.SetBool("IsActive", _isActive);
        
        for (int i = 0; i < _events.Length; i++)
            _events[i].Invoke();
    }

}

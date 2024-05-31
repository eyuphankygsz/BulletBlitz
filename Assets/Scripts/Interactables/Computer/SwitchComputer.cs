using UnityEngine;
using UnityEngine.Events;

public sealed class SwitchComputer : ComputerBehaviorBase
{
    [SerializeField] private bool _computerDeactive;
    [SerializeField] private UnityEvent[] _deactiveEvent;
    [SerializeField] private bool _enabled;
    private void Start()
    {
        if (_deactiveEvent != null)
            for (int i = 0; i < _deactiveEvent.Length; i++)
                _deactiveEvent[i].Invoke();

        ComputerSettings(_enabled);
    }
    public override bool GrantAction()
    {
        return true;
    }
    protected override void OnActivate()
    {
        if (_computerDeactive)
        {
            AudioManager.PlayAudio(_lockedSFX);
            return;
        }
        _enabled = !_enabled;
        AudioManager.PlayAudio(_enabled ? _activeSFX : _deactiveSFX);
        ComputerSettings(_enabled);
        for (int i = 0; i < _activateEvents.Length; i++)
            _activateEvents[i].Invoke();
    }

    protected override void OnDeactivate()
    {
        if (_computerDeactive)
        {
            AudioManager.PlayAudio(_lockedSFX);
            return;
        }
        AudioManager.PlayAudio(_deactiveSFX);
        ComputerSettings(false);
        for (int i = 0; i < _deactivateEvents.Length; i++)
            _deactivateEvents[i].Invoke();
    }
    public void ComputerLock(bool locked)
    {
        if (locked && _deactivateEvents == null)
            OnDeactivate();
        _computerDeactive = locked;
    }
    public void CertainBehaviour(bool open)
    {
        if (open)
            OnActivate();
        else
            OnDeactivate();
    }
}

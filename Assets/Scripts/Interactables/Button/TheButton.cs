using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TheButton : Interactable
{
    private bool _isActive;
    private bool _pressure;
    private SpriteRenderer _renderer;
    [SerializeField] private Sprite _activeSprite, _deactiveSprite;
    [SerializeField] private bool _bothWayEvents;
    [SerializeField] private UnityEvent[] _activateEvents, _deactivateEvents;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    public override void OnEnabled()
    {
        Debug.Log("Deneme");
    }

    private void OnActivate()
    {
        _isActive = true;

        for (int i = 0; i < _activateEvents.Length; i++)
            _activateEvents[i].Invoke();

        if (!_bothWayEvents)
            for (int i = 0; i < _deactivateEvents.Length; i++)
                _deactivateEvents[i].Invoke();

        _renderer.sprite = _activeSprite;
    }
    private void OnDeactivate()
    {
        _isActive = false;

        for (int i = 0; i < _deactivateEvents.Length; i++)
            _deactivateEvents[i].Invoke();

        if (!_bothWayEvents)
            for (int i = 0; i < _activateEvents.Length; i++)
                _activateEvents[i].Invoke();

        _renderer.sprite = _deactiveSprite;
    }

    public override bool TriggerEnter(out Interactable interactable)
    {
        interactable = this;
        return TryEnter();
    }
    private bool TryEnter()
    {
        if (_isActive || _pressure)
            return true;

        OnActivate();
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnActivate();
        _pressure = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OnDeactivate();
        _pressure = false;
    }
    public override void TriggerExit()
    {
        if (!_isActive || _pressure)
            return;
        OnDeactivate();
    }
}

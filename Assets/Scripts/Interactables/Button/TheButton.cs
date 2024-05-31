using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TheButton : Interactable
{
    private bool _isActive;
    private SpriteRenderer _renderer;
    [SerializeField] private Sprite _activeSprite, _deactiveSprite;
    [SerializeField] private bool _bothWayEvents;
    [SerializeField] private UnityEvent[] _activateEvents, _deactivateEvents;
    private List<Collider2D> _colliders = new List<Collider2D>();
    Collider2D oldcollider;
    [SerializeField] private AudioClip _pressedSFX, _unpressedSFX;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    public override void OnEnabled() { }

    private void OnActivate()
    {
        _isActive = true;
        AudioManager.PlayAudio(_pressedSFX);
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
        AudioManager.PlayAudio(_unpressedSFX);

        for (int i = 0; i < _deactivateEvents.Length; i++)
            _deactivateEvents[i].Invoke();

        if (!_bothWayEvents)
            for (int i = 0; i < _activateEvents.Length; i++)
                _activateEvents[i].Invoke();

        _renderer.sprite = _deactiveSprite;
    }

    public override bool TriggerEnter(out Interactable interactable, Collider2D collider)
    {

        interactable = this;
        if (!_colliders.Contains(collider))
            _colliders.Add(collider);

        if (_isActive || _colliders.Count != 1)
            return true;

        _isActive = true;
        OnActivate();
        return true;
    }
    public override void TriggerExit(Collider2D collider)
    {
        if (_colliders.Contains(collider))
            _colliders.Remove(collider);

        if (!_isActive || _colliders.Count > 0)
            return;


        _isActive = false;
        OnDeactivate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_colliders.Contains(collision))
            _colliders.Add(collision);

        if (_isActive || _colliders.Count != 1) return;
        _isActive = true;
        OnActivate();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_colliders.Contains(collision))
            _colliders.Remove(collision);
        if (!_isActive || _colliders.Count > 0) return;
        _isActive = false;
        OnDeactivate();
    }
}

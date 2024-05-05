using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : TrapsBase
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private int _damage;
    private Animator _animator;
    private bool _isActive = true;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public override void OnCollide(GameObject affected)
    {
        if (affected.CompareTag("Player"))
            affected.GetComponent<PlayerController>().HitWithDamage(_damage, gameObject);
    }
    public void TrapActivity()
    {
        _isActive = !_isActive;
        _animator.SetBool("IsOn", _isActive);
    }
    public override void TrapActive()
    {
        _collider.enabled = true;
    }

    public override void TrapDeactive()
    {
        _collider.enabled = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletNextEnemy : BulletSkill
{
    Transform _bulletTransform;
    Bullet _bullet;
    Transform _nextTarget = null;
    float _area = 0.1f;
    bool _startSkill;
    bool _endSkill;
    int _maxRange = 40;
    public override void Enter(Transform parentBullet)
    {
        _bulletTransform = parentBullet;
        _bullet = _bulletTransform.GetComponent<Bullet>();
    }

    public override void Update()
    {
        if (_nextTarget == null && _startSkill)
            SelectEnemy();
        else
            MoveToNextEnemy();
    }
    void SelectEnemy()
    {
        if(_area >= _maxRange)
            _bullet.gameObject.SetActive(false);

        RaycastHit2D[] hit = Physics2D.CircleCastAll(_bullet.transform.position, _area, Vector2.down, 0, GameManager.Instance.EnemyLayer);
        _area += Time.deltaTime * 40;
        for (int i = 0; i < _bullet.HittedEnemies.Count; i++)
            for (int j = 0; j < hit.Length; j++)
                if (hit[j].collider.gameObject != _bullet.HittedEnemies[i])
                {
                    _nextTarget = hit[j].collider.transform;
                    _bullet.SetBulletRotation(_nextTarget.position);
                }
    }

    void MoveToNextEnemy()
    {
        _bulletTransform.position = Vector3.MoveTowards(_bulletTransform.position, _nextTarget.position, 0.1f);
    }
    public override void Exit() { }

    public override void TryToActivate()
    {

        if (_bullet.HittedEnemies.Count > 0 && !_endSkill)
        {
            for (int i = 0; i < _bullet.HittedEnemies.Count; i++)
                Physics2D.IgnoreCollision(_bullet.GetComponent<Collider2D>(), _bullet.HittedEnemies[i].GetComponent<Collider2D>());

            _bullet.OverdriveMovement = true;
            _bullet.gameObject.SetActive(true);
            _startSkill = true;
            _endSkill = true;
        }
    }
}

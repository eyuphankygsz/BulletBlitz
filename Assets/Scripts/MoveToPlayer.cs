using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    private Transform _target;
    private float _timer = .5f;
    void Start()
    {
        _target = GameManager.Instance.Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            return;
        }
        if(_timer < 0)
        {
            _timer = 0;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
        Vector3 direction = _target.position - transform.position;
        direction.Normalize();
        transform.Translate(direction * 3 * Time.deltaTime, Space.World);
    }
}

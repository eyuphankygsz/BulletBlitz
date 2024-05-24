using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpController : MonoBehaviour
{
    bool _jump;

    private float _timer = 0.5f;

    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    public void StartJump(int direction)
    {
        _controller.RB.AddForce(new Vector2(200 * direction, 300));
        _controller.CanCheckGround = false;
        _jump = true;
    }
    public void Update()
    {
        if (_jump)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _controller.CanCheckGround = true;
                _jump = false;
                _timer = 0.5f;
            }
        }
    }
}

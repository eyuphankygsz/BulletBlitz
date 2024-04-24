using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState _currentState;
    public PlayerBaseState CurrentState { get { return _currentState; } }

    PlayerController _controller;
    // Update is called once per frame
    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (_currentState != null || !_controller.Dead)
            _currentState.Update();
    }

    public void SwitchState(PlayerBaseState state)
    {
        if (_controller.Dead)
            return;

        if (_currentState != null)
            _currentState.ExitState();

        Debug.Log(state);
        _currentState = state;
        _currentState.EnterState(this);
    }
}

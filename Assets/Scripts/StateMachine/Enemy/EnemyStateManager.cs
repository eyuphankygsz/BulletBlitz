using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    EnemyBaseState _currentState;
    public EnemyBaseState CurrentState { get { return _currentState; } }

    public void SwitchState(EnemyBaseState state)
    {
        if (_currentState != null)
            _currentState.ExitState();
        _currentState = state;
        _currentState.EnterState(this);
    }


}

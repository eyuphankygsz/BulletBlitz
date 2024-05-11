using UnityEngine;

public abstract class PlayerBaseState : MonoBehaviour
{
    public abstract void EnterState(PlayerStateManager player);
    public abstract void StateUpdate();
    public abstract void ExitState();
}

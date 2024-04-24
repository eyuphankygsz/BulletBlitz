using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyBehaviorBase
{
    EnemyBaseState StartState();
    void SetController(EnemyController controller);
    EnemyBaseState Hit();
    EnemyBaseState Dead();
    void OnDrawGizmos();
}

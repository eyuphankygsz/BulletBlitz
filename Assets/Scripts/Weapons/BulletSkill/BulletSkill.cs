using UnityEngine;

public abstract class BulletSkill
{
    public abstract void Enter(Transform parentBullet);
    public abstract void TryToActivate();
    public abstract void Update();
    public abstract void Exit();
}

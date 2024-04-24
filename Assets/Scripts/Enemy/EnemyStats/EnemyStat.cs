using UnityEngine;

[CreateAssetMenu( fileName = "Enemy" , menuName = "EnemyStat/NewEnemy")]
public class EnemyStat : ScriptableObject
{
    public int Health;
    public float Speed, ReMoveDefaultTime;
    public float AttackDefaultTime;
    public int AttackDamage;
    public float BulletSpeed;
    public float AttackSpeed;
    public float AttackRange;
    public float FollowRange;
}

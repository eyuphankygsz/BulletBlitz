using UnityEngine;

public class Weapon : MonoBehaviour
{
    [field: SerializeField] public Transform ShootPoint { get; private set; }
    [field: SerializeField] public WeaponStat WeaponStatSO { get; private set; }
    [field: SerializeField] public Animator AnimatorSc { get; private set; }

    [SerializeField] private AutoShooter _autoShooter;
    private void Shoot()
    {
        _autoShooter.Shoot();
    }
}

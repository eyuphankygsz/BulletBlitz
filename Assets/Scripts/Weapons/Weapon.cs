using UnityEngine;

public class Weapon : MonoBehaviour
{
    [field: SerializeField] public Transform _shootPoint { get; private set; }
    [field: SerializeField] public WeaponStat _weaponStatSO { get; private set; }
    [field: SerializeField] public Animator _animator { get; private set; }

    [SerializeField] private AutoShooter _autoShooter;
    private void Shoot()
    {
        _autoShooter.Shoot();
    }
}

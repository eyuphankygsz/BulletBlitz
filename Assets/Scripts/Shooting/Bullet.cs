using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletDamage { get { return _bulletDamage; } }
    float _bulletDamage;

    public float BulletSpeed { get { return _bulletSpeed; } }
    float _bulletSpeed;

    public string From { get { return _from; } }
    [SerializeField] string _from;

    bool _canActivateSkill;
    BulletSkill _skill;

    Vector3 _shootDirection;

    public List<GameObject> HittedEnemies { get; private set; }

    public bool OverdriveMovement { get; set; }
    public Bullet[] OtherBullets { get; private set; }
    public Vector3 Target { get; private set; }
    // Start is called before the first frame update

    public void Setup(Vector3 startPosition, Vector3 target, float damage, float speed, string from, bool canActivateSkill, Bullet[] otherBullets)
    {
        _from = from;
        _canActivateSkill = canActivateSkill;
        _bulletSpeed = speed;
        _bulletDamage = damage;
        transform.position = startPosition;
        HittedEnemies = new List<GameObject>();
        OverdriveMovement = false;

        OtherBullets = otherBullets;
        Target = target;

        SetBulletRotation(target);

        if (_canActivateSkill && !string.IsNullOrEmpty(PlayerPrefs.GetString("CurrentBulletSkill")))
        {
            _skill = GameManager.Instance.BulletSkills[PlayerPrefs.GetString("CurrentBulletSkill")];
            _skill.Enter(transform);
        }
        gameObject.SetActive(true);
    }

    public void AddHittedEnemies(GameObject enemy)
    {
        HittedEnemies.Add(enemy);
        if (_canActivateSkill)
            _skill?.TryToActivate();
    }
    public void SetBulletRotation(Vector3 target)
    {
        _shootDirection = (target - transform.position).normalized;
        Vector3 difference = transform.position + _shootDirection - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }
    void Update()
    {
        if (!OverdriveMovement)
        {
            transform.position += _shootDirection * _bulletSpeed * Time.deltaTime;
            if (_canActivateSkill)
                _skill?.TryToActivate();
        }
        else if (_canActivateSkill)
            _skill?.Update();
    }
    private void OnDisable()
    {
        if (_canActivateSkill)
            _skill?.TryToActivate();
    }
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

}

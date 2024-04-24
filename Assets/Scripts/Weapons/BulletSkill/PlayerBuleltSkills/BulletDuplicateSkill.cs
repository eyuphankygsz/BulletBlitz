using UnityEngine;

public class BulletDuplicateSkill : BulletSkill
{
    Transform _bulletTransform;
    Bullet _bullet;
    bool _endSkill;

    public override void Enter(Transform parentBullet)
    {
        _bulletTransform = parentBullet;
        _bullet = _bulletTransform.GetComponent<Bullet>();
    }

    public override void Exit()
    {

    }

    public override void TryToActivate()
    {
        if ( !_endSkill)
        {
            _endSkill = true;

            Bullet nextBullet;
            int index = 0;

            for (int i = 0; i < _bullet.OtherBullets.Length; i++)
            {
                if(_bullet == _bullet.OtherBullets[i])
                {
                    index = (i == _bullet.OtherBullets.Length - 1) ? 0 : i + 1;
                    break;
                }
                
            }
            nextBullet = _bullet.OtherBullets[index];
            Vector3 newPos = _bullet.transform.position + ((GameManager.Instance.Player.transform.localScale.x > 0) ? new Vector3(0.5f,0,0) : new Vector3(-0.5f,0,0));
            nextBullet.Setup(newPos, _bullet.Target,_bullet.BulletDamage,_bullet.BulletSpeed,_bullet.From,false,null);

        }
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}

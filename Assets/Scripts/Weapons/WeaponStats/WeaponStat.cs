using EditorAttributes;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "BulletStat/NewBullet")]
public class WeaponStat : ScriptableObject
{
    [GUIColor(GUIColor.Blue)]
    [AssetPreview] public Sprite GunSprite;
    [GUIColor(GUIColor.White)]
    [AssetPreview] public GameObject Projectile;
    [GUIColor(GUIColor.Cyan)]
    public float[] Speed;
    [GUIColor(GUIColor.Red)]
    public int[] Damage;
    [GUIColor(GUIColor.Yellow)]
    public float[] Timer;
    [GUIColor(GUIColor.Magenta)]
    public int MaxBullet;
    [GUIColor(GUIColor.Green)]
    public int AdditionalHealth;
    [GUIColor(GUIColor.Orange)]
    public float ReloadTime;

    [GUIColor(GUIColor.Gray)]
    [SerializeField] private float[] _baseSpeed;
    [SerializeField] private int[] _baseDamage;
    [SerializeField] private float[] _baseTimer;
    [SerializeField] private int _baseMaxBullet;
    [SerializeField] private int _baseAdditionalHealth;
    [SerializeField] private float _baseReloadTime;



    public void SetupGun()
    {
        Speed = new float[_baseSpeed.Length];
        for (int i = 0; i < Speed.Length; i++)
            Speed[i] = _baseSpeed[i] + PlayerPrefs.GetFloat(name + "BulletSpeed" + "Value");

        Damage = new int[_baseDamage.Length];
        for (int i = 0; i < Damage.Length; i++)
            Damage[i] = _baseDamage[i] + PlayerPrefs.GetInt(name + "BulletDamage" + "Value");

        Timer = new float[_baseTimer.Length];
        for (int i = 0; i < Timer.Length; i++)
            Timer[i] = _baseTimer[i] + PlayerPrefs.GetFloat(name + "ShootSpeed" + "Value");

        MaxBullet = _baseMaxBullet + PlayerPrefs.GetInt(name + "BulletCount" + "Value");
        ReloadTime = _baseReloadTime + PlayerPrefs.GetFloat(name + "ReloadSpeed" + "Value");
        AdditionalHealth = _baseAdditionalHealth;
    }

}

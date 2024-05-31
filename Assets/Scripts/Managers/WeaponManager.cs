using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private static WeaponManager instance;
    [SerializeField] WeaponStat[] _weapons;

    private GameObject _playerWeaponParent;
    private Dictionary<string, GameObject> _weaponDictionary = new Dictionary<string, GameObject>();

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        if (!PlayerPrefs.HasKey("CurrentWeapon"))
        {
            PlayerPrefs.SetString("CurrentWeapon", "Pistol");
            PlayerPrefs.SetString("Pistol", "Bought");
        }
        for (int i = 0; i < _weapons.Length; i++)
            _weapons[i].SetupGun();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            PlayerPrefs.SetInt("CorruptedSilver",PlayerPrefs.GetInt("CorruptedSilver") + 1000);
    }
    public static void Setup(AutoShooter shooter)
    {
        instance.SetWeapons(shooter);
    }


    private void SetWeapons(AutoShooter shooter)
    {
        _playerWeaponParent = GameManager.Instance.Player.GetComponent<PlayerController>().WeaponParent;
        _weaponDictionary.Clear();

        Transform weaponArray = _playerWeaponParent.transform;
        for (int i = 0; i < weaponArray.childCount; i++)
        {
            _weaponDictionary.Add(weaponArray.GetChild(i).name, weaponArray.GetChild(i).gameObject);
            weaponArray.GetChild(i).gameObject.SetActive(false);
        }

        shooter.SetWeapon(SelectWeapon());
    }
    private Weapon SelectWeapon()
    {
        string currentWeapon = PlayerPrefs.GetString("CurrentWeapon");
        Debug.Log(currentWeapon);
        _weaponDictionary[currentWeapon].SetActive(true);
        return _weaponDictionary[currentWeapon].GetComponent<Weapon>();
    }
    public static WeaponStat[] GetWeapons()
    {
        return instance._weapons;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private UpgradeButton[] _upgradeBases;
    private void Awake()
    {
        for (int i = 0; i < _upgradeBases.Length; i++)
            _upgradeBases[i].Initialize();

        SelectGun("Pistol");
    }

    public void SelectGun(string gunName)
    {
        PlayerPrefs.SetString("UpgradeWeapon", gunName);
        for (int i = 0; i < _upgradeBases.Length; i++)
            _upgradeBases[i].Setup();
    }
}

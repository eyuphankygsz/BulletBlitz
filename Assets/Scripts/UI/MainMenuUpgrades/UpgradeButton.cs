using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{

    [SerializeField] private string _upgradeName;
    [SerializeField] private Color _deactiveLevelColor, _activeLevelColor;
    private Transform _weaponHolder;
    private Transform _levelHolder;
    private TextMeshProUGUI _priceHolder;
    private int _maxLevel;
    [SerializeField] private UpgradeList[] _upgradeList;
    Dictionary<string, UpgradeLister[]> _upgradeListDic;
    [SerializeField] private UpgradeType _upgradeType;

    [SerializeField] private TextMeshProUGUI _corruptedText, _spaceGemText;

    public void Initialize()
    {
        InitializeDict();
        _weaponHolder = transform.GetChild(0).GetChild(1);
        _levelHolder = transform.GetChild(1);
        _priceHolder = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();

        transform.GetChild(3).GetComponent<Button>().onClick.AddListener(Upgrade);

    }
    public void Setup()
    {
        string upgradeObjName = PlayerPrefs.GetString("UpgradeWeapon");
        int level = PlayerPrefs.GetInt(upgradeObjName + _upgradeName);

        for (int i = 0; i < _weaponHolder.childCount; i++)
        {
            _weaponHolder.GetChild(i).gameObject.SetActive(false);
            if (_weaponHolder.GetChild(i).gameObject.name == upgradeObjName)
                _weaponHolder.GetChild(i).gameObject.SetActive(true);
        }

        if (level != _upgradeListDic[upgradeObjName].Length)
            _priceHolder.text = _upgradeListDic[upgradeObjName][level].Price.ToString();
        else
            _priceHolder.text = "MAX.";

        _maxLevel = _upgradeListDic[upgradeObjName].Length;


        for (int i = 0; i < _levelHolder.childCount; i++)
        {
            _levelHolder.GetChild(i).GetComponent<Image>().color = _deactiveLevelColor;
            if (level > i)
                _levelHolder.GetChild(i).GetComponent<Image>().color = _activeLevelColor;

            if (i >= _maxLevel)
                _levelHolder.GetChild(i).gameObject.SetActive(false);
            else
                _levelHolder.GetChild(i).gameObject.SetActive(true);
        }

        WeaponStat[] weapons = WeaponManager.GetWeapons();
        for (int i = 0; i < weapons.Length; i++)
            weapons[i].SetupGun();

        _corruptedText.text = PlayerPrefs.GetInt("CorruptedSilver").ToString();
        _spaceGemText.text = PlayerPrefs.GetInt("SpaceGem").ToString();
    }
    private void Upgrade()
    {
        string upgradeObjName = PlayerPrefs.GetString("UpgradeWeapon");

        if (PlayerPrefs.GetInt(upgradeObjName + _upgradeName) == _upgradeListDic[upgradeObjName].Length)
            return;

        int corruptedSilver = PlayerPrefs.GetInt("CorruptedSilver");

        if (_upgradeListDic[upgradeObjName][PlayerPrefs.GetInt(upgradeObjName + _upgradeName)].Price > corruptedSilver)
            return;

        PlayerPrefs.SetInt("CorruptedSilver", PlayerPrefs.GetInt("CorruptedSilver") - _upgradeListDic[upgradeObjName][PlayerPrefs.GetInt(upgradeObjName + _upgradeName)].Price);
        switch (_upgradeType)
        {
            case UpgradeType.Int:
                PlayerPrefs.SetInt(upgradeObjName + _upgradeName + "Value",
                    PlayerPrefs.GetInt(upgradeObjName + _upgradeName + "Value")
                    + _upgradeListDic[upgradeObjName][PlayerPrefs.GetInt(upgradeObjName + _upgradeName)].IntAmount);
                break;
            case UpgradeType.Float:
                PlayerPrefs.SetFloat(upgradeObjName + _upgradeName + "Value",
                    PlayerPrefs.GetFloat(upgradeObjName + _upgradeName + "Value")
                    + _upgradeListDic[upgradeObjName][PlayerPrefs.GetInt(upgradeObjName + _upgradeName)].FloatAmount);

                break;
            case UpgradeType.String:
                PlayerPrefs.SetString(upgradeObjName + _upgradeName + "Value",
                    _upgradeListDic[upgradeObjName][PlayerPrefs.GetInt(upgradeObjName + _upgradeName)].StringValue);
                break;
        }

        PlayerPrefs.SetInt(upgradeObjName + _upgradeName, PlayerPrefs.GetInt(upgradeObjName + _upgradeName) + 1);


        Setup();
    }
    private void InitializeDict()
    {
        _upgradeListDic = new Dictionary<string, UpgradeLister[]>();
        for (int i = 0; i < _upgradeList.Length; i++)
        {
            _upgradeListDic.Add(_upgradeList[i].UpgradeObjName, _upgradeList[i].Lister);
        }
    }
}

enum UpgradeType
{
    Int,
    Float,
    String
}

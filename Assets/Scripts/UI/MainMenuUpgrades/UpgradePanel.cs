using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private UpgradeButton[] _upgradeBases;
    [SerializeField] private Button[] _gunButtons;
    [SerializeField] private Button _equipButton;
    [SerializeField] private Vector2[] _oldGunScales;
    [SerializeField] private Sprite[] _oldGunSprites;
    [SerializeField] private Sprite _lockSprite;
    [SerializeField] private Sprite _equipSprite;
    [SerializeField] private Sprite _equipedSprite;
    [SerializeField] private GameObject _gunSpriteHolder;
    private Dictionary<string, int> _gunLine = new Dictionary<string, int>();
    private void Awake()
    {
        for (int i = 0; i < _upgradeBases.Length; i++)
            _upgradeBases[i].Initialize();

        _oldGunScales = new Vector2[_gunButtons.Length];
        _oldGunSprites = new Sprite[_gunButtons.Length];
        for (int i = 0; i < _gunButtons.Length; i++)
        {
            _oldGunSprites[i] = _gunButtons[i].transform.GetChild(0).GetComponent<Image>().sprite;
            _oldGunScales[i] = _gunButtons[i].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
        }
        Redesign();

        for (int i = 0; i < _gunButtons.Length; i++)
            _gunLine.Add(_gunButtons[i].name, i);
    }
    private void OnEnable()
    {
        Redesign();
    }

    private void Redesign()
    {
        for (int i = 0; i < _gunButtons.Length; i++)
        {
            if (PlayerPrefs.GetString(_gunButtons[i].name) != "Bought")
            {
                _gunButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = _lockSprite;
                _gunButtons[i].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
                _gunButtons[i].interactable = false;
            }
            else
            {
                _gunButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = _oldGunSprites[i];
                _gunButtons[i].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = _oldGunScales[i];
                _gunButtons[i].interactable = true;
            }
        }


        SelectGun(PlayerPrefs.GetString("CurrentWeapon"));

        ArrangeEquipButton();
    }
    public void SelectGun(string gunName)
    {
        if (PlayerPrefs.GetString(gunName) != "Bought")
            return;

        PlayerPrefs.SetString("UpgradeWeapon", gunName);
        for (int i = 0; i < _upgradeBases.Length; i++)
            _upgradeBases[i].Setup();

        ArrangeEquipButton();
    }
    public void EquipGun()
    {
        PlayerPrefs.SetString("CurrentWeapon", PlayerPrefs.GetString("UpgradeWeapon"));
        ArrangeEquipButton();

        for (int i = 0; i < _gunSpriteHolder.transform.childCount; i++)
            _gunSpriteHolder.transform.GetChild(i).gameObject.SetActive(false);
        
        _gunSpriteHolder.transform.GetChild(_gunLine[PlayerPrefs.GetString("CurrentWeapon")]).gameObject.SetActive(true);
    }
    public void ArrangeEquipButton()
    {
        if(PlayerPrefs.GetString("CurrentWeapon") == PlayerPrefs.GetString("UpgradeWeapon"))
        {
            _equipButton.interactable = false;
            _equipButton.transform.GetChild(0).GetComponent<Image>().sprite = _lockSprite;
        }
        else
        {
            _equipButton.interactable = true;
            _equipButton.transform.GetChild(0).GetComponent<Image>().sprite = _equipSprite;
        }

    }

}

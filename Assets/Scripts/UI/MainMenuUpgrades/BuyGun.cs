using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyGun : MonoBehaviour
{
    private Dictionary<string, int> _gunPrices = new Dictionary<string, int>();
    [SerializeField] private GameObject[] _guns;
    [SerializeField] private Sprite _boughtSprite, _buySprite;
    [SerializeField] private TextMeshProUGUI _corruptedText;
    private void Awake()
    {
        for (int i = 0; i < _guns.Length; i++)
            _gunPrices.Add(_guns[i].name, int.Parse(_guns[i].transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text));
        
        ArrangeGuns();
    }
    public void BuyThisGun(string name)
    {
        int corruptedSilvers = PlayerPrefs.GetInt("CorruptedSilver");
        if (_gunPrices[name] <= corruptedSilvers)
        {
            PlayerPrefs.SetInt("CorruptedSilver", corruptedSilvers - _gunPrices[name]);
            _corruptedText.text = PlayerPrefs.GetInt("CorruptedSilver").ToString();
            Debug.Log("Name: " + name);
            PlayerPrefs.SetString(name, "Bought");
        }

        ArrangeGuns();
    }

    private void ArrangeGuns()
    {
        for (int i = 0; i < _guns.Length; i++)
        {
            if (PlayerPrefs.GetString(_guns[i].name) != "Bought")
            {
                _guns[i].transform.GetChild(2).GetComponent<Button>().interactable = true;
                _guns[i].transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = _buySprite;
            }
            else
            {
                _guns[i].transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Bought";
                _guns[i].transform.GetChild(2).GetComponent<Button>().interactable = false;
                _guns[i].transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = _boughtSprite;
            }
        }
    }
}

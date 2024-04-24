using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;
    private GameObject _upgradeCanvas;

    [SerializeField] private Upgrade[] _upgrades;
    private List<Upgrade> _tempUpgradeList = new List<Upgrade>();
    private List<Upgrade> _finalUpgradeList;

    private Transform _upgradeContainer;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void Setup()
    {
        GameManager.Instance.Player.GetComponent<PlayerController>().CanMove = false;
        _upgradeCanvas = PanelManager.Instance.Panels["SkillPanel"];

        _tempUpgradeList = _upgrades.ToList();
        _finalUpgradeList = new List<Upgrade>();

        _upgradeContainer = _upgradeCanvas.transform.GetChild(0).GetChild(0);
        for (int i = 0; i < _upgradeContainer.childCount; i++)
        {
            int selectedUpgrade = UnityEngine.Random.Range(0, _tempUpgradeList.Count);

            Transform itemContainer = _upgradeContainer.GetChild(i).GetChild(0).GetChild(0);

            itemContainer.GetChild(0).GetComponent<Image>().sprite = _tempUpgradeList[selectedUpgrade].AffectedItemSprite;
            itemContainer.GetChild(1).GetChild(0).GetComponent<Image>().sprite = _tempUpgradeList[selectedUpgrade].EffectSprite;

            _finalUpgradeList.Add(_tempUpgradeList[selectedUpgrade]);
            AddEvent(i);

            _tempUpgradeList.RemoveAt(selectedUpgrade);

        }

        transform.GetChild(0).GetComponent<SkillerTree>().Approach();
        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.TreeSFX.Clips["Come"],false);
    }
    public void OpenCanvas()
    {
        PanelManager.Instance.Panels["SkillPanel"].SetActive(true);
    }
    public void AddEvent(int i)
    {
        EventTrigger trigger = _upgradeContainer.GetChild(i).GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { UpgradeThis(_finalUpgradeList[i]); });
        trigger.triggers.Add(entry);
    }
    void UpgradeThis(Upgrade upgrade)
    {
        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.TreeSFX.Clips["Upgrade"],false);
     
        int scene = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("Upgrade_Level" + scene, 1);
        switch (upgrade.TypeOfUpgrade)
        {
            case "string":
                PlayerPrefs.SetString(upgrade.NameOfUpgrade, upgrade.ValueOfUpgrade);
                break;
            case "int":
                PlayerPrefs.SetInt(upgrade.NameOfUpgrade, PlayerPrefs.GetInt(upgrade.NameOfUpgrade) + Convert.ToInt32(upgrade.ValueOfUpgrade));

                break;
            case "float":
                PlayerPrefs.SetFloat(upgrade.NameOfUpgrade, PlayerPrefs.GetFloat(upgrade.NameOfUpgrade) + Convert.ToSingle(upgrade.ValueOfUpgrade));
                break;
        }
        
        _upgradeCanvas.SetActive(false);
        GameManager.Instance.EndGameAfterUpgrade();
        transform.GetChild(0).GetComponent<SkillerTree>().GoBack();

        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.TreeSFX.Clips["Go"], false);

    }

    public void NextScene()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

}

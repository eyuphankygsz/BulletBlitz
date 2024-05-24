using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSpecialSkill : MonoBehaviour
{
    private int _minEnemies = 10;
    private int _killedEnemies;

    private Image _skillBar;
    [SerializeField] private GameObject _particle;


    public int KilledEnemies
    {
        get { return _killedEnemies; }
        set
        {
            if (_killedEnemies == _minEnemies)
                return;

            _killedEnemies = value;
            PlayerPrefs.SetInt("KilledEnemies", _killedEnemies);
            ChangeUI();
        }
    }

    private void Awake() => _killedEnemies =  PlayerPrefs.GetInt("KilledEnemies");
    private void Start()
    {
        _skillBar = GameObject.FindGameObjectWithTag("SkillBar").GetComponent<Image>();
        ChangeUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _killedEnemies = _minEnemies;
            PlayerPrefs.SetInt("KilledEnemies", _killedEnemies);
            ChangeUI();
        }
    }

    void ChangeUI()
    {
        _skillBar.fillAmount = (float)_killedEnemies / _minEnemies;
        if (_killedEnemies == _minEnemies)
            SetButton();
    }
    void SetButton()
    {
        EventTrigger trigger = PanelManager.Instance.SpecialButton;
        if (trigger.triggers.Count > 0)
            trigger.triggers.Clear();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventdata) => { ActivateSkill(); });
        trigger.triggers.Add(entry);
    }

    void ActivateSkill()
    {
        if (_killedEnemies != _minEnemies)
            return;
        _particle.SetActive(true);
        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.PlayerSFX.Clips["Hit"], false);

        PlayerController controller = GetComponent<PlayerController>();
        controller.TryToChangeState(controller.SpecialSkillState,controller.CurrentState);
        _killedEnemies = 0;
        PlayerPrefs.SetInt("KilledEnemies", _killedEnemies);
        ChangeUI();
    }


}

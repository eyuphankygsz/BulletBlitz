using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance;
    public Dictionary<string, GameObject> Panels {  get; private set; }
    [SerializeField] private GameObject[] _panels;
    [SerializeField] private Button[] _buttons;
    [field: SerializeField] public EventTrigger JumpButton { get; private set; }
    [field: SerializeField] public EventTrigger SpecialButton { get; private set; }
    [field: SerializeField] public FixedJoystick Joystick { get; private set; }
    void Awake()
    {
        if (FindObjectsOfType<PanelManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetPanels();
    }
    private void Start()
    {
        SetButtons();
    }

    void SetPanels()
    {
        Panels = new Dictionary<string, GameObject>();
        for (int i = 0; i < _panels.Length; i++)
        {
            Panels.Add(_panels[i].name, _panels[i]);
        }
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    void SetButtons()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            ButtonBase[] bases = _buttons[i].GetComponents<ButtonBase>();
            
            for (int j = 0; j < bases.Length; j++)
                _buttons[i].onClick.AddListener(bases[j].OnActivate);
        }
    }
}

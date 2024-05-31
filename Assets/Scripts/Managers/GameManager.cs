using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject Player { get { if (_player == null) _player = GameObject.FindWithTag("Player"); return _player; } set { _player = value; } }
    private GameObject _player;

    private AutoShooter _shooter;
    private PlayerSpecialSkill _skiller;

    public List<GameObject> Collectables { get; private set; } = new List<GameObject>();


    public GameObject UpgradeCanvas { get; private set; }

    public bool SpecialSkill { get { return _specialSkill; } set { _specialSkill = value; } }
    private bool _specialSkill;

    //--------Layers---------
    [field: SerializeField] public LayerMask PlayerLayer { get; private set; }
    [field: SerializeField] public LayerMask EnemyLayer { get; private set; }
    [field: SerializeField] public LayerMask LevelLayer { get; private set; }
    [field: SerializeField] public LayerMask CollectableLayer { get; private set; }
    [field: SerializeField] public LayerMask TrapsLayer { get; private set; }
    [field: SerializeField] public LayerMask BulletsLayer { get; private set; }
    [field: SerializeField] public LayerMask InteractableLayer { get; private set; }
    //-----------------------

    //--------BulletSkills--------

    public Dictionary<string, BulletSkill> BulletSkills
    {
        get
        {
            SetBulletSkills();
            return _bulletSkills;
        }
    }
    private Dictionary<string, BulletSkill> _bulletSkills;

    [SerializeField] private List<GameObject> _enemies = new List<GameObject>();

    public List<GameObject> GetEnemies() { return _enemies; }
    private void Awake()
    {
        Instance = this;

        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
        Application.targetFrameRate = 60;

        _bulletSkills = new Dictionary<string, BulletSkill>();
        _shooter = Player.GetComponent<AutoShooter>();
        _skiller = Player.GetComponent<PlayerSpecialSkill>();

        UpgradeCanvas = GameObject.FindGameObjectWithTag("UpgradeCanvas");
        SetBulletSkills();
        IgnoreCollisions();
    }
    private void Start()
    {
        EnemySoundHolder.Instance.CreateSFXLists();
    }

    public void AddEnemy(GameObject enemy)
    {
        _enemies.Add(enemy);
        _shooter.RefreshEnemies();
    }
    public void RemoveEnemy(GameObject enemy)
    {
        if (_player.GetComponent<PlayerController>().CurrentState != PlayerController.States.Special)
            _skiller.KilledEnemies++;

        _enemies.Remove(enemy);
        _shooter.RefreshEnemies();
        if (_enemies.Count == 0)
            EndGame();
    }

    void SetBulletSkills()
    {
        _bulletSkills.Clear();
        _bulletSkills.Add("SkipToEnemy", new BulletNextEnemy());
        _bulletSkills.Add("DoubleArrow", new BulletDuplicateSkill());
    }
    void EndGame()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        if (PlayerPrefs.HasKey("Upgrade_Level" + scene) || PlayerPrefs.GetInt("Level") % 5 != 0)
            GameObject.FindGameObjectWithTag("LevelLazer").GetComponent<Lasers>().CertainActivation(true);
        //else
        //    ShowUpgrade();
    }
    public void EndGameAfterUpgrade()
    {
        Player.GetComponent<PlayerController>().CanMove = true;
        GameObject.FindGameObjectWithTag("LevelLazer").GetComponent<Lasers>().CertainActivation(true);
    }
    public void NextScene()
    {
        //TODO: Add Loading Screen!
        SkillManager.Instance.NextScene();
        SceneManager.LoadScene(PlayerPrefs.GetInt("Level") + 1);
    }

    void ShowUpgrade()
    {
        SkillManager.Instance.Setup();
    }

    public void AddCollectable(GameObject collectable)
    {
        Collectables.Add(collectable);
        _shooter.RefreshEnemies();
    }
    private void IgnoreCollisions()
    {
        Physics2D.IgnoreLayerCollision(Mathf.RoundToInt(Mathf.Log(PlayerLayer.value, 2)), Mathf.RoundToInt(Mathf.Log(InteractableLayer.value, 2)), true);
        Physics2D.IgnoreLayerCollision(Mathf.RoundToInt(Mathf.Log(EnemyLayer.value, 2)), Mathf.RoundToInt(Mathf.Log(InteractableLayer.value, 2)), true);
        //        Physics2D.IgnoreLayerCollision(Mathf.RoundToInt(Mathf.Log(InteractableLayer.value, 2)), Mathf.RoundToInt(Mathf.Log(InteractableLayer.value, 2)));
    }
    public void StartGame()
    {
        PanelManager.Instance.ClosePanel(PanelManager.Instance.Panels["MainPanel"]);
        PanelManager.Instance.OpenPanel(PanelManager.Instance.Panels["InGamePanel"]);
        SceneManager.LoadScene(1);
    }
    public void Dead()
    {
        PanelManager.Instance.OpenPanel(PanelManager.Instance.Panels["DeathPanel"]);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

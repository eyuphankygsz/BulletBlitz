using EditorAttributes;
using UnityEngine;

public class BulletSelector : MonoBehaviour
{
    [GUIColor(GUIColor.Lime)]
    [SerializeField, ColorField(GUIColor.Lime)]
    private WeaponStat[] _bulletStats;
    private void Awake()
    {
        GetComponent<AutoShooter>().WeaponStat =  _bulletStats[PlayerPrefs.GetInt("PlayerBulletStat")];
    }
    private void Start()
    {
        enabled = false;
    }
}

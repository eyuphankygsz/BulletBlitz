using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrades/New Upgrade")]
public class Upgrade : ScriptableObject
{
    public string NameOfUpgrade;
    public string TypeOfUpgrade;
    public string ValueOfUpgrade;
    public Sprite AffectedItemSprite;
    public Sprite EffectSprite;
}

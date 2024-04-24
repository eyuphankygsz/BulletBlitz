using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New UpgradeList", menuName = "UpgradeList/New List")]
public class UpgradeList : ScriptableObject
{
    public string UpgradeObjName;
    public UpgradeLister[] Lister;
}

[Serializable] 
public struct UpgradeLister
{
    public int Price;
    public float FloatAmount;
    public int IntAmount;
    public string StringValue;
}

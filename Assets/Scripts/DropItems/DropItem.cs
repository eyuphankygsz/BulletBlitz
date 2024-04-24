using UnityEngine;

[CreateAssetMenu(fileName ="DropItem", menuName = "DropItem/New Item")]
public class DropItem : ScriptableObject
{
    public GameObject DroppedItem;
    public int MinQuantity, MaxQuantity;
    public int Luck;
}

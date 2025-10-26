using UnityEngine;

public enum ItemType
{
    HealthPotion,
    ManaPotion,
    LvlUpPotion,
    MaxHealthPotion,
    MaxManaPorion,
    Amulet,
    Sword,
    Book,
    Stave,
}

public class Item : MonoBehaviour
{
    public ItemType itemType;
    [HideInInspector] public bool select;
    public float minStage, maxStage;
    public GameObject icon;

    public virtual void RemoveBonus()
    {

    }
}

using UnityEngine;

public class PotionItems : Item
{
    Potions potions;

    public float addMaxValue;

    private void Start()
    {
        potions = FindObjectOfType<Potions>();
    }
}

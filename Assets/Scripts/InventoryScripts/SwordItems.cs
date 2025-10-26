using UnityEngine;

public class SwordItems : Item
{
    public float blockPercent;

    public float Block(ref float Damage)
    {
        Damage = Damage - Damage / 100 * blockPercent;
        return Damage;
    }
}

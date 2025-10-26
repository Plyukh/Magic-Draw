using UnityEngine;

public class CanvasEffectManager : MonoBehaviour
{
    [SerializeField] CurrencyBase currencyBase;

    [SerializeField] private CanvasEffect xp_Effect;
    [SerializeField] private CanvasEffect damage_Effect;
    [SerializeField] private CanvasEffect miss_Effect;
    [SerializeField] private CanvasEffect mana_Effect;
    [SerializeField] private CanvasEffect heal_Effect;
    [SerializeField] private ParticleSystem currency_Effect;

    [SerializeField] Color32 airColor, fireColor, waterColor, earthColor, darkColor;

    public void SpawnXPEffect(string Text)
    {
        CanvasEffect XP_Effect = Instantiate(xp_Effect, gameObject.transform);
        XP_Effect.TakeEffect(new Vector2(525f, -420f), "+" + Text + " XP");
    }
    public void SpawnDamageEffect(Vector2 Position, string Text)
    {
        CanvasEffect Damage_Effect = Instantiate(damage_Effect, gameObject.transform);
        Damage_Effect.TakeEffect(Position, "-" + Text + " HP");
    }
    public void SpawnMissEffect(Vector2 Position)
    {
        CanvasEffect Miss_Effect = Instantiate(miss_Effect, gameObject.transform);
        Miss_Effect.TakeEffect(Position);
    }
    public void SpawnHealEffect(string Text)
    {
        CanvasEffect Heal_Effect = Instantiate(heal_Effect, gameObject.transform);
        if(Text[0] != '-')
        {
            Heal_Effect.TakeEffect(new Vector2(-950f, -190f), "+" + Text + " HP");
        }
        else
        {
            Heal_Effect.TakeEffect(new Vector2(-950f, -190f), Text + " HP");
        }
    }
    public void SpawnMagicRecoveryEffect(string Text)
    {
        CanvasEffect MagicRecovery_Effect = Instantiate(mana_Effect, gameObject.transform);
        if (Text[0] != '-')
        {
            MagicRecovery_Effect.TakeEffect(new Vector2(950f, -190f), "+" + Text + " MP");
        }
        else
        {
            MagicRecovery_Effect.TakeEffect(new Vector2(950f, -190f), Text + " MP");
        }
    }

    [System.Obsolete]
    public void SpawnCurrencyEffect(GameObject Object, int Currency, Elements Elements)
    {
        ParticleSystem Currency_Effect = Instantiate(currency_Effect, Object.transform.position, Object.transform.rotation, Object.transform.parent);
        Currency_Effect.emission.SetBurst(0, new ParticleSystem.Burst(0,Currency));

        currencyBase.AddPoints(Currency ,Elements);

        if (Elements == Elements.Earth)
        {
            Currency_Effect.startColor = earthColor;
        }
        else if (Elements == Elements.Water)
        {
            Currency_Effect.startColor = waterColor;
        }
        else if (Elements == Elements.Fire)
        {
            Currency_Effect.startColor = fireColor;
        }
        else if (Elements == Elements.Air)
        {
            Currency_Effect.startColor = airColor;
        }
        else
        {
            Currency_Effect.startColor = darkColor;
        }
    }
}
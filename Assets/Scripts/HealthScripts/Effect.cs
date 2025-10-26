using UnityEngine;

public class Effect : MonoBehaviour
{
    private Health health;
    private Enemy enemy;

    private Elements type;
    private int burnDamage, freezeDamage;

    [SerializeField] private float burnTime, freezeTime, stunTime;
    private float second;

    private Color32 freezeColor = new Color32(0,100,255,255);
    private GameObject burnEffect, stunEffect;
        
    private void Awake()
    {
        health = GetComponent<Health>();
        enemy = GetComponent<Enemy>();

        burnEffect = transform.GetChild(0).gameObject;
        stunEffect = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        if (!enemy.Pause)
        {
            if (burnTime > 0)         //Burn
            {
                burnEffect.SetActive(true);
                second += Time.deltaTime;
                if (second >= 1)
                {
                    burnEffect.GetComponent<AudioSource>().Play();
                    burnTime -= second;
                    second = 0;
                    Burn(burnDamage);
                    if (burnTime <= 0)
                    {
                        burnEffect.GetComponent<Animator>().SetTrigger("End");
                        burnTime = 0;
                    }
                }
            }

            if (freezeTime > 0)        //Freeze
            {
                Freeze(freezeDamage);
                second += Time.deltaTime;
                if (second >= 0.5f)
                {
                    freezeTime -= second;
                    second = 0;
                    if (freezeTime <= 0)
                    {
                        freezeTime = 0;
                        enemy.SpeedReset();
                        enemy.ChangeSpeedAnimation(1);
                        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                    }
                }
            }

            if (stunTime > 0)        //Stun
            {
                Stun();
                stunEffect.SetActive(true);
                second += Time.deltaTime;
                if (second >= 0.25f)
                {
                    stunTime -= second;
                    second = 0;
                    if (stunTime <= 0)
                    {
                        stunEffect.GetComponent<Animator>().SetTrigger("End");
                        stunTime = 0;
                        enemy.ChangeSpeed(false);
                    }
                }
            }
        }
    }

    public void TakeSpell(float EffectTime, Elements element, int Damage)
    {
        if (GetComponent<EnemyHealth>().Resistance == Elements.Air && element == Elements.Earth ||
                 GetComponent<EnemyHealth>().Resistance == Elements.Fire && element == Elements.Air ||
                 GetComponent<EnemyHealth>().Resistance == Elements.Water && element == Elements.Fire ||
                 GetComponent<EnemyHealth>().Resistance == Elements.Earth && element == Elements.Water)
        {
            Damage /= 2;
            EffectTime /= 2;
        }
        else if (GetComponent<EnemyHealth>().Resistance == Elements.Air && element == Elements.Fire ||
                 GetComponent<EnemyHealth>().Resistance == Elements.Fire && element == Elements.Water ||
                 GetComponent<EnemyHealth>().Resistance == Elements.Water && element == Elements.Earth ||
                 GetComponent<EnemyHealth>().Resistance == Elements.Earth && element == Elements.Air)
        {
            Damage *= 2;
            EffectTime *= 2;
        }
        if (element == Elements.Fire)
        {
            burnTime = EffectTime;
            burnDamage = Damage;
        }
        if (element == Elements.Water)
        {
            freezeTime = EffectTime;
            freezeDamage = Damage;
        }
        if (element == Elements.Earth)
        {
            stunTime = EffectTime;
        }
    }
    void ApplyEffects()
    {
        if(type == Elements.Fire)
        {
            Burn(burnDamage);
        }
        if(type == Elements.Earth)
        {
            Stun();
        }
        if (type == Elements.Water)
        {
            Freeze(freezeDamage);
        }
    }
    void Burn(float damage)
    {
        damage /= 5;
        health.ApplyDamage(damage);
    }
    void Freeze(float damage)
    {
        GetComponent<SpriteRenderer>().color = freezeColor;
        damage /= 4;
        enemy.ChangeSpeed(damage);
    }
    void Stun()
    {
        enemy.ChangeSpeed(true);
    }
}

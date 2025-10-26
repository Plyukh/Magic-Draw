using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] protected GameObject destroyPrefab;
    [SerializeField] protected Elements elementResistance;
    protected CanvasEffectManager canvasEffectManager;

    protected float invulnerabilityTime = 0.5f;
    protected float currentTime;
    protected float bigSpellDamage;
    protected bool invulnerability, miss;
    
    private Elements counterElemental;

    public Elements Resistance
    {
        get
        {
            return elementResistance;
        }
    }
    public Elements CounterElemental
    {
        get
        {
            return counterElemental;
        }
    }

    public bool Miss
    {
        get
        {
            return miss;
        }
        set
        {
            miss = value;
        }
    }

    public CanvasEffectManager CanvasEffectManager
    {
        get
        {
            return canvasEffectManager;
        }
    }

    private void Start()
    {
        canvasEffectManager = FindObjectOfType<CanvasEffectManager>();
        currentHP = maxHP;
        ApplyCounterElemental();
    }

    private void Update()
    {
        if (invulnerability)
        {
            bigSpellDamage = 0;
            currentTime += Time.deltaTime;
            if (currentTime >= invulnerabilityTime)
            {
                invulnerability = false;
                currentTime = 0;
            }
        }
    }

    private void ApplyCounterElemental()
    {
        if (elementResistance == Elements.Air)
        {
            counterElemental = Elements.Fire;
        }
        else if (elementResistance == Elements.Earth)
        {
            counterElemental = Elements.Air;
        }
        else if (elementResistance == Elements.Water)
        {
            counterElemental = Elements.Earth;
        }
        else if (elementResistance == Elements.Fire)
        {
            counterElemental = Elements.Water;
        }
        else
        {
            counterElemental = Elements.All;
        }
    }

    public void ApplyBigDamage(float Damage)
    {
        bigSpellDamage = Damage;
        if (invulnerability == false)
        {
            ApplyDamage(bigSpellDamage);
            invulnerability = true;
        }
    }

    public override void ApplyDamage(float Damage)
    {
        if (Damage > 0)
        {
            if (miss)
            {
                canvasEffectManager.SpawnMissEffect(gameObject.transform.position);
            }
            else
            {
                StartCoroutine(ApplyDamageCoroutine());
                Damage = Mathf.Round(Damage);
                canvasEffectManager.SpawnDamageEffect(gameObject.transform.position, Damage.ToString());
                currentHP -= Damage;
                if (currentHP <= 0)
                {
                    Death();
                }
            }
        }
    }

    public void ApplyDamage(Elements ElementResistance, ref int Damage)
    {
        if (elementResistance == Elements.Air && ElementResistance == Elements.Earth ||
            elementResistance == Elements.Fire && ElementResistance == Elements.Air ||
            elementResistance == Elements.Water && ElementResistance == Elements.Fire ||
            elementResistance == Elements.Earth && ElementResistance == Elements.Water)
        {
            Damage /= 2;
        }
        else if (elementResistance == Elements.Air && ElementResistance == Elements.Fire ||
                 elementResistance == Elements.Fire && ElementResistance == Elements.Water ||
                 elementResistance == Elements.Water && ElementResistance == Elements.Earth ||
                 elementResistance == Elements.Earth && ElementResistance == Elements.Air)
        {
            Damage *= 2;
        }
        if (miss)
        {
            canvasEffectManager.SpawnMissEffect(gameObject.transform.position);
        }
        else
        {
            if (Damage > 0)
            {
                StartCoroutine(ApplyDamageCoroutine());
                canvasEffectManager.SpawnDamageEffect(gameObject.transform.position, Damage.ToString());
                currentHP -= Damage;
                if (currentHP <= 0)
                {
                    if(ElementResistance == Elements.Earth)
                    {
                        FindObjectOfType<UnlockSystem>().UnlockAchivement(1);
                    }
                    else if (ElementResistance == Elements.Water)
                    {
                        FindObjectOfType<UnlockSystem>().UnlockAchivement(2);
                    }
                    else if (ElementResistance == Elements.Fire)
                    {
                        FindObjectOfType<UnlockSystem>().UnlockAchivement(3);
                    }
                    else if (ElementResistance == Elements.Air)
                    {
                        FindObjectOfType<UnlockSystem>().UnlockAchivement(4);
                    }

                    Death();
                }
            }
        }
    }
    public override void Death()
    {
        GetComponent<Enemy>().Target.GetComponent<PlayerStats>().AddXP(GetComponent<Enemy>().XP);

        AmuletItems amulet = FindObjectOfType<AmuletItems>();
        if (amulet != null)
        {
            if (amulet.percentMurders)
            {
                if(amulet.manaBonus != 0)
                {
                    GetComponent<Enemy>().Target.GetComponent<PlayerStats>().ApplyMagicRecovery(Mathf.Round(maxHP / 100 * amulet.manaBonus));
                    FindObjectOfType<CanvasEffectManager>().SpawnMagicRecoveryEffect(Mathf.Round(maxHP / 100 * amulet.manaBonus).ToString());
                }
                else if (amulet.healthBonus != 0)
                {
                    GetComponent<Enemy>().Target.GetComponent<PlayerStats>().ApplyHeal(Mathf.Round(maxHP / 100 * amulet.healthBonus));
                    FindObjectOfType<CanvasEffectManager>().SpawnHealEffect(Mathf.Round(maxHP / 100 * amulet.healthBonus).ToString());
                }
            }
        }

        if (tag == "RightEnemy")
        {
            FindObjectOfType<Tutorial>().CompleteTutorial(0);
        }
        else if (tag == "LeftEnemy")
        {
            FindObjectOfType<Tutorial>().CompleteTutorial(1);
        }

        GameObject DestroyPrefab = Instantiate(destroyPrefab.gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
        if(gameObject.GetComponent<SpriteRenderer>()!= null)
        {
            DestroyPrefab.GetComponent<SpriteRenderer>().flipX = gameObject.GetComponent<SpriteRenderer>().flipX;
        }

        canvasEffectManager.SpawnCurrencyEffect(gameObject, GetComponent<Enemy>().currentCurrency, Resistance);

        if(elementResistance == Elements.All)
        {
            FindObjectOfType<UnlockSystem>().UnlockEnemy(maxHP, GetComponent<Enemy>().Speed, "Dark");
        }
        else
        {
            FindObjectOfType<UnlockSystem>().UnlockEnemy(maxHP, GetComponent<Enemy>().Speed, Resistance.ToString());
        }

        Destroy(gameObject);
    }
    public void DeathOnPlayer()
    {
        Instantiate(destroyPrefab.gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);

        canvasEffectManager.SpawnCurrencyEffect(gameObject, GetComponent<Enemy>().currentCurrency, Resistance);

        if (elementResistance == Elements.All)
        {
            FindObjectOfType<UnlockSystem>().UnlockEnemy(maxHP, GetComponent<Enemy>().Speed, "Dark");
        }
        else
        {
            FindObjectOfType<UnlockSystem>().UnlockEnemy(maxHP, GetComponent<Enemy>().Speed, Resistance.ToString());
        }

        Destroy(gameObject);
    }
}
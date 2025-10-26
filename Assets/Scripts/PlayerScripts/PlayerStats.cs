using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : Health
{
    private Collider2D collider;

    [SerializeField] private float[] baseValue;

    [SerializeField] private float[] maxValue;

    [SerializeField] private float[] currentValue;

    [SerializeField] private Image[] sliders;
    [SerializeField] private int lvl;
    [SerializeField] private int addStats;
    [SerializeField] private Text currentLvlText;
    [SerializeField] private Text[] valueText;
    [SerializeField] private GameObject lvlUpEffect;

    [SerializeField] private float requiredXP;

    [SerializeField] private GameObject stats;
    [SerializeField] private Chapter chapter;

    [SerializeField] private AddValueEffect addHealthEffect, addManaEffect;
    [SerializeField] private GameObject applyDamageEffect, applyMagicEffect;
    [SerializeField] private GameObject destroyEffect;

    private AudioSource audioSource;
    [SerializeField] AudioClip hit, dead;

    [SerializeField] private GameObject ShoopUI;
    [SerializeField] private Button[] shopButtons;
    private bool shopping,hub;

    private UnlockSystem unlockSystem;
    private GameOver gameOver;
    private Pause pause;

    [HideInInspector] public bool cheat;

    public float CurrentLvl
    {
        get
        {
            return lvl;
        }
    }
    public float CurrentMana
    {
        get
        {
            return currentValue[1];
        }
    }
    public float CurrentXP
    {
        get
        {
            return currentValue[2];
        }
    }
    public float MaxMana
    {
        get
        {
            return maxValue[1];
        }
    }
    public float MaxHealth
    {
        get
        {
            return maxValue[0];
        }
    }
    public float MaxXp
    {
        get
        {
            return maxValue[2];
        }
    }
    public Chapter Chapter
    {
        get
        {
            return chapter;
        }
    }
    public GameObject Stats
    {
        get
        {
            return stats;
        }
    }
    public float[] BaseValues
    {
        get
        {
            return baseValue;
        }
        set
        {
            baseValue = value;
        }
    }

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        unlockSystem = FindObjectOfType<UnlockSystem>();
        gameOver = FindObjectOfType<GameOver>();
        pause = FindObjectOfType<Pause>();
    }

    private void Update()
    {

        //Scrennshot
        if (Input.GetKeyDown(KeyCode.Space) && tag == "Player")
        {
            ScreenCapture.CaptureScreenshot("Screenshot.png");
        }

        maxHP = maxValue[0];
        currentHP = currentValue[0];
        currentLvlText.text = lvl.ToString();

        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].fillAmount = currentValue[i] / maxValue[i];
            valueText[i].text = currentValue[i].ToString() + "/" + maxValue[i].ToString();
        }

        for (int i = 0; i < maxValue.Length - 1; i++)
        {
            if(currentValue[i] > maxValue[i])
            {
                currentValue[i] = maxValue[i];
            }
        }
    }

    public void AddBonus(float HealthBonus, float ManaBonus)
    {
        maxValue[0] = maxValue[0] + HealthBonus;
        maxValue[1] = maxValue[1] + ManaBonus;
    }

    public void ApplyDamage(float Damage, bool Ghost_Projectile = false, bool Animation_Sound = true)
    {
        if (Damage > 0)
        {
            if (Ghost_Projectile == false)
            {
                if (GameObject.Find("Sword").transform.childCount > 0)
                {
                    float Initial_Damage = Damage;
                    GameObject.Find("Sword").transform.GetChild(0).GetComponent<SwordItems>().Block(ref Damage);
                    AddXP(Initial_Damage / 10);
                }
            }

            AddValueEffect ApplyDamageEffect = Instantiate(applyDamageEffect.GetComponent<AddValueEffect>(), sliders[0].transform);
            ApplyDamageEffect.effectValue = -Damage;

            Damage = Mathf.Round(Damage);
            if (!cheat)
            {
                currentValue[0] -= Damage;
            }

            if (currentValue[0] <= 0)
            {
                currentValue[0] = 0;
                Death();
                return;
            }

            StartCoroutine(ApplyDamageCoroutine());

            if (Animation_Sound == true)
            {
                audioSource.clip = hit;
                audioSource.pitch = 1.25f;
                audioSource.Play();

                unlockSystem.UnlockAchivement(5);
            }
        }
    }

    public override void Death()
    {
        collider.isTrigger = false;

        StartCoroutine(pause.PauseDead(1.5f));

        GetComponent<PlayerAnimations>().Dead();
        Instantiate(destroyEffect, gameObject.transform);

        audioSource.clip = dead;
        audioSource.pitch = 1f;
        audioSource.Play();

        AmuletItems amulet = FindObjectOfType<AmuletItems>();
        if (amulet != null)
        {
            if (amulet.reincarnation)
            {
                amulet.Reincarnation();
                return;
            }
        }

        unlockSystem.UnlockAchivement(0);

        gameOver.Game_Over();
    }

    public void ResetStats()
    {
        unlockSystem.products[8].LoadBonus();
        unlockSystem.products[9].LoadBonus();

        maxValue[0] = baseValue[0];
        maxValue[1] = baseValue[1];
        maxValue[2] = baseValue[2];

        collider.isTrigger = true;

        lvl = 1;
        requiredXP = 15;

        for (int i = 0; i < currentValue.Length; i++)
        {
            currentValue[i] = 0;
        }
    }

    public void ApplyMagic(float Magic, out bool canCast)
    {
        canCast = true;
        if(Magic <= currentValue[1])
        {
            if(Magic != 0)
            {
                AddValueEffect ApplyMagicEffect = Instantiate(applyMagicEffect.GetComponent<AddValueEffect>(), sliders[1].transform);
                ApplyMagicEffect.effectValue = -Magic;
                if (!cheat)
                {
                    currentValue[1] -= Magic;
                }
            }
        }
        else
        {
            canCast = false;
        }
    }
    public void ApplyMagicRecovery(float MagicRecovery)
    {
        currentValue[1] += MagicRecovery;

        if (currentValue[1] >= maxValue[1])
        {
            currentValue[1] = maxValue[1];
        }
    }
    public void ApplyMagicRecoveryEffect(float Division)
    {
        AddValueEffect mana = Instantiate(addManaEffect, sliders[1].transform);

        mana.valueType = Value.Mana;
        mana.division = Division;
        mana.value = maxValue[1];
        mana.playerStats = this;
    }
    public override void ApplyHeal(float Heal)
    {
        if (currentHP > 0 || FindObjectOfType<Angel>() || lvl == 1 && currentValue[0] == 0 && currentValue[1] == 0)
        {
            currentValue[0] += Heal;

            if (currentValue[0] >= maxValue[0])
            {
                currentValue[0] = maxValue[0];
            }
        }
    }
    public void ApplyHealEffect(float Division)
    {
        AddValueEffect hp = Instantiate(addHealthEffect, sliders[0].transform);
        hp.valueType = Value.Health;
        hp.division = Division;
        hp.value = maxValue[0];
        hp.playerStats = this;
    }
    public void AddXP(double XP)
    {
        XP = System.Math.Round(XP, 0, System.MidpointRounding.AwayFromZero);
        if (XP > 0)
        {
            FindObjectOfType<CanvasEffectManager>().SpawnXPEffect(XP.ToString());
            if (CurrentLvl < 75)
            {
                currentValue[2] += System.Convert.ToInt32(XP);
                if (currentValue[2] >= maxValue[2])
                {
                    lvl += 1;

                    float bufetXP;

                    bufetXP = currentValue[2] - maxValue[2];

                    maxValue[2] += requiredXP;

                    requiredXP += 5;

                    currentValue[2] = 0;
                    currentValue[2] += bufetXP;

                    LvlUp();
                }
            }
            else
            {
                currentValue[2] = maxValue[2];
            }
        }
    }

    void FirstStage()
    {
        if (shopping)
        {
            ShoopUI.SetActive(true);
            shopping = false;

            for (int i = 0; i < shopButtons.Length; i++)
            {
                shopButtons[i].interactable = true;
            }
        }
        else if (hub)
        {
            gameOver.StartButton.SetActive(true);
            hub = false;
        }
        else if (lvl == 1 && currentValue[0] == 0 && currentValue[1] == 0)
        {
            stats.SetActive(true);
            chapter.Open_Close_Book();
            chapter.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SkillIcon>().IconInfo.LvlUpButton();
            chapter.BookButton.interactable = false;
            ApplyHealEffect(1);
            ApplyMagicRecoveryEffect(1);
        }
    }
    public void Shoping()
    {
        shopping = true;
    }
    public void Hub()
    {
        hub = true;
    }

    public void LvlUp()
    {
        maxValue[0] += addStats;
        maxValue[1] += addStats;

        chapter.Open_Close_Book();
        chapter.BookButton.interactable = false;

        Instantiate(lvlUpEffect, gameObject.transform.position, lvlUpEffect.transform.rotation, gameObject.transform.parent);
        chapter.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SkillIcon>().IconInfo.LvlUpButton();

        ApplyHealEffect(2);
        ApplyMagicRecoveryEffect(2);

        if (CurrentLvl == 5)
        {
            unlockSystem.UnlockAchivement(7);
        }
        if (CurrentLvl == 10)
        {
            unlockSystem.UnlockAchivement(8);
        }
    }

    public void SetDestroyEffect(GameObject NewEffect)
    {
        destroyEffect = NewEffect;
    }
}
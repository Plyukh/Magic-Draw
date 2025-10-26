using UnityEngine;
using UnityEngine.UI;

public class BossHealth : EnemyHealth
{
    [SerializeField] private string textName;
    [SerializeField] private string engTextName;
    [SerializeField] private string ruTextName;
    private GameObject backgroundSmall, backgroundLarge;
    private GameObject bossHPObject;
    private Image slider;

    private UnlockSystem unlockSystem;

    private void Start()
    {
        canvasEffectManager = FindObjectOfType<CanvasEffectManager>();
        unlockSystem = FindObjectOfType<UnlockSystem>();

        currentHP = maxHP;

        bossHPObject = GameObject.Find("Boss HP").transform.GetChild(0).gameObject;
        bossHPObject.SetActive(true);
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

        for (int i = 0; i < bossHPObject.transform.childCount; i++)
        {
            if (bossHPObject.transform.GetChild(i).name == "Current Boss HP")
            {
                slider = bossHPObject.transform.GetChild(i).GetComponent<Image>();
            }
            if (bossHPObject.transform.GetChild(i).name == "Name Text")
            {
                if (unlockSystem.languageManager.currentLanguage == Language.English)
                {
                    textName = engTextName;
                }
                else if (unlockSystem.languageManager.currentLanguage == Language.Russian)
                {
                    textName = ruTextName;
                }
                bossHPObject.transform.GetChild(i).GetComponent<Text>().text = textName;
            }
            if (bossHPObject.transform.GetChild(i).name == "Background Small")
            {
                backgroundSmall = bossHPObject.transform.GetChild(i).gameObject;
            }
            if (bossHPObject.transform.GetChild(i).name == "Background Large")
            {
                backgroundLarge = bossHPObject.transform.GetChild(i).gameObject;
            }
        }

        if (textName.Length >= 8)
        {
            backgroundSmall.SetActive(false);
            backgroundLarge.SetActive(true);
        }
        else
        {
            backgroundSmall.SetActive(true);
            backgroundLarge.SetActive(false);
        }

        slider.fillAmount = currentHP / maxHP;
    }

    public override void Death()
    {
        if (currentHP <= 0)
        {
            bossHPObject.SetActive(false);
        }

        if (GetComponent<NecromancerBoss>())
        {
            unlockSystem.UnlockAchivement(13);

            for (int i = 0; i < FindObjectsOfType<EnemyHealth>().Length; i++)
            {
                if (!FindObjectsOfType<EnemyHealth>()[i].GetComponent<BossHealth>())
                {
                    FindObjectsOfType<EnemyHealth>()[i].Death();
                }
            }
        }
        if (GetComponent<PlayerBoss>())
        {
            GetComponent<PlayerBoss>().spawner.RandomSpawn();
        }

        base.Death();
    }
}
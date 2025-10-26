using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : Icon
{
    private PlayerStats player;

    public float maxPoints, cureentPoints;

    public string drawType;

    public int damage, baseDamage;
    public float effectTime, baseEffectTime;
    public int manacost;

    public int addDamage;
    public float addEffectTime;

    [SerializeField] private SkillIcon nextSpell;
    public Spell spell;

    public bool open;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        icon = transform.GetChild(0).GetComponent<Image>();
        unlockSystem = FindObjectOfType<UnlockSystem>();
    }

    public void Update()
    {
        spell.damage = damage;
        spell.effectTime = effectTime;
        spell.manacost = manacost;

        if(cureentPoints >= maxPoints)
        {
            gameObject.GetComponent<Image>().color = new Color32(255, 255, 0, 255);
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if(cureentPoints > 0)
        {
            transform.GetChild(0).GetComponent<Image>().color = new Color32(255,255,255,255);
            if (nextSpell != null)
            {
                nextSpell.open = true;
            }
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().color = new Color32(150, 150, 150, 255);
            if (nextSpell != null)
            {
                nextSpell.open = false;
            }
        }
    }

    public void ResetIcon()
    {
        cureentPoints = 0;
        if(engType == "Dark")
        {
            damage = baseDamage;
        }
    }
}

using UnityEngine;
enum MagicType
{
    Air,
    Fire,
    Earth,
    Water
}

public class StaveItems : Item
{
    [SerializeField] private MagicType magicType;
    public int bonusMultiplication;
    private int bonusDamage;
    private float bonusEffect;

    private void Start()
    {
        AddBonus();
    }

    void AddBonus()
    {
        GameObject skillTree = GameObject.Find("Player").GetComponent<PlayerStats>().Chapter.transform.GetChild(0).GetChild(0).gameObject;
        for (int i = 0; i < skillTree.transform.childCount; i++)
        {
            if(skillTree.transform.GetChild(i).GetComponent<SkillIcon>().type == magicType.ToString())
            {
                bonusDamage = skillTree.transform.GetChild(i).GetComponent<SkillIcon>().addDamage * bonusMultiplication;
                bonusEffect = skillTree.transform.GetChild(i).GetComponent<SkillIcon>().addEffectTime * bonusMultiplication;
                skillTree.transform.GetChild(i).GetComponent<SkillIcon>().damage += bonusDamage;
                skillTree.transform.GetChild(i).GetComponent<SkillIcon>().effectTime += bonusEffect;
                skillTree.transform.GetChild(i).GetComponent<SkillIcon>().Update();
            }
        }
    }
    public override void RemoveBonus()
    {
        GameObject skillTree = GameObject.Find("Player").GetComponent<PlayerStats>().Chapter.transform.GetChild(0).GetChild(0).gameObject;
        for (int i = 0; i < skillTree.transform.childCount; i++)
        {
            if (skillTree.transform.GetChild(i).GetComponent<SkillIcon>().type == magicType.ToString())
            {
                bonusDamage = -skillTree.transform.GetChild(i).GetComponent<SkillIcon>().addDamage * bonusMultiplication;
                bonusEffect = -skillTree.transform.GetChild(i).GetComponent<SkillIcon>().addEffectTime * bonusMultiplication;
                skillTree.transform.GetChild(i).GetComponent<SkillIcon>().damage += bonusDamage;
                skillTree.transform.GetChild(i).GetComponent<SkillIcon>().effectTime += bonusEffect;
                skillTree.transform.GetChild(i).GetComponent<SkillIcon>().Update();
            }
        }
    }
}

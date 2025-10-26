using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IconInfo : MonoBehaviour
{
    SpellManager spellManager;

    [SerializeField] Image icon;
    [SerializeField] GameObject unlockedText, lockedText;
    [SerializeField] Text skillPoints;
    [SerializeField] Text nameText, descriptionText;
    [SerializeField] Text lvlRequirement;
    [SerializeField] Text typeText;
    [SerializeField] Color32 airColor, fireColor, waterColor, earthColor, darkColor;
    [SerializeField] Text manacost, damage, effectTime, effectTimeNone;
    [SerializeField] Text addDamage, addEffectTime;
    [SerializeField] GameObject drawZone;
    [SerializeField] Button addPointButton, selectButton;
    [SerializeField] string[] drawTypes;

    [SerializeField] Text healthText;
    [SerializeField] Text speedText;
    [SerializeField] Text experienceText;
    [SerializeField] Text abilities;

    [SerializeField] Text enemies;

    [SerializeField] GameObject addSkillPointEffect;

    private Icon currentIcon;

    public Button AddPointButton
    {
        get
        {
            return addPointButton;
        }
    }

    private void Awake()
    {
        spellManager = GameObject.FindGameObjectWithTag("Player").GetComponent<SpellManager>();
    }

    public void TransferInfo(Icon Icon)
    {
        currentIcon = Icon;

        if (currentIcon.GetComponent<SettingsIcon>())
        {
            return;
        }

        icon.sprite = currentIcon.icon.sprite;
        icon.color = currentIcon.icon.color;
        nameText.text = currentIcon.name;
        descriptionText.text = currentIcon.description;
        typeText.text = currentIcon.type;
        icon.transform.parent.GetComponent<Image>().color = currentIcon.icon.transform.parent.GetComponent<Image>().color;
        icon.GetComponent<RectTransform>().sizeDelta = currentIcon.icon.GetComponent<RectTransform>().sizeDelta;

        if (currentIcon.GetComponent<SkillIcon>())
        {
            skillPoints.text = currentIcon.GetComponent<SkillIcon>().cureentPoints + "/" + currentIcon.GetComponent<SkillIcon>().maxPoints;
            manacost.text = currentIcon.GetComponent<SkillIcon>().manacost.ToString();

            if (currentIcon.engType == "Air")
            {
                typeText.color = airColor;
            }
            else if (currentIcon.engType == "Fire")
            {
                typeText.color = fireColor;
            }
            else if (currentIcon.engType == "Water")
            {
                typeText.color = waterColor;
            }
            else if (currentIcon.engType == "Earth")
            {
                typeText.color = earthColor;
            }
            else
            {
                typeText.color = darkColor;
            }

            damage.text = currentIcon.GetComponent<SkillIcon>().damage.ToString();
            if (currentIcon.GetComponent<SkillIcon>().effectTime == 0)
            {
                effectTimeNone.gameObject.SetActive(true);
                effectTime.gameObject.SetActive(false);
            }
            else
            {
                effectTimeNone.gameObject.SetActive(false);
                effectTime.gameObject.SetActive(true);
                effectTime.text = currentIcon.GetComponent<SkillIcon>().effectTime.ToString();
            }

            if (currentIcon.GetComponent<SkillIcon>().cureentPoints > 0 && currentIcon.GetComponent<SkillIcon>().cureentPoints < currentIcon.GetComponent<SkillIcon>().maxPoints)
            {
                addDamage.text = "+" + currentIcon.GetComponent<SkillIcon>().addDamage.ToString();
                if (currentIcon.GetComponent<SkillIcon>().addEffectTime == 0)
                {
                    addEffectTime.text = "";
                }
                else
                {
                    addEffectTime.text = "+" + currentIcon.GetComponent<SkillIcon>().addEffectTime.ToString();
                }
            }
            else
            {
                addDamage.text = "";
                addEffectTime.text = "";
            }
            if (currentIcon.GetComponent<SkillIcon>().open)
            {
                lvlRequirement.gameObject.SetActive(false);
            }
            else
            {
                lvlRequirement.gameObject.SetActive(true);
            }

            for (int i = 0; i < drawTypes.Length; i++)
            {
                if (drawTypes[i] == currentIcon.GetComponent<SkillIcon>().drawType)
                {
                    drawZone.GetComponent<Animator>().SetBool(currentIcon.GetComponent<SkillIcon>().drawType, true);
                }
                else
                {
                    drawZone.GetComponent<Animator>().SetBool(drawTypes[i], false);
                }
            }

            if (currentIcon.GetComponent<SkillIcon>().cureentPoints >= currentIcon.GetComponent<SkillIcon>().maxPoints || !currentIcon.GetComponent<SkillIcon>().open)
            {
                addPointButton.interactable = false;
            }
            else
            {
                addPointButton.interactable = true;
            }
            if (currentIcon.GetComponent<SkillIcon>().cureentPoints <= 0)
            {
                selectButton.interactable = false;
            }
            else
            {
                selectButton.interactable = true;
            }

            if (addPointButton.gameObject.activeInHierarchy)
            {
                selectButton.gameObject.SetActive(false);
            }
            else
            {
                selectButton.gameObject.SetActive(true);
            }
        }
        else if (currentIcon.GetComponent<EnemyIcon>())
        {
            if (currentIcon.engType == "Air")
            {
                typeText.color = airColor;
            }
            else if (currentIcon.engType == "Fire")
            {
                typeText.color = fireColor;
            }
            else if (currentIcon.engType == "Water")
            {
                typeText.color = waterColor;
            }
            else if (currentIcon.engType == "Earth")
            {
                typeText.color = earthColor;
            }
            else if (currentIcon.engType == "Dark")
            {
                typeText.color = darkColor;
            }

            healthText.text = currentIcon.GetComponent<EnemyIcon>().health.ToString();
            speedText.text = currentIcon.GetComponent<EnemyIcon>().speed.ToString();
            experienceText.text = currentIcon.GetComponent<EnemyIcon>().experience.ToString();

            if (currentIcon.GetComponent<EnemyIcon>().abilities.Length == 0)
            {
                abilities.text = "";
            }
            else
            {
                if(FindObjectOfType<UnlockSystem>().languageManager.currentLanguage == Language.English)
                {
                    abilities.text = "Abilities:\n";
                }
                else if (FindObjectOfType<UnlockSystem>().languageManager.currentLanguage == Language.Russian)
                {
                    abilities.text = "Способности:\n";
                }
                for (int i = 0; i < currentIcon.GetComponent<EnemyIcon>().abilities.Length; i++)
                {
                    abilities.text += "- " + currentIcon.GetComponent<EnemyIcon>().abilities[i].ToString() + "\n";
                }
            }
        }
        else if (currentIcon.GetComponent<LocationIcon>())
        {
            if (currentIcon.engType == "Air")
            {
                typeText.color = airColor;
            }
            else if (currentIcon.engType == "Fire")
            {
                typeText.color = fireColor;
            }
            else if (currentIcon.engType == "Water")
            {
                typeText.color = waterColor;
            }
            else if (currentIcon.engType == "Earth")
            {
                typeText.color = earthColor;
            }
            else if (currentIcon.engType == "Dark")
            {
                typeText.color = darkColor;
            }
            else if (currentIcon.engType == "Hub")
            {
                typeText.color = new Color32(0, 0, 0, 255);
            }

            if (currentIcon.GetComponent<LocationIcon>().enemies.Length == 0)
            {
                enemies.text = "";
            }
            else
            {
                if (FindObjectOfType<UnlockSystem>().languageManager.currentLanguage == Language.English)
                {
                    enemies.text = "Enemies:\n";
                }
                else if (FindObjectOfType<UnlockSystem>().languageManager.currentLanguage == Language.Russian)
                {
                    enemies.text = "Противники:\n";
                }
                for (int i = 0; i < currentIcon.GetComponent<LocationIcon>().enemies.Length; i++)
                {
                    enemies.text += "- " + currentIcon.GetComponent<LocationIcon>().enemies[i].ToString() + "\n";
                }
            }
        }
        else if (currentIcon.GetComponent<Achievements>())
        {
            typeText.text = currentIcon.GetComponent<Achievements>().currentProgress + "/" + currentIcon.GetComponent<Achievements>().progress;
        }

        if (!currentIcon.GetComponent<Achievements>())
        {
            if (currentIcon.unlock)
            {
                unlockedText.SetActive(true);
                lockedText.SetActive(false);
            }
            else
            {
                unlockedText.SetActive(false);
                lockedText.SetActive(true);

                lockedText.GetComponent<CanvasGroup>().alpha = 0;
                StartCoroutine(Alpha(lockedText.GetComponent<CanvasGroup>(), 0.05f, 0.1f));
            }
        }
    }

    public void LvlUpButton()
    {
        addPointButton.gameObject.SetActive(true);
        selectButton.gameObject.SetActive(false);
    }

    public void AddPoint()
    {
        if(currentIcon.GetComponent<SkillIcon>().cureentPoints > 0)
        {
            currentIcon.GetComponent<SkillIcon>().damage += currentIcon.GetComponent<SkillIcon>().addDamage;
            currentIcon.GetComponent<SkillIcon>().effectTime += currentIcon.GetComponent<SkillIcon>().addEffectTime;
        }
        currentIcon.GetComponent<SkillIcon>().cureentPoints += 1;

        Instantiate(addSkillPointEffect, gameObject.transform.parent.parent.parent);
        addPointButton.gameObject.SetActive(false);

        AddSkill();

        currentIcon.GetComponent<SkillIcon>().Update();

        FindObjectOfType<Chapter>().Open_Close_Book();
    }

    public void AddSkill()
    {
        for (int i = 0; i < drawTypes.Length; i++)
        {
            if(currentIcon.GetComponent<SkillIcon>().drawType == drawTypes[i] && spellManager.currentSpells[i] == null)
            {
                spellManager.currentSpells[i] = currentIcon.GetComponent<SkillIcon>().spell;
                spellManager.spellIcons[i].sprite = currentIcon.icon.sprite;
                spellManager.spellIcons[i].color += new Color32(0,0,0,255);
                break;
            }
        }
    }
    public void SelectSkill()
    {
        for (int i = 0; i < drawTypes.Length; i++)
        {
            if (currentIcon.GetComponent<SkillIcon>().drawType == drawTypes[i])
            {
                spellManager.currentSpells[i] = currentIcon.GetComponent<SkillIcon>().spell;
                spellManager.spellIcons[i].sprite = currentIcon.icon.sprite;
                spellManager.spellIcons[i].color += new Color32(0, 0, 0, 255);
                break;
            }
        }
    }

    IEnumerator Alpha(CanvasGroup CanvasGroup, float Value, float Seconds)
    {

        if(CanvasGroup.alpha != 1)
        {
            yield return new WaitForSeconds(Seconds);

            CanvasGroup.alpha += Value;
            StartCoroutine(Alpha(CanvasGroup, Value, Seconds));
        }
        else
        {
            StopCoroutine(Alpha(CanvasGroup, Value, Seconds));
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Product : MonoBehaviour
{
    [SerializeField] UnlockSystem unlockSystem;

    [SerializeField] CurrencyBase currencyBase;
    [SerializeField] private GameObject icon;

    public float bonus;
    public float addBonus;
    public float addAddBonus;

    public int currentLvl;
    public int maxLvl;

    public Text lvlText;

    public float[] price;
    public float[] currentValues;
    public float[] addPrice;

    public float[] basePrice;

    public Image[] sliders;
    public Text[] priceTexts;

    public Button buyButton;

    private Shop shop;
    public GameObject effect;

    [SerializeField] private GameObject[] targets;
    [SerializeField] private SkillIcon[] skillIcons;

    public bool damage, effectTime, health, mana, skin;

    private void Start()
    {
        shop = transform.parent.parent.GetComponent<Shop>();
    }

    private void Update()
    {
        for (int i = 0; i < price.Length; i++)
        {
            sliders[i].fillAmount = currentValues[i] / price[i];
            priceTexts[i].text = currentValues[i].ToString() + "/" + price[i].ToString();
        }

        lvlText.text = currentLvl.ToString() + "/" + maxLvl.ToString();
    }

    public void AddValues(int index)
    {
        if (currentValues[index] < price[index])
        {
            currentValues[index] += 1;
        }

        if(currentValues[0] == price[0] && currentValues[1] == price[1] && currentValues[2] == price[2] &&
            currentValues[3] == price[3] && currentValues[4] == price[4])
        {
            StartCoroutine(UpProduct());
        }
    }

    public void ShowProduct()
    {
        for (int i = 0; i < price.Length; i++)
        {
            if (price[i] > 0)
            {
                sliders[i].gameObject.SetActive(true);
            }
            else
            {
                sliders[i].gameObject.SetActive(false);
            }
        }

        // Buy Button active
        if (currencyBase.earthPoints < price[0])
        {
            buyButton.interactable = false;
            return;
        }
        if (currencyBase.waterPoints < price[1])
        {
            buyButton.interactable = false;
            return;
        }
        if (currencyBase.firePoints < price[2])
        {
            buyButton.interactable = false;
            return;
        }
        if (currencyBase.airPoints < price[3])
        {
            buyButton.interactable = false;
            return;
        }
        if (currencyBase.darkPoints < price[4])
        {
            buyButton.interactable = false;
            return;
        }
        if (currentLvl >= maxLvl)
        {
            buyButton.interactable = false;
            return;
        }

        buyButton.interactable = true;
    }

    public void BuyProduct()
    {
        currencyBase.SetProduct = this;
        buyButton.interactable = false;

        shop.InteractableButtons(false);

        for (int i = 0; i < sliders.Length; i++)
        {
            if (sliders[i].gameObject.activeInHierarchy)
            {
                StartCoroutine(currencyBase.MoveParticle(sliders[i].gameObject, i));
            }
        }
    }

    public void LoadBonus()
    {
        if (health)
        {
            targets[0].GetComponent<PlayerStats>().BaseValues[0] = 50 + bonus;
        }
        else if (mana)
        {
            targets[0].GetComponent<PlayerStats>().BaseValues[1] = 50 + bonus;
        }
        else if (damage)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].GetComponent<SkillIcon>().damage = targets[i].GetComponent<SkillIcon>().baseDamage + (int)bonus;
                targets[i].GetComponent<SkillIcon>().Update();
            }
        }
        else if (effectTime)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].GetComponent<SkillIcon>().effectTime = targets[i].GetComponent<SkillIcon>().baseEffectTime + bonus;
                targets[i].GetComponent<SkillIcon>().Update();
            }
        }
        else if (skin)
        {
            if(currentLvl > 0)
            {
                OpenNewSkills(true);
            }
        }
    }

    public void ResetBonus()
    {
        currentLvl = 0;
        bonus = 0;
        addBonus = 0;
        for (int i = 0; i < price.Length; i++)
        {
            price[i] = basePrice[i];
        }
        LoadBonus();
    }

    public void OpenNewSkills(bool open)
    {
        for (int i = 0; i < skillIcons.Length; i++)
        {
            skillIcons[i].gameObject.SetActive(open);
        }
    }

    IEnumerator UpProduct()
    {
        Instantiate(effect, icon.transform);

        RectTransform rect1 = icon.GetComponent<RectTransform>();
        RectTransform rect2 = icon.transform.GetChild(0).GetComponent<RectTransform>();
        Vector2 addValues = new Vector2(5,5);
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.075f);
            rect1.sizeDelta += addValues;
            rect2.sizeDelta += addValues;
        }
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.075f);
            rect1.sizeDelta -= addValues;
            rect2.sizeDelta -= addValues;
        }

        shop.InteractableButtons(true);
        currentLvl += 1;
        addBonus += addAddBonus;
        bonus += addBonus;

        if (damage)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].GetComponent<SkillIcon>().damage = targets[i].GetComponent<SkillIcon>().baseDamage + (int)bonus;
            }
        }
        else if (effectTime)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].GetComponent<SkillIcon>().effectTime = targets[i].GetComponent<SkillIcon>().baseEffectTime + bonus;
            }
        }
        else if (skin)
        {
            for (int i = 0; i < unlockSystem.skills.Length; i++)
            {
                SkillIcon skillIcon = unlockSystem.skills[i];

                if (skillIcon.engType == "Dark")
                {
                    unlockSystem.UnlockSkill(skillIcon);
                }
            }

            unlockSystem.UnlockSkin(2);
            unlockSystem.skins[2].SelectSkin();

            //Effect
            Instantiate(targets[0], transform.parent.parent.parent.parent.parent.position, transform.rotation, transform.parent.parent.parent.parent.parent);
        }

        for (int i = 0; i < currentValues.Length; i++)
        {
            currentValues[i] = 0;

            if(currentLvl == maxLvl)
            {
                price[i] = 0;
            }
            else
            {
                price[i] += addPrice[i];
            }
        }

        unlockSystem.SaveProducts();
        ShowProduct();

        StopCoroutine(UpProduct());
    }
}

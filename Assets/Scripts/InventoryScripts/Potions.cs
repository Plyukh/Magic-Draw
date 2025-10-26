using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Potions : MonoBehaviour
{
    [SerializeField] private Text healthText, manaText;
    [SerializeField] private Button healthButton, manaButton;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Button addPointButton;
    [SerializeField] private GameObject companyManager;

    public int healthPotionsNumber, manaPotionsNumber;

    private void Update()
    {
        healthText.text = healthPotionsNumber.ToString();
        manaText.text = manaPotionsNumber.ToString();

        if(healthText.text == "0")
        {
            healthButton.interactable = false;
        }
        else
        {
            healthButton.interactable = true;
        }
        if (manaText.text == "0")
        {
            manaButton.interactable = false;
        }
        else
        {
            manaButton.interactable = true;
        }
    }

    public void AddHealthPotin()
    {
        healthPotionsNumber += 1;
        StartCoroutine(AddPotionAnimation(ItemType.HealthPotion));
    }

    public void AddManaPotin()
    {
        manaPotionsNumber += 1;
        StartCoroutine(AddPotionAnimation(ItemType.ManaPotion));
    }

    public void HealthPotion()
    {
        healthPotionsNumber -= 1;
        playerStats.ApplyHealEffect(1);

        FindObjectOfType<UnlockSystem>().UnlockAchivement(6);
    }
    public void ManaPotion()
    {
        manaPotionsNumber -= 1;
        playerStats.ApplyMagicRecoveryEffect(1);

        FindObjectOfType<UnlockSystem>().UnlockAchivement(6);
    }
    public void AddMaxValuePotion(float Health = 0, float Mana = 0)
    {
        playerStats.AddBonus(Health, Mana);

        FindObjectOfType<UnlockSystem>().UnlockAchivement(6);
    }
    public void LvlUpPotion()
    {
        playerStats.AddXP(playerStats.MaxXp - playerStats.CurrentXP);
        addPointButton.onClick.AddListener(SetActiveManager);

        FindObjectOfType<UnlockSystem>().UnlockAchivement(6);
    }

    void SetActiveManager()
    {
        companyManager.SetActive(true);
        addPointButton.onClick.RemoveListener(SetActiveManager);
    }

    private IEnumerator AddPotionAnimation(ItemType PotionType)
    {
        if(PotionType == ItemType.HealthPotion)
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(0.01f);
                healthButton.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta += new Vector2(1, 1);
            }
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(0.01f);
                healthButton.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta -= new Vector2(1, 1);
            }
        }
        else if (PotionType == ItemType.ManaPotion)
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(0.01f);
                manaButton.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta += new Vector2(1, 1);
            }
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(0.01f);
                manaButton.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta -= new Vector2(1, 1);
            }
        }
        StopCoroutine(AddPotionAnimation(PotionType));
    }
}

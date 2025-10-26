using UnityEngine;
using UnityEngine.UI;

public class CompanyManager : MonoBehaviour
{
    [SerializeField] private PlayerAnimations player;
    [SerializeField] private Skin skin;
    [SerializeField] private ScrollAnimations[] scrolls;

    [SerializeField] private Item[] items;

    [SerializeField] private Level[] companies;

    public RewardAnimations rewardAnimations;

    public GameObject stageImage;
    public int currentStage;
    public Item currentReward;

    [SerializeField] private Tutorial tutorial;
    
    private void Start()
    {
        ResetPompanies();
    }

    public void OpenScrolls()
    {
        if((currentStage + 1) % 5 != 0)
        {
            for (int i = 0; i < scrolls.Length; i++)
            {
                scrolls[i].gameObject.SetActive(true);
            }
        }
        else
        {
            scrolls[0].gameObject.SetActive(true);
        }
    }
    public void CloseScrolls()
    {
        for (int i = 0; i < scrolls.Length; i++)
        {
            scrolls[i].CloseScrollAnimation();
        }

        stageImage.SetActive(false);
    }

    public void ShowCompanies()
    {
        if(!scrolls[scrolls.Length - 1].transform.GetChild(0).gameObject.activeInHierarchy)
        {
            currentStage += 1;

            if(currentStage == 2)
            {
                tutorial.ShowTutorial(2);
            }

            stageImage.SetActive(true);
            if(FindObjectOfType<UnlockSystem>().languageManager.currentLanguage == Language.English)
            {
                stageImage.transform.GetChild(0).GetComponent<Text>().text = "Stage: " + currentStage;
            }
            else if (FindObjectOfType<UnlockSystem>().languageManager.currentLanguage == Language.Russian)
            {
                stageImage.transform.GetChild(0).GetComponent<Text>().text = "Этап: " + currentStage;
            }

            int StageCompaniesLength = 0;
            for (int i = 0; i < companies.Length; i++)
            {
                if(companies[i].stage == currentStage)
                {
                    StageCompaniesLength += 1;
                }
            }
            Level[] StageCompanies = new Level[StageCompaniesLength];
            for (int i = 0; i < companies.Length; i++)
            {
                if (companies[i].stage == currentStage)
                {
                    for (int j = 0; j < StageCompaniesLength; j++)
                    {
                        if(StageCompanies[j] == null)
                        {
                            StageCompanies[j] = companies[i];
                            break;
                        }
                    }
                }
            }
            if (StageCompaniesLength == 1)
            {
                SpawnRandomCompany(0, 0, StageCompanies);
            }
            else if (StageCompaniesLength == 2)
            {
                if(player.GetComponent<Animator>().runtimeAnimatorController == skin.animator)
                {
                    SpawnRandomCompany(1, 0, StageCompanies);
                }
                else
                {
                    SpawnRandomCompany(0, 0, StageCompanies);
                }
            }
            else
            {
                for (int i = 0; i < scrolls.Length; i++)
                {
                    int random = RandomCompany(StageCompanies.Length);
                    SpawnRandomCompany(random, i, StageCompanies);
                }
            }
        }
    }

    Item RandomItem(Level Company)
    {
        int random = 0;
        Item randomItem = null;

        if (Company.difficulty == "Easy")
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].itemType == ItemType.HealthPotion)
                {
                    if(currentStage != 1)
                    {
                        randomItem = items[i];
                        break;
                    }
                }
            }
        }
        else if (Company.difficulty == "Normal")
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].itemType == ItemType.ManaPotion)
                {
                    randomItem = items[i];
                    break;
                }
            }
        }
        else if(Company.difficulty == "Hard" || Company.difficulty == "Boss")
        {
            int itemNumber = 0;
            for (int i = 0; i < items.Length; i++)
            {
                if (Company.stage >= items[i].minStage && Company.stage <= items[i].maxStage && !items[i].select)
                {
                    itemNumber += 1;
                }
            }
            Item[] selectItems = new Item[itemNumber];
            for (int i = 0; i < items.Length; i++)
            {
                if (Company.stage >= items[i].minStage && Company.stage <= items[i].maxStage && !items[i].select)
                {
                    for (int j = 0; j < selectItems.Length; j++)
                    {
                        if (selectItems[j] == null)
                        {
                            selectItems[j] = items[i];
                            break;
                        }
                    }
                }
            }
            random = Random.Range(0, selectItems.Length);
            randomItem = selectItems[random];
        }
        return randomItem;
    } 
    int RandomCompany(int Length)
    {
        int randomCompany = Random.Range(0, Length);

        return randomCompany;
    }
    void SpawnRandomCompany(int random, int i, Level[] Companies)
    {
        if (Companies[random].select == false)
        {
            Level company = Instantiate(Companies[random], scrolls[i].transform);
            Companies[random].select = true;
            if (currentStage != 25)
            {
                company.reward = RandomItem(Companies[random]);
            }
            if (company.reward != null)
            {
                Instantiate(company.reward.icon, company.transform);
                company.reward.select = true;
            }

            FindObjectOfType<UnlockSystem>().CheckUnlock(company);

            for (int j = 0; j < Companies.Length; j++)
            {
                if (Companies[j].stage > 1 && Companies[j].difficulty == Companies[random].difficulty || Companies[j].type == Companies[random].type)
                {
                    if (Companies[j].type != "Dark")
                    {
                        Companies[j].select = true;
                    }
                }
            }
        }
        else
        {
            SpawnRandomCompany(RandomCompany(Companies.Length), i, Companies);
        }
    }

    public void ResetPompanies()
    {
        currentStage = 0;
        currentReward = null;

        for (int i = 0; i < companies.Length; i++)
        {
            companies[i].select = false;
        }
        for (int i = 0; i < items.Length; i++)
        {
            items[i].select = false;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class UnlockSystem : MonoBehaviour
{
    [SerializeField] Image bookImage;

    public SkillIcon[] skills;
    public ItemIcon[] items;
    public EnemyIcon[] enemies;
    public LocationIcon[] locations;
    public Achievements[] achievements;
    public Tutorial tutorial;
    public Slider[] settings;
    public CurrencyBase currency;
    public Skin[] skins;
    public Product[] products;
    public LanguageManager languageManager;

    private void Start()
    {
        Initialize();
        LoadAllSaves();
        CheckGoogleAchievements();
    }

    public void Initialize()
    {
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) => { });
    }

    public void CheckUnlock(Level Company)
    {
        Company.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(AddLocation);

        void AddLocation()
        {
            UnlockLocation(Company.transform.GetChild(2).GetChild(0).GetComponent<Image>());
            Company.transform.GetChild(4).GetComponent<Button>().onClick.RemoveListener(AddLocation);
        }

        for (int i = 0; i < items.Length; i++)
        {
            if(Company.transform.childCount > 5)
            {
                if (Company.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<Image>().sprite == items[i].unlockImage.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite)
                {
                    if (items[i].unlock)
                    {
                        Company.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                        break;
                    }
                    else
                    {
                        Company.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color32(0, 0, 0, 255);
                        break;
                    }
                }
            }
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            for (int j = 0; j < Company.transform.GetChild(3).transform.childCount; j++)
            {
                if (Company.transform.GetChild(3).GetChild(j).GetChild(0).GetComponent<Image>().sprite == enemies[i].unlockImage.transform.GetChild(0).GetComponent<Image>().sprite)
                {
                    if (enemies[i].unlock)
                    {
                        Company.transform.GetChild(3).GetChild(j).GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                        break;
                    }
                    else
                    {
                        Company.transform.GetChild(3).GetChild(j).GetChild(0).GetComponent<Image>().color = new Color32(0, 0, 0, 255);
                        break;
                    }
                }
            }

        }
        for (int i = 2; i < locations.Length; i++)
        {
            if(Company.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite == locations[i].unlockImage.transform.GetChild(0).GetComponent<Image>().sprite)
            {
                if (locations[i].unlock)
                {
                    Company.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    break;
                }
                else
                {
                    Company.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = new Color32(0, 0, 0, 255);
                    break;
                }
            }
        }
    }

    public void UnlockSkill(SkillIcon skillIcon)
    {
        skillIcon.unlock = true;
        ES3.Save("Skills", skills);
    }
    public void UnlockItem(Item Item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].unlockImage.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite == Item.icon.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite)
            {
                if (items[i].unlock == false)
                {
                    items[i].unlock = true;
                    ES3.Save("Items", items);

                    UnlockAchivement(9);

                    StopAllCoroutines();
                    bookImage.color = new Color32(255, 255, 255, 255);
                    StartCoroutine(UlockCoroutine(bookImage));
                    break;
                }
            }
        }
    }
    public void UnlockEnemy(float Health, float Speed, string Type)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].health == Health && enemies[i].speed == Speed && enemies[i].engType == Type)
            {
                if(enemies[i].unlock == false)
                {
                    enemies[i].unlock = true;
                    ES3.Save("Enemies", enemies);

                    UnlockAchivement(10);

                    StopAllCoroutines();
                    bookImage.color = new Color32(255, 255, 255, 255);
                    StartCoroutine(UlockCoroutine(bookImage));
                    break;
                }
            }
        }
    }

    public void UnlockLocation(Image Location)
    {
        for (int i = 2; i < locations.Length; i++)
        {
            if (locations[i].unlockImage.transform.GetChild(0).GetComponent<Image>().sprite == Location.sprite)
            {
                if (locations[i].unlock == false)
                {
                    locations[i].unlock = true;
                    ES3.Save("Locations", locations);

                    UnlockAchivement(11);

                    StopAllCoroutines();
                    bookImage.color = new Color32(255,255,255,255);
                    StartCoroutine(UlockCoroutine(bookImage));
                    break;
                }
            }
        }
    }

    public void UnlockAchivement(int Index)
    {
        if (achievements[Index].complete == false)
        {
            if(achievements[Index].chaptersIcons.Length == 0)
            {
                achievements[Index].AddProgress();
            }
            else
            {
                achievements[Index].UpdateProrgeress();
            }

            achievements[Index].Update();

            ES3.Save("Achievements", achievements);

            CheckGoogleAchievements();
        }
    }

    public void UnlockSkin(int index)
    {
        skins[index].unlock = true;
        ES3.Save("Skins", skins);
    }

    public void SaveProducts()
    {
        ES3.Save("Products", products);
    }

    public void CheckGoogleAchievements()
    {
        for (int i = 0; i < achievements.Length; i++)
        {
            if (achievements[i].complete)
            {
                if (Social.localUser.authenticated)
                {
                    Social.ReportProgress(achievements[i].achievementID, 100.0f, (bool success) => { });
                }
            }
        }
    }

    public void SaveSettings()
    {
        float[] Settings = new float[settings.Length];
        for (int i = 0; i < settings.Length; i++)
        {
            Settings[i] = settings[i].value;
        }
        ES3.Save("Settings", Settings);
    }
    public void SaveCurrency()
    {
        ES3.Save("Currency", currency);
    }
    public void SaveLanguage()
    {
        ES3.Save("Language", languageManager);
    }

    public void DeleteAllSaves()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if(skills[i].engType == "Dark")
            {
                skills[i].unlock = false;
            }
        }
        for (int i = 2; i < items.Length; i++)
        {
            items[i].unlock = false;
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].unlock = false;
        }
        for (int i = 2; i < locations.Length; i++)
        {
            locations[i].unlock = false;
        }
        for (int i = 0; i < achievements.Length; i++)
        {
            achievements[i].complete = false;
            achievements[i].currentProgress = 0;
        }
        for (int i = 1; i < skins.Length; i++)
        {
            skins[i].unlock = false;
        }
        for (int i = 0; i < products.Length; i++)
        {
            products[i].ResetBonus();
        }
        for (int i = 0; i < tutorial.CompleteTutorials.Length; i++)
        {
            tutorial.CompleteTutorials[i] = false;
        }

        skins[0].SelectSkin();
        currency.DeletePoints();

        ES3.Save("Skills", skills);
        ES3.Save("Items", items);
        ES3.Save("Enemies", enemies);
        ES3.Save("Locations", locations);
        ES3.Save("Achievements", achievements);
        ES3.Save("Skins", skins);
        ES3.Save("Products", products);
        ES3.Save("Currency", currency);
        ES3.Save("Tutorials", tutorial);
    }

    public void LoadAllSaves()
    {
        if (ES3.KeyExists("Skills"))
        {
            for (int i = 0; i < skills.Length; i++)
            {
                skills[i].maxPoints = ES3.Load<SkillIcon[]>("Skills")[i].maxPoints;
                skills[i].cureentPoints = ES3.Load<SkillIcon[]>("Skills")[i].cureentPoints;
                skills[i].drawType = ES3.Load<SkillIcon[]>("Skills")[i].drawType;
                skills[i].damage = ES3.Load<SkillIcon[]>("Skills")[i].damage;
                skills[i].baseDamage = ES3.Load<SkillIcon[]>("Skills")[i].baseDamage;
                skills[i].effectTime = ES3.Load<SkillIcon[]>("Skills")[i].effectTime;
                skills[i].baseEffectTime = ES3.Load<SkillIcon[]>("Skills")[i].baseEffectTime;
                skills[i].manacost = ES3.Load<SkillIcon[]>("Skills")[i].manacost;
                skills[i].addDamage = ES3.Load<SkillIcon[]>("Skills")[i].addDamage;
                skills[i].addEffectTime = ES3.Load<SkillIcon[]>("Skills")[i].addEffectTime;
                skills[i].open = ES3.Load<SkillIcon[]>("Skills")[i].open;
            }
        }
        if (ES3.KeyExists("Items"))
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].unlock = ES3.Load<ItemIcon[]>("Items")[i].unlock;
            }
        }
        if (ES3.KeyExists("Enemies"))
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].unlock = ES3.Load<EnemyIcon[]>("Enemies")[i].unlock;
            }
        }
        if (ES3.KeyExists("Locations"))
        {
            for (int i = 0; i < locations.Length; i++)
            {
                locations[i].unlock = ES3.Load<LocationIcon[]>("Locations")[i].unlock;
            }
        }
        if (ES3.KeyExists("Achievements"))
        {
            for (int i = 0; i < achievements.Length; i++)
            {
                achievements[i].progress = ES3.Load<Achievements[]>("Achievements")[i].progress;
                achievements[i].currentProgress = ES3.Load<Achievements[]>("Achievements")[i].currentProgress;
                achievements[i].complete = ES3.Load<Achievements[]>("Achievements")[i].complete;
            }
        }
        if (ES3.KeyExists("Tutorials"))
        {
            for (int i = 0; i < tutorial.CompleteTutorials.Length; i++)
            {
                tutorial.CompleteTutorials[i] = ES3.Load<Tutorial>("Tutorials").CompleteTutorials[i];
            }

        }
        if (ES3.KeyExists("Settings"))
        {
            float[] Settings = new float[settings.Length];

            Settings = ES3.Load<float[]>("Settings");

            for (int i = 0; i < settings.Length; i++)
            {
                settings[i].value = Settings[i];
            }
        }
        if (ES3.KeyExists("Currency"))
        {
            currency.airPoints = ES3.Load<CurrencyBase>("Currency").airPoints;
            currency.firePoints = ES3.Load<CurrencyBase>("Currency").firePoints;
            currency.waterPoints = ES3.Load<CurrencyBase>("Currency").waterPoints;
            currency.earthPoints = ES3.Load<CurrencyBase>("Currency").earthPoints;
            currency.darkPoints = ES3.Load<CurrencyBase>("Currency").darkPoints;
        }
        if (ES3.KeyExists("Skins"))
        {
            for (int i = 0; i < skins.Length; i++)
            {
                skins[i].unlock = ES3.Load<Skin[]>("Skins")[i].unlock;
                if (ES3.Load<Skin[]>("Skins")[i].select)
                {
                    skins[i].SelectSkin();
                    break;
                }
            }
        }
        if (ES3.KeyExists("Products"))
        {
            for (int i = 0; i < products.Length; i++)
            {
                products[i].bonus = ES3.Load<Product[]>("Products")[i].bonus;
                products[i].addAddBonus = ES3.Load<Product[]>("Products")[i].addAddBonus;
                products[i].addBonus = ES3.Load<Product[]>("Products")[i].addBonus;
                products[i].addPrice = ES3.Load<Product[]>("Products")[i].addPrice;
                products[i].maxLvl = ES3.Load<Product[]>("Products")[i].maxLvl;
                products[i].currentLvl = ES3.Load<Product[]>("Products")[i].currentLvl;
            }

            for (int i = 0; i < products.Length; i++)
            {
                products[i].LoadBonus();
            }
        }
        if (ES3.KeyExists("Language"))
        {
            languageManager = ES3.Load<LanguageManager>("Language");
            languageManager.SelectLanguage(languageManager.currentLanguage);
        }
    }

    public IEnumerator UlockCoroutine(Image image)
    {
        for (int i = 0; i < 25; i++)
        {
            yield return new WaitForSeconds(0.01f);

            image.color -= new Color32(2, 2, 2, 0);
        }
        for (int i = 0; i < 25; i++)
        {
            yield return new WaitForSeconds(0.01f);

            image.color += new Color32(2, 2, 2, 0);
        }

        StartCoroutine(UlockCoroutine(image));
    }
}

using UnityEngine.UI;
using UnityEngine;

public class Achievements : Icon
{
    [SerializeField] private GameObject achievmentAnimation;

    public string achievementID;
    public float progress, currentProgress;
    public bool complete;

    public bool checkProducts;
    public GameObject[] chaptersIcons;

    private void Awake()
    {
        if (unlockSystem == null)
        {
            unlockSystem = FindObjectOfType<UnlockSystem>();
        }
        icon = transform.GetChild(0).GetComponent<Image>();
    }

    public void Update()
    {
        if (complete)
        {
            transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            progress = currentProgress;
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().color = new Color32(150, 150, 150, 255);
        }

        if(chaptersIcons.Length > 0)
        {
            UpdateProrgeress();
        }
    }

    public void AddProgress()
    {
        currentProgress += 1;

        if(currentProgress >= progress)
        {
            currentProgress = progress;
            complete = true;

            Instantiate(achievmentAnimation, gameObject.transform.parent.parent.parent.parent);

            if (unlockSystem == null)
            {
                unlockSystem = FindObjectOfType<UnlockSystem>();
            }
        }
    }

    public void UpdateProrgeress()
    {
        currentProgress = 0;
        progress = 0;

        for (int i = 0; i < chaptersIcons.Length; i++)
        {
            for (int j = 0; j < chaptersIcons[i].transform.childCount; j++)
            {
                progress += 1;
            }
        }

        for (int i = 0; i < chaptersIcons.Length; i++)
        {
            for (int j = 0; j < chaptersIcons[i].transform.childCount; j++)
            {
                if (checkProducts)
                {
                    if (chaptersIcons[i].transform.GetChild(j).GetComponent<Product>().maxLvl == chaptersIcons[i].transform.GetChild(j).GetComponent<Product>().currentLvl)
                    {
                        if (complete == false)
                        {
                            AddProgress();
                        }
                        else
                        {
                            currentProgress = progress;
                        }
                    }
                }
                else
                {
                    if (chaptersIcons[i].transform.GetChild(j).GetComponent<Icon>().unlock)
                    {
                        if (complete == false)
                        {
                            AddProgress();
                        }
                        else
                        {
                            currentProgress = progress;
                        }
                    }
                }
            }
        }
    }
}
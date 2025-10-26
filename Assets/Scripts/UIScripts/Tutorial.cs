using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject[] tutorialObjects;
    [SerializeField] bool[] completeTutorial;

    private GameObject rightEnemy, leftEnemy;

    public bool[] CompleteTutorials
    {
        get
        {
            return completeTutorial;
        }
        set
        {
            completeTutorial = value;
        }
    }

    public void ShowTutorial(int Index)
    {
        if (!completeTutorial[0] && Index == 0)
        {
            tutorialObjects[0].SetActive(true);
        }
        if (!completeTutorial[1] && Index == 1)
        {
            tutorialObjects[1].SetActive(true);
        }
        if (!completeTutorial[2] && Index == 2)
        {
            if (FindObjectOfType<CompanyManager>().currentStage == 2 && FindObjectsOfType<ScrollAnimations>().Length > 0)
            {
                tutorialObjects[2].SetActive(true);
            }
        }
        if (!completeTutorial[3] && Index == 3)
        {
            if (FindObjectsOfType<ItemIcon>().Length > 0 || FindObjectsOfType<EnemyIcon>().Length > 0 ||
                FindObjectsOfType<LocationIcon>().Length > 0)
            {
                tutorialObjects[3].SetActive(true);
            }
        }
    }

    public void CompleteTutorial(int Index)
    {
        if(completeTutorial[Index] == false)
        {
            if (Index == 2 || Index == 3)
            {
                if (tutorialObjects[2].activeInHierarchy)
                {
                    completeTutorial[Index] = true;
                    tutorialObjects[Index].SetActive(false);
                }
                else if (tutorialObjects[3].activeInHierarchy)
                {
                    completeTutorial[Index] = true;
                    tutorialObjects[Index].SetActive(false);
                }
            }
            else
            {
                completeTutorial[Index] = true;
                tutorialObjects[Index].SetActive(false);
            }
  
            ES3.Save("Tutorials", this);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    private CompanyManager companyManager;

    [SerializeField] private Portal portal;
    [SerializeField] private Location[] locations;
    [SerializeField] private Text difficultyText;
    public Item reward;

    public string difficulty;
    public string type;
    public int stage;

    public bool select;

    private UnlockSystem unlockSystem;

    private void Start()
    {
        companyManager = FindObjectOfType<CompanyManager>();
        unlockSystem = FindObjectOfType<UnlockSystem>();

        difficultyText = transform.GetChild(1).GetComponent<Text>();
        Update();

        if (difficulty == "Easy")
        {
            difficultyText.color = new Color32(0,150,0,255);
        }
        else if(difficulty == "Normal")
        {
            difficultyText.color = new Color32(205, 80, 0, 255);
        }
        else if (difficulty == "Hard")
        {
            difficultyText.color = new Color32(150, 0, 0, 255);
        }
        else if (difficulty == "Boss")
        {
            difficultyText.color = new Color32(255, 0, 0, 255);
        }
    }

    public void StartLevel()
    {
        companyManager.CloseScrolls();

        FindObjectOfType<Tutorial>().CompleteTutorial(2);

        for (int i = 0; i < locations.Length; i++)
        {
            if (i < locations.Length - 1)
            {
                locations[i].portal = portal;
                locations[i].nextLocation = locations[i + 1];
            }
            else
            {
                locations[i].nextLocation = null;
                locations[i].companyManager = companyManager;
            }
        }

        if (reward != null)
        {
            companyManager.currentReward = reward;
        }

        Portal Portal = Instantiate(portal);
        Portal.location = locations[0];

        FindObjectOfType<SoundController>().NextMusic();
    }

    public void StartTown()
    {
        for (int i = 0; i < locations.Length; i++)
        {
            if (i < locations.Length - 1)
            {
                locations[i].portal = portal;
                locations[i].nextLocation = locations[i + 1];
            }
            else
            {
                locations[i].nextLocation = null;
                locations[i].companyManager = companyManager;
            }
        }

        Portal Portal = Instantiate(portal);
        Portal.location = locations[0];
    }

    private void Update()
    {
        if (unlockSystem.languageManager.currentLanguage == Language.English)
        {
            difficultyText.text = "(" + difficulty + ")";
        }
        else if (unlockSystem.languageManager.currentLanguage == Language.Russian)
        {
            if (difficulty == "Easy")
            {
                difficultyText.text = "(Легко)";
            }
            else if (difficulty == "Normal")
            {
                difficultyText.text = "(Нормально)";
            }
            else if (difficulty == "Hard")
            {
                difficultyText.text = "(Сложно)";
            }
            else if (difficulty == "Boss")
            {
                difficultyText.text = "(Босс)";
            }
        }
    }
}
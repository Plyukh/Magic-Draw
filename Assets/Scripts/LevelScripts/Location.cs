using UnityEngine;
using System.Collections;

public class Location : MonoBehaviour
{
    [HideInInspector] public CompanyManager companyManager;
    [HideInInspector] public Location nextLocation;
    public Portal portal;
    private PlayerStats playerStats;
    private Spawner[] spawners;
    private bool pause;
    public bool killAll;

    private float waitSeconds = 1;
    private float currentSeconds;

    private SpellManager spellManager;
    private GameOver gameOver;
    private UnlockSystem unlockSystem;
    private Pause pauseObject;

    public bool Pause
    {
        get
        {
            return pause;
        }
        set
        {
            pause = value;
        }
    }
    public Spawner[] Spawners
    {
        get
        {
            return spawners;
        }
    }

    private void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        spellManager = playerStats.GetComponent<SpellManager>();

        gameOver = FindObjectOfType<GameOver>();
        unlockSystem = FindObjectOfType<UnlockSystem>();

        pauseObject = FindObjectOfType<Pause>();
        pauseObject.location = this;

        foreach (var item in pauseObject.spells)
        {
            if(item != null)
            {
                Destroy(item.gameObject);
            }
        }

        spawners = new Spawner[transform.GetComponentsInChildren<Spawner>().Length];

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetComponentsInChildren<Spawner>()[i];
        }
    }

    private void Update()
    {
        CheckEnemies();
    }

    public void CheckEnemies()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            if (spawners[i].Enemies != 0)
            {
                return;
            }
        }

        if (spellManager.seeEnemy == true)
        {
            return;
        }

        if (!killAll && !pause)
        {
            currentSeconds += Time.deltaTime;
            if(currentSeconds >= waitSeconds)
            {
                Recovery();
                NextLocation();
                killAll = true;
            }
        }
    }

    void NextLocation()
    {
        if (nextLocation != null)
        {
            portal = Instantiate(portal);
            portal.location = nextLocation;
        }
        else if (companyManager.currentReward != null)
        {
            companyManager.rewardAnimations.gameObject.SetActive(true);
            companyManager.rewardAnimations.TakeReward(companyManager.currentReward);

            unlockSystem.UnlockItem(companyManager.currentReward);

            companyManager.currentReward = null;
        }
        else if (companyManager.currentStage + 1 == 26)
        {
            portal = Instantiate(gameOver.StartLocation.portal);
            portal.location = gameOver.StartLocation;
            gameOver.Win();
        }
        else
        {
            companyManager.OpenScrolls();
        }
    }

    void Recovery()
    {
        playerStats.ApplyMagicRecoveryEffect(4);

        AmuletItems amulet = FindObjectOfType<AmuletItems>();
        if (amulet != null && amulet.regeneration)
        {
            playerStats.ApplyHealEffect(4);
        }
    }
}

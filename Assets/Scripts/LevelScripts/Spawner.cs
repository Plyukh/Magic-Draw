using UnityEngine;

public class Spawner : MonoBehaviour
{
    private PlayerStats player;
    private Pause pauseObject;

    [SerializeField] private Enemy[] enemies;
    [SerializeField] private float[] timers;

    private bool pause;
    public bool tutorial;
    public bool randomSpawn;

    private Enemy tutorialEnemy;

    private Tutorial tutorialObject;

    private bool checkTutorial;

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
    public bool Tutorial
    {
        get
        {
            return tutorial;
        }
        set
        {
            tutorial = value;
        }
    }
    public int Enemies
    {
        get
        {
            return enemies.Length;
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        pauseObject = FindObjectOfType<Pause>();
        pauseObject.spawners.Add(this);

        if (tutorial)
        {
            tutorialObject = FindObjectOfType<Tutorial>();
        }
        if (player.Chapter.gameObject.activeInHierarchy)
        {
            Pause = true;
        }
    }

    private void Update()
    {
        if (!pause && !checkTutorial)
        {
            if (randomSpawn)
            {
                if (timers[0] > 0)
                {
                    timers[0] -= Time.deltaTime;
                    if (timers[0] <= 0)
                    {
                        RandomSpawn();
                    }
                }
            }
            else
            {
                for (int i = 0; i < timers.Length; i++)
                {
                    if (timers[i] > 0)
                    {
                        timers[i] -= Time.deltaTime;
                        if (timers[i] <= 0)
                        {
                            Spawn();
                        }
                        break;
                    }
                }
            }
        }

        if (tutorial && tutorialEnemy != null)
        {
            if (tutorialEnemy.transform.position.x < 0 && tutorialEnemy.transform.position.x >= -80)
            {
                if (!tutorialObject.CompleteTutorials[1])
                {
                    tutorialObject.ShowTutorial(1);
                    tutorialEnemy.Pause = true;
                    checkTutorial = true;
                    tutorial = false;
                }
            }
            else if(tutorialEnemy.transform.position.x > 0 && tutorialEnemy.transform.position.x <= 80)
            {
                if (!tutorialObject.CompleteTutorials[1])
                {
                    tutorialObject.ShowTutorial(0);
                    tutorialEnemy.Pause = true;
                    checkTutorial = true;
                    tutorial = false;
                }
            }
        }
        if (checkTutorial)
        {
            if(tutorialEnemy == null)
            {
                checkTutorial = false;
            }
        }
    }

    public void Spawn()
    {
        FindObjectOfType<SpellManager>().seeEnemy = true;
        Enemy enemy = Instantiate(enemies[0], gameObject.transform.position, transform.gameObject.transform.rotation, transform.parent);

        if (tutorial)
        {
            tutorialEnemy = enemy;
        }

        Enemy[] newMas = new Enemy[enemies.Length - 1];
        for (int j = 0; j < newMas.Length; j++)
        {
            newMas[j] = enemies[j + 1];
        }
        enemies = newMas;
    }
    public void RandomSpawn()
    {
        int random = Random.Range(0, enemies.Length);

        Instantiate(enemies[random], gameObject.transform.position, transform.gameObject.transform.rotation, transform.parent);
    }
}

using UnityEngine;

public class AmuletItems : Item
{
    private PlayerStats playerStats;
    public bool percentMurders;
    public float manaBonus;
    public float healthBonus;

    [SerializeField] GameObject angel;
    public bool reincarnation;
    public bool regeneration;

    private void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    private void Start()
    {
        if (!percentMurders && !regeneration)
        {
            playerStats.AddBonus(healthBonus, manaBonus);
        }
    }

    public override void RemoveBonus()
    {
        if (!percentMurders && !regeneration)
        {
            healthBonus = -healthBonus;
            manaBonus = -manaBonus;
            playerStats.AddBonus(healthBonus, manaBonus);
        }
    }

    public void Reincarnation()
    {
        Instantiate(angel, new Vector3(0, 125, 0), angel.transform.rotation);
        Destroy(gameObject, 1);
    }
}

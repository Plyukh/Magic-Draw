using UnityEngine;

public enum Elements
{
    Earth,
    Water,
    Fire,
    Air,
    All
}

public class Spell : Move
{
    [SerializeField] protected Elements type;
    [SerializeField] protected string id;
    public int manacost;
    public int currentManacost;
    public int damage;
    public float effectTime;
    public string direction;
    public bool findEnemy;

    [SerializeField] protected GameObject destroyEffect;
    protected Pause pauseObject;

    public string Id
    {
        get
        {
            return id;
        }
    }
    public Elements Type
    {
        get
        {
            return type;
        }
    }
    public Vector3 TargetPosition
    {
        get
        {
            return targetPosition;
        }
        set
        {
            targetPosition = value;
        }
    }

    private void Start()
    {
        pauseObject = FindObjectOfType<Pause>();
        pauseObject.spells.Add(this);

        if (findEnemy)
        {
            FindEnemy();
        }
        SpeedReset();
        LookAt2D();
        ChangeManacost(false);
    }

    private void Update()
    {
        Moving();

        if (transform.position.x >= 170 || transform.position.x <= -170 ||
            transform.position.y >= 80 || transform.position.y <= -80)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeManacost(bool OneCost)
    {
        if (OneCost)
        {
            currentManacost = 0;
        }
        else
        {
            currentManacost = manacost;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>())
        {
            if(collision.GetComponent<EnemyHealth>().Miss == false)
            {
                if (!collision.GetComponent<Projectile>())
                {
                    collision.GetComponent<Effect>().TakeSpell(effectTime, type, damage);
                    collision.GetComponent<EnemyHealth>().ApplyDamage(type, ref damage);
                }
                else if(collision.GetComponent<Projectile>() && 
                    collision.GetComponent<EnemyHealth>().CounterElemental == type || collision.GetComponent<EnemyHealth>().CounterElemental == Elements.All)
                {
                    collision.GetComponent<EnemyHealth>().Death();
                }

                DestroySpell();
            }
            else
            {
                collision.GetComponent<EnemyHealth>().ApplyDamage(type, ref damage);
            }
        }
    }

    virtual protected void DestroySpell()
    {
        GameObject effect = Instantiate(destroyEffect, gameObject.transform.GetChild(0).transform.position, gameObject.transform.GetChild(0).transform.rotation);
        effect.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
        Destroy(gameObject);
    }

    protected virtual void FindEnemy()
    {
        Enemy[] enemies = new Enemy[GameObject.FindGameObjectsWithTag(direction + "Enemy").Length];
        for (int i = 0; i < GameObject.FindGameObjectsWithTag(direction + "Enemy").Length; i++)
        {
            enemies[i] = GameObject.FindGameObjectsWithTag(direction + "Enemy")[i].GetComponent<Enemy>();
        }
        if(enemies.Length > 0)
        {
            float minDistance = Vector3.Distance(gameObject.transform.position, enemies[0].transform.position);
            for (int i = 0; i < enemies.Length; i++)
            {
                float distance = Vector3.Distance(gameObject.transform.position, enemies[i].transform.position);
                if (distance <= minDistance)
                {
                    minDistance = distance;
                    target = enemies[i].gameObject;
                    targetPosition = target.GetComponent<Collider2D>().bounds.center;
                }
            }
        }
        else
        {
            if(direction == "Right")
            {
                targetPosition = new Vector3(20000, 0,0);
            }
            else
            {
                targetPosition = new Vector3(-20000, 0, 0);
            }
        }
    }

    protected void ActiveColliders()
    {
        transform.GetComponent<Collider2D>().enabled = true;
    }

    protected void DeactivateColliders()
    {
        transform.GetComponent<Collider2D>().enabled = false;
    }

    protected void LookAt2D(Vector3? eye = null)
    {
        float signedAngle = Vector2.SignedAngle(eye ?? transform.right, targetPosition - transform.position);

        if (Mathf.Abs(signedAngle) >= 1e-3f)
        {
            var angles = transform.eulerAngles;
            angles.z += signedAngle;
            transform.eulerAngles = angles;
        }
    }
}
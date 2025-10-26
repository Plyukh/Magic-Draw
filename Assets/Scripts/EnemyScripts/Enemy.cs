using UnityEngine;

public class Enemy : Move
{
    protected Animator animator;
    public double XP;
    public double minCurrency;
    public double maxCurrency;
    public int currentCurrency;

    [SerializeField] protected bool stun;
    [SerializeField] protected bool fly;
    [SerializeField] protected bool spawn;
    private bool firstRotate;

    private Pause pauseObject;

    public bool Fly
    {
        get
        {
            return fly;
        }
    }

    private void Awake()
    {
        if (GetComponent<Animator>())
        {
            animator = GetComponent<Animator>();
        }
    }

    protected void Start()
    {
        pauseObject = FindObjectOfType<Pause>();
        pauseObject.enemies.Add(this);

        SpeedReset();
        Rotate();
        target = GameObject.FindGameObjectWithTag("Player").gameObject;
        targetPosition = target.GetComponent<Collider2D>().bounds.center;
        XP = GetComponent<Health>().MaxHP / 10;
        XP = System.Math.Round(XP, 0, System.MidpointRounding.AwayFromZero);
        maxCurrency = GetComponent<Health>().MaxHP / 2;
        maxCurrency = System.Math.Round(maxCurrency, 0, System.MidpointRounding.AwayFromZero);
        minCurrency = maxCurrency / 2;
        minCurrency = System.Math.Round(minCurrency, 0, System.MidpointRounding.AwayFromZero);
        currentCurrency = Random.Range((int)minCurrency, (int)maxCurrency + 1);
    }

    private void Update()
    {
        OnBecameVisible();

        if (!spawn)
        {
            Moving();
        }

        if (pause)
        {
            stun = true;
        }
        else
        {
            stun = false;
        }
    }

    protected override void Moving()
    {
        if (!stun)
        {
            ChangeSpeedAnimation((1 / (moveSpeed / currentSpeed)));
            if (fly)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPosition.x, transform.position.y), currentSpeed * Time.deltaTime);
            }
        }
        else
        {
            ChangeSpeedAnimation(0);
        }
    }
    protected virtual void Rotate()
    {
        float[] StertPosEffects = new float[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            StertPosEffects[i] = transform.GetChild(i).transform.localPosition.x;
        }

        if (!firstRotate)
        {
            if (transform.position.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).transform.localPosition = new Vector3(-StertPosEffects[i], transform.GetChild(i).transform.localPosition.y, transform.GetChild(i).transform.localPosition.z);
                }
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            firstRotate = true;
        }
        else
        {
            if (transform.position.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).transform.localPosition = new Vector3(-StertPosEffects[i], transform.GetChild(i).transform.localPosition.y, transform.GetChild(i).transform.localPosition.z);
            }
        }
    }
    public void ChangeSpeed(bool Stun)
    {
        stun = Stun;
    }
    public virtual void ChangeSpeed(float FreezeSpeed)
    {
        float PercentageSlowdown = 1 / (moveSpeed / FreezeSpeed);
        ChangeSpeedAnimation(1 - PercentageSlowdown);
        if (moveSpeed != 0)
        {
            if (moveSpeed - FreezeSpeed > 1)
            {
                if (currentSpeed == moveSpeed)
                {
                    currentSpeed -= FreezeSpeed;
                }
            }
            else
            {
                currentSpeed = 1;
            }
        }
    }
    public void ChangeSpeedAnimation(float Value)
    {
        if(animator != null)
        {
            animator.SetFloat("Speed", Value);
        }
    }

    protected virtual void OnBecameVisible()
    {
        if (!spawn)
        {
            if (transform.position.x < 0)
            {
                tag = "LeftEnemy";
            }
            else if(transform.position.x > 0)
            {
                tag = "RightEnemy";
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            EnemyHealth health = GetComponent<EnemyHealth>();

            collision.GetComponent<PlayerAnimations>().Hit();
            collision.GetComponent<PlayerStats>().ApplyDamage(health.CurrentHP);

            if(tag == "LeftEnemy")
            {
                collision.GetComponent<PlayerAnimations>().TurntoSide("Left");
            }
            else
            {
                collision.GetComponent<PlayerAnimations>().TurntoSide("Right");
            }

            health.DeathOnPlayer();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Platform")
        {
            gameObject.transform.position += new Vector3(0, 0.1f, 0);
        }
    }

    public void Spawn()
    {
        GetComponent<Collider2D>().enabled = true;
        spawn = false;
    }
}
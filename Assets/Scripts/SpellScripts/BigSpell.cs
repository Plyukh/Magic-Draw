using UnityEngine;

public class BigSpell : Spell
{
    [SerializeField] private float y;
    [SerializeField] private float spellTime;
    private float currentTime;

    private void Start()
    {
        if (findEnemy)
        {
            FindEnemy();
        }
        SpeedReset();
        transform.position = new Vector3(transform.position.x, y);
    }

    private void Update()
    {
        Moving();
    }

    protected override void Moving()
    {
        if (!Pause)
        {
            currentTime += Time.deltaTime;
            DestroySpell(currentTime, spellTime);

            if (targetPosition.x > 0)
            {
                rb.linearVelocity = transform.right * moveSpeed;
            }
            else
            {
                rb.linearVelocity = transform.right * -moveSpeed;
            }
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
            GetComponent<Animator>().SetFloat("Speed", 1);
        }
        else
        {
            rb.linearVelocity = transform.right * 0;
            GetComponent<Animator>().SetFloat("Speed", 0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            if(collision.GetComponent<EnemyHealth>().Miss == false)
            {
                if (!collision.GetComponent<Projectile>())
                {
                    collision.GetComponent<Effect>().TakeSpell(effectTime, type, damage);
                    collision.GetComponent<EnemyHealth>().ApplyBigDamage(damage);
                }
                else if (collision.GetComponent<Projectile>() &&
                    collision.GetComponent<EnemyHealth>().CounterElemental == type || collision.GetComponent<EnemyHealth>().CounterElemental == Elements.All)
                {
                    collision.GetComponent<EnemyHealth>().Death();
                }
            }
        }
    }

    protected void DestroySpell(float currentTime, float spellTime)
    {
        if (currentTime >= spellTime)
        {
            Instantiate(destroyEffect, gameObject.transform.GetChild(0).transform.position, gameObject.transform.GetChild(0).transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}

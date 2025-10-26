using UnityEngine;

public class SummonSpell : Spell
{
    public float currentHP;
    public bool spawn;
    public Vector2 rightSpawnPosition;
    public Vector2 leftSpawnPosition;

    private void Start()
    {
        currentHP = damage;

        pauseObject = FindObjectOfType<Pause>();
        pauseObject.spells.Add(this);

        if (direction == "Left")
        {
            transform.position = leftSpawnPosition;
            moveSpeed = -moveSpeed;
        }
        else
        {
            transform.position = rightSpawnPosition;
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void Update()
    {
        if (spawn == false)
        {
            Moving();
        }

        if (transform.position.x >= 170 || transform.position.x <= -170)
        {
            Destroy(gameObject);
        }
    }

    public void SpawnCreature()
    {
        GetComponent<Collider2D>().enabled = true;
        spawn = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            if (collision.GetComponent<EnemyHealth>().Miss == false)
            {
                if (!collision.GetComponent<Projectile>())
                {
                    currentHP -= collision.GetComponent<EnemyHealth>().CurrentHP;
                    collision.GetComponent<Effect>().TakeSpell(effectTime, type, damage);
                    collision.GetComponent<EnemyHealth>().ApplyDamage(type, ref damage);
                }
                else if (collision.GetComponent<Projectile>() &&
                    collision.GetComponent<EnemyHealth>().CounterElemental == type || collision.GetComponent<EnemyHealth>().CounterElemental == Elements.All)
                {
                    collision.GetComponent<EnemyHealth>().Death();
                }
            }
            else
            {
                currentHP -= collision.GetComponent<EnemyHealth>().CurrentHP;
                collision.GetComponent<EnemyHealth>().ApplyDamage(type, ref damage);
            }

            if(currentHP <= 0)
            {
                DestroySpell();
            }
        }
    }
}

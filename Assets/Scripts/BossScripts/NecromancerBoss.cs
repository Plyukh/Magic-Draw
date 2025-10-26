using UnityEngine;

public class NecromancerBoss : Enemy
{
    [SerializeField] private float damage;
    [SerializeField] private float attackMoveSpeed;
    [SerializeField] private Vector2 rightPosition, leftPosition;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Vector2 spawnPosition;
    [SerializeField] private GameObject trailsEffect;

    [SerializeField] private bool attack;

    [SerializeField] private Color ghostColor;
    [SerializeField] private Color baseColor;

    private void Update()
    {
        OnBecameVisible();

        if (!stun)
        {
            ChangeSpeedAnimation((1 / (moveSpeed / currentSpeed)));

            if (attack)
            {
                Attack();
            }
        }
        else
        {
            ChangeSpeedAnimation(0);
        }

        if (Pause)
        {
            stun = true;
        }
        else
        {
            stun = false;
        }
    }

    void Attack()
    {
        animator.SetBool("Move", true);
        attack = true;

        if (GetComponent<SpriteRenderer>().flipX == true)
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, rightPosition, currentSpeed * Time.deltaTime);

            if (transform.position.x >= -60)
            {
                animator.SetBool("Move", false);
                trailsEffect.SetActive(true);
                Ghost(true);

                currentSpeed = attackMoveSpeed;
            }
            if (transform.position.x >= rightPosition.x)
            {
                attack = false;
                SpeedReset();
                Ghost(false);
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, leftPosition, currentSpeed * Time.deltaTime);

            if (transform.position.x <= 60)
            {
                animator.SetBool("Move", false);
                trailsEffect.SetActive(true);
                Ghost(true);

                currentSpeed = attackMoveSpeed;
            }
            if (transform.position.x <= leftPosition.x)
            {
                attack = false;
                SpeedReset();
                Ghost(false);
            }
        }
    }

    private void Ghost(bool active)
    {
        GetComponent<EnemyHealth>().Miss = active;

        if (!active)
        {
            GetComponent<SpriteRenderer>().color = ghostColor;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = baseColor;
        }
    }

    void SpawnEnemy()
    {
        if(GetComponent<SpriteRenderer>().flipX == false)
        {
            Instantiate(enemy, new Vector3(spawnPosition.x, spawnPosition.y), enemy.transform.rotation, gameObject.transform.parent);
        }
        else
        {
            Instantiate(enemy, new Vector3(-spawnPosition.x, spawnPosition.y), enemy.transform.rotation, gameObject.transform.parent);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (tag == "LeftEnemy")
            {
                collision.GetComponent<PlayerAnimations>().TurntoSide("Left");
            }
            else
            {
                collision.GetComponent<PlayerAnimations>().TurntoSide("Right");
            }
            collision.GetComponent<PlayerAnimations>().Hit();
            GetComponent<EnemyHealth>().CanvasEffectManager.SpawnMissEffect(gameObject.transform.position);
            target.GetComponent<PlayerStats>().ApplyDamage(damage, true);
        }
    }
}
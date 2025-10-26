using UnityEngine;

public class GhostWolfBoss : Enemy
{
    [SerializeField] private float damage;
    [SerializeField] private Vector2 rightPosition, leftPosition;
    [SerializeField] private bool attack;
    [SerializeField] private bool firstAttack;

    [SerializeField] private Color ghostColor;
    [SerializeField] private Color baseColor;
    [SerializeField] private float colorSpeed;

    private void Update()
    {
        OnBecameVisible();

        if (!stun)
        {
            ChangeSpeedAnimation((1 / (moveSpeed / currentSpeed)));

            Ghost();

            if (attack)
            {
                AttackMove();
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

    void AttackMove()
    {
        animator.SetBool("Move", true);
        attack = true;

        if (firstAttack)
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, leftPosition, currentSpeed * Time.deltaTime);

            if(currentSpeed < 100)
            {
                currentSpeed += 0.5f;
            }

            if(transform.position.x <= -100)
            {
                animator.SetBool("Move", false);
                attack = false;
                firstAttack = false;

                currentSpeed = moveSpeed;
            }
        }
        else
        {
            if (GetComponent<SpriteRenderer>().flipX == true)
            {
                transform.position = Vector2.MoveTowards(gameObject.transform.position, rightPosition, currentSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(gameObject.transform.position, leftPosition, currentSpeed * Time.deltaTime);
            }

            if (currentSpeed < 75)
            {
                currentSpeed += 0.5f;
            }

            if (transform.position.x >= 100 || transform.position.x <= -100)
            {
                animator.SetBool("Move", false);
                attack = false;

                currentSpeed = moveSpeed;
            }
        }
    }

    private void Ghost()
    {
        if (GetComponent<SpriteRenderer>().color.a < 0.95f)
        {
            GetComponent<EnemyHealth>().Miss = true;
        }
        else
        {
            GetComponent<EnemyHealth>().Miss = false;
        }

        if (transform.position.x <= 60 && transform.position.x > -45)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, ghostColor, colorSpeed * Time.deltaTime);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, baseColor, colorSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
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

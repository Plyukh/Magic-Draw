using UnityEngine;

public class DemonBoss : Enemy
{
    [SerializeField] private float damage;
    [SerializeField] private Vector2 rightPosition, leftPosition;

    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject breathFireSound;
    private bool canHit;

    private void Update()
    {
        OnBecameVisible();

        if (!stun)
        {
            ChangeSpeedAnimation((1 / (moveSpeed / currentSpeed)));

            if (fly)
            {
                FlyToPosition();
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

    public void FlyToPosition()
    {
        fly = true;
        animator.SetBool("Fly", true);

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

        if (transform.position.x == 65 || transform.position.x == -65)
        {
            fly = false;
            animator.SetBool("Fly", false);

            Rotate();
            currentSpeed = moveSpeed;
        }
    }

    public void Breath()
    {
        if (GetComponent<SpriteRenderer>().flipX)
        {
            Instantiate(projectile, new Vector3(-35, 15, -1), projectile.transform.rotation, transform.parent.parent.parent);
        }
        else
        {
            Instantiate(projectile, new Vector3(35, 15, -1), projectile.transform.rotation, transform.parent.parent.parent);
        }

        Instantiate(breathFireSound, transform.position, projectile.transform.rotation, transform.parent.parent.parent);

        canHit = false;
    }

    public void CanHit()
    {
        canHit = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canHit)
        {
            animator.SetTrigger("Hit");
            fly = true;
            canHit = false;
        }

        if (collision.tag == "Player")
        {
            EnemyHealth health = GetComponent<EnemyHealth>();

            collision.GetComponent<PlayerAnimations>().Hit();
            collision.GetComponent<PlayerStats>().ApplyDamage(health.CurrentHP);

            if (tag == "LeftEnemy")
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
}
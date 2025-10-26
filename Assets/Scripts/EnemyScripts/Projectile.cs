using UnityEngine;

public class Projectile : Enemy
{
    [SerializeField] private float damage;
    [SerializeField] private bool destroy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth health = GetComponent<EnemyHealth>();

        if (collision.tag == "Player")
        {
            if (transform.position.x < 0)
            {
                collision.GetComponent<PlayerAnimations>().TurntoSide("Left");
            }
            else
            {
                collision.GetComponent<PlayerAnimations>().TurntoSide("Right");
            }

            if (destroy)
            {
                collision.GetComponent<PlayerAnimations>().Hit();

                collision.GetComponent<PlayerStats>().ApplyDamage(damage - health.CurrentHP);
                health.DeathOnPlayer();
            }
            else
            {
                collision.GetComponent<PlayerStats>().ApplyDamage(damage - health.CurrentHP, true, false);
                collision.GetComponent<PlayerAnimations>().Pain();
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    protected override void OnBecameVisible()
    {

    }
}

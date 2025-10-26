using UnityEngine;

public class WizardBoss : Enemy
{
    [SerializeField] private Vector2 rightPosition, leftPosition;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float damageThreshold;
    [SerializeField] private float currentDamageThreshold;
    private bool teleport;

    private void Update()
    {
        OnBecameVisible();

        if (!stun)
        {
            ChangeSpeedAnimation((1 / (moveSpeed / currentSpeed)));

            if (GetComponent<EnemyHealth>().CurrentHP <= currentDamageThreshold && !teleport)
            {
                Teleport();
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

    public void Teleport()
    {
        teleport = true;
        animator.SetBool("Teleport", true);
        GetComponent<Collider2D>().enabled = false;
        currentDamageThreshold -= damageThreshold;
        GetComponent<AudioSource>().Play();
    }

    public void EndTeleport()
    {
        teleport = false;
        animator.SetBool("Teleport", false);
        if(transform.position.x == rightPosition.x)
        {
            transform.position = leftPosition;
        }
        else
        {
            transform.position = rightPosition;
        }
        Rotate();
    }

    public void Cast()
    {
        Instantiate(projectile.gameObject, transform.position, projectile.transform.rotation, transform.parent.parent.parent);
    }
}
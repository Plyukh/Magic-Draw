using UnityEngine;

public class FireballSpell : Spell
{
    [SerializeField] float radius;
    private Enemy enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            enemy = collision.GetComponent<Enemy>();
            
            if (collision.GetComponent<EnemyHealth>().Miss == false)
            {
                if (!collision.GetComponent<Projectile>())
                {
                    collision.GetComponent<Effect>().TakeSpell(effectTime, type, damage);
                    collision.GetComponent<EnemyHealth>().ApplyDamage(type, ref damage);
                }
                else if (collision.GetComponent<Projectile>() &&
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

    protected override void DestroySpell()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius * radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Enemy>() && hitCollider.GetComponent<Enemy>() != enemy)
            {
                if (hitCollider.GetComponent<EnemyHealth>().Miss == false)
                {
                    if (!hitCollider.GetComponent<Projectile>())
                    {
                        hitCollider.GetComponent<Effect>().TakeSpell(effectTime, type, damage);
                        hitCollider.GetComponent<EnemyHealth>().ApplyDamage(type, ref damage);
                    }
                    else if (hitCollider.GetComponent<Projectile>() &&
                        hitCollider.GetComponent<EnemyHealth>().CounterElemental == type || hitCollider.GetComponent<EnemyHealth>().CounterElemental == Elements.All)
                    {
                        hitCollider.GetComponent<EnemyHealth>().Death();
                    }
                }
            }
        }
        base.DestroySpell();
    }
}
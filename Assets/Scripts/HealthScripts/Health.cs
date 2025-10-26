using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [SerializeField] protected float maxHP;
    [SerializeField] protected float currentHP;

    public float CurrentHP
    {
        get
        {
            return currentHP;
        }
    }
    public float MaxHP
    {
        get
        {
            return maxHP;
        }
    }

    public virtual void ApplyDamage(float Damage)
    {
        if(Damage > 0)
        {
            StartCoroutine(ApplyDamageCoroutine());
            Damage = Mathf.Round(Damage);
            currentHP -= Damage;
            if (currentHP <= 0)
            {
                Death();
            }
        }
    }
    public virtual void ApplyHeal(float Heal)
    {
        currentHP += Heal;
        if(currentHP >= maxHP)
        {
            currentHP = maxHP;
        }
    }

    public virtual void Death()
    {

    }

    protected IEnumerator ApplyDamageCoroutine()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.01f);
            gameObject.transform.localScale += new Vector3(0.5f, 0.5f);
            if(gameObject.GetComponent<SpriteRenderer>() != null)
            {
                gameObject.GetComponent<SpriteRenderer>().color -= new Color32(0, 10, 10, 0);
            }
        }
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.01f);
            gameObject.transform.localScale -= new Vector3(0.5f, 0.5f);
            if(gameObject.GetComponent<SpriteRenderer>() != null)
            {
                gameObject.GetComponent<SpriteRenderer>().color += new Color32(0, 10, 10, 0);
            }
        }
        StopCoroutine(ApplyDamageCoroutine());
    }
}

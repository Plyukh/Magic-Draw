using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TurntoSide(string Direction)
    {
        if (Direction == "Left")
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public void CastAnimation(string Direction)
    {
        animator.SetTrigger("Cast");
        TurntoSide(Direction);
    }

    public void TeleportAnimation()
    {
        animator.SetTrigger("Teleport");
    }
    public void TeleporEndtAnimation()
    {
        animator.SetTrigger("Teleport End");
    }

    public void Hit()
    {
        animator.SetTrigger("Hit");
    }
    public void Pain()
    {
        animator.SetTrigger("Pain");
    }
    public void Dead()
    {
        animator.SetTrigger("Dead");
    }
    public void Reincarnation(bool Value)
    {
        animator.SetTrigger("Reincarnation");
    }
    public void GameOverAnimation()
    {
        animator.SetTrigger("GameOver");
    }
}

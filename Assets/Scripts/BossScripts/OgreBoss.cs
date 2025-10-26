using UnityEngine;

public class OgreBoss : Enemy
{
    [SerializeField] private Vector2 lowerRight, upperRight, lowerLeft, upperLeft;
    [SerializeField] private GameObject projectile;
    [SerializeField] private bool fall, jump;

    private void Update()
    {
        OnBecameVisible();

        if (!stun)
        {
            ChangeSpeedAnimation((1 / (moveSpeed / currentSpeed)));
            if (jump)
            {
                Jump();
            }
            if (fall)
            {
                Fall();
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

    public void Jump()
    {
        animator.SetBool("Jump", true);
        jump = true;

        if (GetComponent<SpriteRenderer>().flipX == true)
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, upperLeft, currentSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, upperRight, currentSpeed * Time.deltaTime);
        }

        if (transform.position.y >= upperRight.y)
        {
            animator.SetBool("Jump", false);
            TakeProjectile();

            jump = false;
        }
    }
    public void Fall()
    {
        currentSpeed = moveSpeed;
        animator.SetBool("Fall", true);
        fall = true;

        if(GetComponent<SpriteRenderer>().flipX == true)
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, lowerLeft, currentSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, lowerRight, currentSpeed * Time.deltaTime);
        }

        currentSpeed += currentSpeed / 100;
        if (transform.position.y <= lowerRight.y)
        {
            animator.SetBool("Fall", false);
            fall = false;

            currentSpeed = moveSpeed;
        }
    }

    public void Throw()
    {
        Instantiate(projectile.gameObject, transform.position, projectile.transform.rotation, transform.parent.parent.parent);
    }
    public void TakeProjectile()
    {
        if(GetComponent<SpriteRenderer>().flipX == true)
        {
            transform.position = upperRight;
        }
        else
        {
            transform.position = upperLeft;
        }
        Rotate();
        fall = true;
    }

    public void ShakeCamera()
    {
        Camera.main.GetComponent<CameraShake>().Shake(10);
        GetComponent<AudioSource>().Play();
    }
}

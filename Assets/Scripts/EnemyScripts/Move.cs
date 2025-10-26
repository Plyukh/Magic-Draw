using UnityEngine;

public class Move : MonoBehaviour
{
    protected Rigidbody2D rb;
    [SerializeField] protected GameObject target;
    protected Vector3 targetPosition;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float currentSpeed;
    protected bool pause;

    public bool Pause
    {
        get
        {
            return pause;
        }
        set
        {
            pause = value;
        }
    }
    public GameObject Target
    {
        get
        {
            return target;
        }
    }
    public float Speed
    {
        get
        {
            return moveSpeed;
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Moving();
    }

    protected virtual void Moving()
    {
        if (!pause)
        {
            if (rb != null)
            {
                rb.linearVelocity = transform.right * moveSpeed;
            }
            if(GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().SetFloat("Speed", 1);
            }
            else
            {
                transform.GetChild(0).GetComponent<Animator>().SetFloat("Speed", 1);
            }
        }
        else
        {
            rb.linearVelocity = transform.right * 0;
            if(GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().SetFloat("Speed", 0);
            }
            else
            {
                transform.GetChild(0).GetComponent<Animator>().SetFloat("Speed", 1);
            }
        }
    }
    public virtual void SpeedReset()
    {
        currentSpeed = moveSpeed;
    }
}

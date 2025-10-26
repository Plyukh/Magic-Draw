using UnityEngine;

public class AnimationAutoDestroy : MonoBehaviour
{
    public float delay = 0f;
    public bool hide;
    public bool audioSource;

    void Start()
    {
        if (!hide)
        {
            if (audioSource)
            {
                Destroy(gameObject, delay);
            }
            else
            {
                Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
            }
        }
    }

    private void Update()
    {
        if (hide)
        {
            delay -= Time.deltaTime;
            if(delay <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Hide()
    {
        hide = true;
    }
}

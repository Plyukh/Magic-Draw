using UnityEngine;

public class AnimationHide : MonoBehaviour
{
    public float delay;
    [SerializeField] private float time;
    public bool hide;

    public void Start()
    {
        time = delay;
        hide = false;
    }

    private void Update()
    {
        if (hide)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                time = delay;
                hide = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void Hide()
    {
        hide = true;
    }
}
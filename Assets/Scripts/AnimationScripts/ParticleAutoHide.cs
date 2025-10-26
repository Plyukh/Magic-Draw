using UnityEngine;

public class ParticleAutoHide : MonoBehaviour
{
    public float delay;

    [System.Obsolete]
    void Start()
    {
        Destroy(gameObject, GetComponent<ParticleSystem>().startLifetime);
    }
}

using UnityEngine;

public class PlayerBoss : Enemy
{
    [SerializeField] Vector2 startPosition;
    [SerializeField] private GameObject projectile;

    [SerializeField] private AudioSource portalAudio;
    [SerializeField] private AudioSource playerPortalAudio;
    [SerializeField] private AudioClip openPortalClip, closePortalClip;

    [HideInInspector] public Spawner spawner;

    private void Start()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        if (player.Chapter.gameObject.activeInHierarchy)
        {
            pause = true;
        }
        spawner = FindObjectOfType<Spawner>();
        transform.position = startPosition;
        base.Start();
    }

    private void Update()
    {
        OnBecameVisible();

        if (!stun)
        {
            ChangeSpeedAnimation((1 / (moveSpeed / currentSpeed)));
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

    protected override void Rotate()
    {

    }

    public void Cast()
    {
        Instantiate(projectile.gameObject, transform.position, projectile.transform.rotation, transform.parent);
    }

    public void OpenSound()
    {
        portalAudio.clip = openPortalClip;
        portalAudio.Play();
    }
    public void CloseSound()
    {
        portalAudio.clip = closePortalClip;
        portalAudio.Play();
    }
    public void PlayerTeleport()
    {
        playerPortalAudio.Play();
    }
}

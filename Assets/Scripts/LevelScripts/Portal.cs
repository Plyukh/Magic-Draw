using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject background;
    private PlayerAnimations player;
    public Location currentLocation;
    public Location location;

    AudioSource audioSource;
    AudioSource AudioSourceTeleport;
    [SerializeField] private AudioClip openPortalClip, closePortalClip; 

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimations>();
        audioSource = GetComponent<AudioSource>();
        AudioSourceTeleport = transform.GetChild(0).GetComponent<AudioSource>();
    }

    private void Start()
    {
        currentLocation = FindObjectOfType<Location>();
        player.TeleportAnimation();
    }

    public void LoadLocation()
    {
        Destroy(currentLocation.gameObject);
        Instantiate(location);
        CloseAnimation();
    }

    public void CloseAnimation()
    {
        GetComponent<Animator>().SetTrigger("Close");
        player.TeleporEndtAnimation();
    }
    public void BackgroundClose()
    {
        background.GetComponent<Animator>().SetTrigger("Close");
    }
    public void BackgroundOpen()
    {
        background = Instantiate(background, GameObject.Find("Canvas").transform);
    }

    public void OpenSound()
    {
        audioSource.clip = openPortalClip;
        audioSource.Play();
    }
    public void CloseSound()
    {
        audioSource.clip = closePortalClip;
        audioSource.Play();
    }
    public void PlayerTeleport()
    {
        AudioSourceTeleport.Play();
    }
}

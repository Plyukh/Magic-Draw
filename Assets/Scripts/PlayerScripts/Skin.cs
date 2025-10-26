using UnityEngine;
using UnityEngine.UI;

public class Skin : MonoBehaviour
{
    [SerializeField] private PlayerAnimations player;
    [SerializeField] private UnlockSystem unlockSystem;

    public Image icon;
    public RuntimeAnimatorController animator;
    public GameObject destroyEffect;
    public Button selectButton;

    public bool unlock;
    public bool select;

    public void UpdateLoked()
    {
        if (!unlock)
        {
            icon.color = new Color32(0, 0, 0, 255);
            selectButton.interactable = false;
        }
        else if (!GetComponent<SettingsIcon>())
        {
            icon.color = new Color32(255, 255, 255, 255);
            selectButton.interactable = true;
        }
    }

    public void SelectSkin()
    {
        player.GetComponent<Animator>().runtimeAnimatorController = animator;
        player.GetComponent<PlayerStats>().SetDestroyEffect(destroyEffect);

        for (int i = 0; i < unlockSystem.skins.Length; i++)
        {
            unlockSystem.skins[i].select = false;
        }
        select = true;

        unlockSystem.UnlockSkin(0);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ItemIcon : Icon
{
    private void Awake()
    {
        unlockSystem = FindObjectOfType<UnlockSystem>();
        icon = transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();

        UpdateLoked();
    }
}

using UnityEngine.UI;
using UnityEngine;

public class Icon : MonoBehaviour
{
    public Image unlockImage;

    [HideInInspector] public Image icon;
    public string name, engName, ruName;
    public string description, engDescription, ruDescription;
    public string type, engType, ruType;

    [SerializeField] private IconInfo iconInfo;

    public bool unlock;

    protected UnlockSystem unlockSystem;

    public IconInfo IconInfo
    {
        get
        {
            return iconInfo;
        }
    }

    private void Awake()
    {
        unlockSystem = FindObjectOfType<UnlockSystem>();
    }

    public void OpenInfo()
    {
        if (unlockSystem.languageManager.currentLanguage == Language.English)
        {
            name = engName;
            description = engDescription;
            type = engType;
        }
        else if (unlockSystem.languageManager.currentLanguage == Language.Russian)
        {
            name = ruName;
            description = ruDescription;
            type = ruType;
        }

        iconInfo.gameObject.SetActive(true);
        UpdateLoked();
        iconInfo.TransferInfo(this);
    }
    public void CloseInfo()
    {
        iconInfo.gameObject.SetActive(false);
    }

    public void UpdateLoked()
    {
        if (!unlock)
        {
            icon.color = new Color32(0, 0, 0, 255);
        }
        else if(!GetComponent<SettingsIcon>())
        {
            icon.color = new Color32(255, 255, 255, 255);
        }
    }
}

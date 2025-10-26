using UnityEngine;
using UnityEngine.UI;

public class LocationIcon : Icon
{
    public string[] enemies;
    public string[] engEnemies;
    public string[] ruEnemies;

    private void Awake()
    {
        unlockSystem = FindObjectOfType<UnlockSystem>();
        icon = transform.GetChild(0).GetComponent<Image>();
    }

    new public void OpenInfo()
    {
        if (unlockSystem.languageManager.currentLanguage == Language.English)
        {
            enemies = engEnemies;
        }
        else if (unlockSystem.languageManager.currentLanguage == Language.Russian)
        {
            enemies = ruEnemies;
        }
        base.OpenInfo();
    }
}

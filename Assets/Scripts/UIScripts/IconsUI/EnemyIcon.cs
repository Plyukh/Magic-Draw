using UnityEngine;
using UnityEngine.UI;

public class EnemyIcon : Icon
{
    public int health;
    public int speed;
    public int experience;
    public string[] abilities;
    public string[] engAbilities;
    public string[] ruAbilities;

    private void Awake()
    {
        icon = transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        unlockSystem = FindObjectOfType<UnlockSystem>();
    }

    new public void OpenInfo()
    {
        if (unlockSystem.languageManager.currentLanguage == Language.English)
        {
            abilities = engAbilities;
        }
        else if (unlockSystem.languageManager.currentLanguage == Language.Russian)
        {
            abilities = ruAbilities;
        }
        base.OpenInfo();
    }
}

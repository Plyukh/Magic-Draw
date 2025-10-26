using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public Language currentLanguage;

    [SerializeField] private UnlockSystem unlockSystem;

    public void SelectEnglishLanguage()
    {
        currentLanguage = Language.English;

        unlockSystem.SaveLanguage();
    }
    public void SelectRussianLanguage()
    {
        currentLanguage = Language.Russian;

        unlockSystem.SaveLanguage();
    }

    public void SelectLanguage(Language language)
    {
        currentLanguage = language;

        unlockSystem.SaveLanguage();
    }
}

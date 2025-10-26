using UnityEngine;
using UnityEngine.UI;

public enum Language
{
    Russian,
    English
}

public class LanguageScript : MonoBehaviour
{
    private Text languageText;

    private Language language;

    [SerializeField] private string ruText;
    [SerializeField] private string engText;

    private UnlockSystem unlockSystem;

    private void Awake()
    {
        languageText = GetComponent<Text>();
        unlockSystem = FindObjectOfType<UnlockSystem>();
    }

    private void Update()
    {
        SelectLanguage(unlockSystem.languageManager.currentLanguage);
    }

    public void SelectLanguage(Language Language)
    {
        language = Language;

        if (language == Language.English)
        {
            languageText.text = engText;
        }
        else if (language == Language.Russian)
        {
            languageText.text = ruText;
        }
    }
}

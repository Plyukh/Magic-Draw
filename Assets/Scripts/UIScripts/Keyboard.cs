using UnityEngine;
using UnityEngine.UI;
using System;

public class Keyboard : MonoBehaviour
{
    [SerializeField] private UnlockSystem unlockSystem;
    [SerializeField] private CompanyManager companyManager;
    [SerializeField] private PlayerStats player;

    private TouchScreenKeyboard keyboard;
    private Button button;
    private AudioSource audioSource;

    public string keyboardText;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (companyManager.currentStage == 0)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }

        if (keyboard != null)
        {
            if (keyboard.status == TouchScreenKeyboard.Status.Visible)
            {
                keyboardText = keyboard.text;
            }

            if (keyboard.status == TouchScreenKeyboard.Status.Done)
            {
                if (keyboardText == "PaVeryHell")
                {
                    unlockSystem.UnlockSkin(1);
                    audioSource.Play();
                }
                else if (keyboardText == "Thanks")
                {
                    player.cheat = true;
                    audioSource.Play();
                }
                else if (keyboardText[0].ToString() + keyboardText[1].ToString() == "TP")
                {
                    int x = 0;

                    if (keyboardText.Length == 3)
                    {
                        int.TryParse(keyboardText[2].ToString(), out x);

                        for (int i = 0; i < 9; i++)
                        {
                            if (i == x)
                            {
                                if (x != 4 && x != 9)
                                {
                                    companyManager.currentStage = x;
                                    audioSource.Play();
                                    break;
                                }
                            }
                        }
                    }
                    else if (keyboardText.Length == 4)
                    {
                        int.TryParse(keyboardText[2].ToString() + keyboardText[3].ToString(), out x);

                        for (int i = 10; i < 25; i++)
                        {
                            if (i == x)
                            {
                                if (x != 14 && x != 19 && x != 24)
                                {
                                    companyManager.currentStage = x;
                                    audioSource.Play();
                                    break;
                                }
                            }
                        }
                    }
                }

                keyboardText = "";
            }
        }
    }

    public void OpenKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false,false,false, true);
    }
}

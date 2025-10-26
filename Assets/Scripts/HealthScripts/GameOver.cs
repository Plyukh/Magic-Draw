using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour
{
    [SerializeField] UnlockSystem unlockSystem;
    [SerializeField] Button restartButton;

    [SerializeField] private PlayerStats player;
    [SerializeField] private CompanyManager companyManager;
    [SerializeField] private Potions potions;
    [SerializeField] private Location startLocation;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject bossHP;
    [SerializeField] private GameObject skillChapter;
    [SerializeField] private Text gameOverText;

    public Location StartLocation
    {
        get
        {
            return startLocation;
        }
    }

    public GameObject StartButton
    {
        get
        {
            return startButton;
        }
    }

    private void Update()
    {
        if (player.GetComponent<SpellManager>().currentSpells[0] != null && player.CurrentHP > 0 && !FindObjectOfType<Location>().killAll)
        {
            restartButton.interactable = true;
        }
        else
        {
            restartButton.interactable = false;
        }
    }

    public void Game_Over()
    {
        GetComponent<Image>().raycastTarget = true;
        GetComponent<Animator>().SetTrigger("Dead");
    }

    public void Win()
    {
        gameOverText.gameObject.SetActive(true);
        StartCoroutine(WinText());
    }

    public void ResetIcons()
    {
        for (int i = 0; i < unlockSystem.skills.Length; i++)
        {
            unlockSystem.skills[i].ResetIcon();
        }
        for (int i = 0; i < unlockSystem.products.Length; i++)
        {
            unlockSystem.products[i].LoadBonus();
        }
    }

    public void New_Game()
    {
        for (int i = 0; i < FindObjectsOfType<Spell>().Length; i++)
        {
            Destroy(FindObjectsOfType<Spell>()[i].gameObject);
        }

        ResetIcons();

        for (int i = 0; i < FindObjectsOfType<Item>().Length; i++)
        {
            Destroy(FindObjectsOfType<Item>()[i].gameObject);
        }

        for (int i = 0; i < player.GetComponent<SpellManager>().currentSpells.Length; i++)
        {
            player.GetComponent<SpellManager>().currentSpells[i] = null;
            player.GetComponent<SpellManager>().spellIcons[i].color = new Color32(255,255,255,0);
        }

        GetComponent<Image>().raycastTarget = false;
        Destroy(FindObjectOfType<Location>().gameObject);
        Instantiate(startLocation);

        potions.manaPotionsNumber = 0;
        potions.healthPotionsNumber = 0;

        player.Stats.SetActive(false);
        player.GetComponent<PlayerAnimations>().GameOverAnimation();

        companyManager.ResetPompanies();
        bossHP.SetActive(false);
        startButton.SetActive(true);
        gameOverText.GetComponent<CanvasGroup>().alpha = 0;
        gameOverText.gameObject.SetActive(false);

        FindObjectOfType<SoundController>().NextMusic();
    }

    IEnumerator WinText()
    {
        if (gameOverText.GetComponent<CanvasGroup>().alpha == 0)
        {
            yield return new WaitForSeconds(10f);
        }
        if(gameOverText.GetComponent<CanvasGroup>().alpha < 1)
        {
            yield return new WaitForSeconds(0.1f);
            gameOverText.GetComponent<CanvasGroup>().alpha += 0.05f;
            StartCoroutine(WinText());
        }
        else
        {
            yield return new WaitForSeconds(3f);
            player.Death();

            StopCoroutine(WinText());
        }
    }
}
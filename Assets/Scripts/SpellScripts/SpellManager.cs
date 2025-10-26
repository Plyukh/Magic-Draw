using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpellManager : MonoBehaviour
{
    [HideInInspector] public PlayerStats playerStats;
    private PlayerAnimations playerAnimations;

    public Image[] spellIcons;
    public Spell[] currentSpells;
    [SerializeField] private Spell[] spells;
    [SerializeField] private float cooldown;

    public bool seeEnemy;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerAnimations = GetComponent<PlayerAnimations>();
    }

    private void Update()
    {
        for (int i = 0; i < currentSpells.Length; i++)
        {
            if (spellIcons[i].GetComponent<Image>().fillAmount != 1)
            {
                spellIcons[i].GetComponent<Image>().fillAmount += 1 / cooldown * Time.deltaTime;
            }
        }

        if(FindObjectsOfType<Enemy>().Length == 0)
        {
            seeEnemy = false;
        }
        else if(FindObjectsOfType<Enemy>().Length > 0)
        {
            seeEnemy = true;
        }
    }

    public void CastingSpell(string id, string Direction)
    {
        if (seeEnemy)
        {
            for (int i = 0; i < currentSpells.Length; i++)
            {
                if (currentSpells[i] != null)
                {
                    if (id == currentSpells[i].Id)
                    {
                        bool canCast;
                        playerStats.ApplyMagic(currentSpells[i].manacost, out canCast);

                        if (canCast)
                        {
                            BookItems book = FindObjectOfType<BookItems>();
                            if (book != null && currentSpells[i].name == book.spellModifier.name)
                            {
                                currentSpells[i].ChangeManacost(true);

                                book.ActiveBookBonus(this, currentSpells[i], id);

                                currentSpells[i].ChangeManacost(false);

                                for (int j = 0; j < spellIcons.Length; j++)
                                {
                                    spellIcons[j].GetComponent<Image>().fillAmount = 0; //Cooldown off!
                                }

                                break;
                            }
                            else if (spellIcons[i].GetComponent<Image>().fillAmount == 1)
                            {
                                SpawnSpell(currentSpells[i], Direction);

                                for (int j = 0; j < spellIcons.Length; j++)
                                {
                                    spellIcons[j].GetComponent<Image>().fillAmount = 0; //Cooldown off!
                                }

                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    public void SpawnSpell(Spell Spell, string Direction)
    {
        Spell spell = Instantiate(Spell, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 1), Spell.transform.rotation, gameObject.transform.parent);
        spell.direction = Direction;
        playerAnimations.CastAnimation(Direction);
    }
}

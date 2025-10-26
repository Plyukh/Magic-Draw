using UnityEngine;
using System.Collections;

public enum BookType
{
    DoubleArrow,
    MassCast,
    Wave,
    Protection
}

public class BookItems : Item
{
    public BookType bookType;
    public Spell spellModifier;

    public void ActiveBookBonus(SpellManager Spell_Manager, Spell spell, string id)
    {
        if(bookType == BookType.DoubleArrow)
        {
            DoubleArrow(Spell_Manager, spell, id);
        }
        else if (bookType == BookType.MassCast)
        {
            MassCast(Spell_Manager, spell, id);
        }
        else if(bookType == BookType.Wave)
        {
            Wave(Spell_Manager, spell, id);
        }
        else if (bookType == BookType.Protection)
        {
            ProtectionSkill(Spell_Manager, spell, id);
        }
    }

    private void DoubleArrow(SpellManager Spell_Manager, Spell spell, string id)
    {
        Spell_Manager.SpawnSpell(spell, "Left");
        Spell_Manager.SpawnSpell(spell, "Right");
    }

    private void MassCast(SpellManager Spell_Manager, Spell spell, string id)
    {
        int enemiesLength = 0;

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("RightEnemy").Length + GameObject.FindGameObjectsWithTag("LeftEnemy").Length; i++)
        {
            if (!FindObjectsOfType<Enemy>()[i].Fly && FindObjectsOfType<Enemy>()[i].tag == "RightEnemy" || FindObjectsOfType<Enemy>()[i].tag == "LeftEnemy")
            {
                enemiesLength += 1;
            }
        }
        Enemy[] enemies = new Enemy[enemiesLength];
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("RightEnemy").Length + GameObject.FindGameObjectsWithTag("LeftEnemy").Length; i++)
        {
            if (!FindObjectsOfType<Enemy>()[i].Fly && FindObjectsOfType<Enemy>()[i].tag == "RightEnemy" || FindObjectsOfType<Enemy>()[i].tag == "LeftEnemy")
            {
                for (int j = 0; j < enemiesLength; j++)
                {
                    if (enemies[j] == null)
                    {
                        enemies[j] = FindObjectsOfType<Enemy>()[i];
                        break;
                    }
                }
            }
        }
        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                Spell_Manager.SpawnSpell(spell, "Right");
                FindObjectOfType<BoltSpell>().TargetPosition = enemies[i].transform.position;
                FindObjectOfType<BoltSpell>().findEnemy = false;
            }
        }
        else
        {
            Spell_Manager.SpawnSpell(spell, "Right");
            FindObjectOfType<BoltSpell>().findEnemy = false;
            FindObjectOfType<BoltSpell>().TargetPosition = new Vector3(75f, 0);
        }
    }

    private void Wave(SpellManager Spell_Manager, Spell spell, string id)
    {
        Spell_Manager.GetComponent<PlayerAnimations>().CastAnimation("Right");

        StartCoroutine(WaveCoroutine(20, 0.25f, Spell_Manager, spell, id));
    }

    private void ProtectionSkill(SpellManager Spell_Manager, Spell spell, string id)
    {
        float delay = 0.3f;
        StopCoroutine(ProtectionCoroutine(delay, Spell_Manager, spell, id));
        StartCoroutine(ProtectionCoroutine(delay, Spell_Manager, spell, id));
    }

    private IEnumerator WaveCoroutine(float SpellDistance, float Delay, SpellManager Spell_Manager, Spell spell, string id)
    {
        Spell lastRightSpell = null;
        Spell lastLeftSpell = null;

        while (lastLeftSpell == null && lastLeftSpell == null || lastRightSpell.transform.position.x <= 150 || lastLeftSpell.transform.position.x >= -150)
        {
            Instantiate(spell, new Vector3(Spell_Manager.gameObject.transform.position.x, Spell_Manager.gameObject.transform.position.y, Spell_Manager.gameObject.transform.position.z - 1), Spell_Manager.currentSpells[1].gameObject.transform.rotation, Spell_Manager.gameObject.transform.parent);
            if (lastRightSpell == null)
            {
                FindObjectOfType<BoltSpell>().TargetPosition = new Vector3(FindObjectOfType<BoltSpell>().transform.position.x + SpellDistance, 0);
            }
            else
            {
                FindObjectOfType<BoltSpell>().TargetPosition = new Vector3(lastRightSpell.transform.position.x + SpellDistance, 0);
            }
            lastRightSpell = FindObjectOfType<BoltSpell>();
            lastRightSpell.findEnemy = false;

            Instantiate(spell, new Vector3(Spell_Manager.gameObject.transform.position.x, Spell_Manager.gameObject.transform.position.y, Spell_Manager.gameObject.transform.position.z - 1), Spell_Manager.currentSpells[1].gameObject.transform.rotation, Spell_Manager.gameObject.transform.parent);
            if (lastLeftSpell == null)
            {
                FindObjectOfType<BoltSpell>().TargetPosition = new Vector3(FindObjectOfType<BoltSpell>().transform.position.x - SpellDistance, 0);
            }
            else
            {
                FindObjectOfType<BoltSpell>().TargetPosition = new Vector3(lastLeftSpell.transform.position.x - SpellDistance, 0);
            }
            lastLeftSpell = FindObjectOfType<BoltSpell>();
            lastLeftSpell.findEnemy = false;

            yield return new WaitForSeconds(Delay);
        }
    }

    private IEnumerator ProtectionCoroutine(float Delay, SpellManager Spell_Manager, Spell spell, string id)
    {
        float repeats = spell.effectTime / Delay;

        while (repeats > 0)
        {
            Instantiate(spell, Spell_Manager.transform.position, Spell_Manager.transform.rotation);
            repeats -= 1;

            yield return new WaitForSeconds(Delay);
        }
    }
}
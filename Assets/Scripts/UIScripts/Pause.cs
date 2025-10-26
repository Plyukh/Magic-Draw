using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pause : MonoBehaviour
{
    [SerializeField] Casting casting;
    public Location location;

    public List<Enemy> enemies;
    public List<Spawner> spawners;
    public List<Spell> spells;

    public void PauseGame(bool pause)
    {
        location.Pause = pause;

        casting.Pause = pause;

        foreach (var item in enemies)
        {
            item.Pause = pause;
        }
        foreach (var item in spawners)
        {
            item.Pause = pause;
        }
        foreach (var item in spells)
        {
            item.Pause = pause;
        }
    }

    public IEnumerator PauseDead(float Seconds)
    {
        PauseGame(true);
        yield return new WaitForSeconds(Seconds);
        PauseGame(false);
        StopCoroutine(PauseDead(Seconds));
    }
}
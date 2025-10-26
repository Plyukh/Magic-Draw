using UnityEngine;
using System.Collections;

public class Angel : MonoBehaviour
{
    PlayerAnimations playerAnimations;
    AudioSource audioSource;

    private void Awake()
    {
        playerAnimations = FindObjectOfType<PlayerAnimations>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Reincarnation()
    {
        playerAnimations.Reincarnation(true);
        audioSource.Play();
        StartCoroutine(EnemiesBack());
    }

    IEnumerator EnemiesBack()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < 30; i++)
        {
            yield return new WaitForSeconds(0.1f);
            for (int j = 0; j < enemies.Length; j++)
            {
                if(enemies[j].tag == "RightEnemy")
                {
                    enemies[j].transform.position += new Vector3(1, 0);
                }
                else
                {
                    enemies[j].transform.position += new Vector3(-1, 0);
                }
            }
        }

        StopCoroutine(EnemiesBack());
    }
}
using UnityEngine;

public class BoltSpell : Spell
{
    private void Start()
    {
        BookItems book = FindObjectOfType<BookItems>();

        if(findEnemy)
        {
            FindEnemy();
        }
        transform.position = new Vector3(targetPosition.x, GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.position.y, gameObject.transform.position.z);
    }

    protected override void FindEnemy()
    {
        int enemyLength = 0;
        for (int i = 0; i < GameObject.FindGameObjectsWithTag(direction + "Enemy").Length; i++)
        {
            if(!GameObject.FindGameObjectsWithTag(direction + "Enemy")[i].GetComponent<Enemy>().Fly)
            {
                enemyLength += 1;
            }
        }

        Enemy[] enemies = new Enemy[enemyLength];

        for (int i = 0; i < GameObject.FindGameObjectsWithTag(direction + "Enemy").Length; i++)
        {
            if (!GameObject.FindGameObjectsWithTag(direction + "Enemy")[i].GetComponent<Enemy>().Fly)
            {
                for (int j = 0; j < enemyLength; j++)
                {
                    if (enemies[j] == null)
                    {
                        enemies[j] = GameObject.FindGameObjectsWithTag(direction + "Enemy")[i].GetComponent<Enemy>();
                        break;
                    }
                }
            }
        }

        if (enemies.Length > 0)
        {
            float minDistance = Vector3.Distance(gameObject.transform.position, enemies[0].transform.position);
            for (int i = 0; i < enemies.Length; i++)
            {
                float distance = Vector3.Distance(gameObject.transform.position, enemies[i].transform.position);
                if (distance <= minDistance)
                {
                    minDistance = distance;
                    target = enemies[i].gameObject;
                    targetPosition = target.transform.position;
                }
            }
        }
        else
        {
            if (direction == "Right")
            {
                targetPosition = new Vector3(75f, 0);
            }
            else
            {
                targetPosition = new Vector3(-75, 0);
            }
        }
    }

    override protected void DestroySpell()
    {
        DeactivateColliders();
    }
}

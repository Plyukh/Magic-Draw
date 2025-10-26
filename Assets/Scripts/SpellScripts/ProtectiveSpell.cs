using UnityEngine;

public class ProtectiveSpell : Spell
{
    private void Start()
    {
        if (findEnemy)
        {
            FindEnemy();

            if(target == null)
            {
                if(direction == "Left")
                {
                    transform.position = new Vector2(-50,-5);
                }
                else
                {
                    transform.position = new Vector2(50, -5);
                }
            }
            else
            {
                transform.position = target.GetComponent<Collider2D>().bounds.center;
            }
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }

    protected override void DestroySpell()
    {

    }
}

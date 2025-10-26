using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Vector2 limits;
    public Vector3 startPos;

    public void Shake(int Quantity)
    {
        startPos = transform.position;

        StartCoroutine(ShakeCoroutine(Quantity));
    }

    IEnumerator ShakeCoroutine(int Quantity)
    {
        yield return new WaitForSeconds(0.05f);

        transform.position = new Vector3(Random.Range(limits.x, -limits.x), Random.Range(limits.y, -limits.y), startPos.z);

        Quantity -= 1;

        if (Quantity != 0)
        {
            StartCoroutine(ShakeCoroutine(Quantity));
        }
        else
        {
            transform.position = startPos;
        }

        StopCoroutine(ShakeCoroutine(Quantity));
    }
}

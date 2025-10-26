using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Replicas : MonoBehaviour
{
    [SerializeField] private Text text;

    private Color32 visColor = new Color32(255,0,0,255);
    private Color32 invisColor = new Color32(255, 0, 0, 0);
    [SerializeField] private float colorSpeed;

    [SerializeField] private UnlockSystem unlockSystem;

    [SerializeField] private string currentReplic;
    private int currentReplicIndex;

    [SerializeField] private string[] enReplicas;
    [SerializeField] private string[] rusReplicas;

    private bool lerpVis, lerpInvis;

    public void ShowReplic()
    {
        StopCoroutine(NewReplica());
        text.color = Color.Lerp(text.color, invisColor, colorSpeed);
        StartCoroutine(NewReplica());
    }

    private void Update()
    {
        if (lerpVis)
        {
            text.color = Color.Lerp(text.color, visColor, colorSpeed * Time.deltaTime);
            if (text.color.a > 0.99)
            {
                lerpVis = false;
            }
        }
        if (lerpInvis)
        {
            text.color = Color.Lerp(text.color, invisColor, colorSpeed * Time.deltaTime);
            if (text.color.a <= 0.01)
            {
                lerpInvis = false;
            }
        }

        if (unlockSystem.languageManager.currentLanguage == Language.English)
        {
            currentReplic = enReplicas[currentReplicIndex];
        }
        else if (unlockSystem.languageManager.currentLanguage == Language.Russian)
        {
            currentReplic = rusReplicas[currentReplicIndex];
        }
        text.text = currentReplic;
    }

    IEnumerator NewReplica()
    {
        if (text.color.a <= 0.01)
        {
            int randomWaitTime = Random.Range(5, 20);
            yield return new WaitForSeconds(randomWaitTime);

            currentReplicIndex = Random.Range(0, enReplicas.Length);

            lerpVis = true;
        }
        else
        {
            int randomWaitTime = Random.Range(3, 5);
            yield return new WaitForSeconds(randomWaitTime);
            lerpInvis = true;
        }

        yield return new WaitForSeconds(1.5f);

        StopCoroutine(NewReplica());
        StartCoroutine(NewReplica());
    }
}

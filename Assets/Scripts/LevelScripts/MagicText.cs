using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MagicText : MonoBehaviour
{
    private Text text;
    [SerializeField] private char[] magicText;
    private string charBase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";   

    private void Awake()
    {
        text = GetComponent<Text>();
        magicText = new char[text.text.Length];
        magicText = text.text.ToCharArray();
    }

    private void Start()
    {
        StartCoroutine(RandomText(0.25f));
    }

    IEnumerator RandomText(float Delay)
    {
        yield return new WaitForSeconds(Delay);

        for (int i = 0; i < magicText.Length; i++)
        {
            int random = Random.Range(0, 2);

            if(random > 0)
            {
                magicText[i] = charBase[Random.Range(0, charBase.Length)];
            }
        }

        text.text = new string(magicText);

        StartCoroutine(RandomText(Delay));
    }
}

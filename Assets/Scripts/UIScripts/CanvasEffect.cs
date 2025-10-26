using UnityEngine;
using UnityEngine.UI;

public class CanvasEffect : MonoBehaviour
{
    public float speed;
    public float addX;
    public float addY;
    private Vector2 startPosition;
    private RectTransform rectTransform;
    private bool size;

    private void Start()
    {
        GetComponent<Text>().color += new Color32(0,0,0,255);

        rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector2(startPosition.x, startPosition.y + addY) * addX;
    }

    private void Update()
    {
        if (GetComponent<Text>().fontSize >= 100)
        {
            size = true;
        }
        if (size)
        {
            GetComponent<Text>().fontSize -= 1;
        }
        else
        {
            GetComponent<Text>().fontSize += 1;
        }

        rectTransform.anchoredPosition += new Vector2(0, speed);
        if(GetComponent<Text>().fontSize == 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeEffect(Vector2 Position)
    {
        startPosition = Position;
    }
    public void TakeEffect(Vector2 Position, string Text)
    {
        GetComponent<Text>().text = Text;
        startPosition = Position;
    }
}

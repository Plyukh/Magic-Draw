using UnityEngine;

public class NPSButton : MonoBehaviour
{
    private RectTransform buttonRectTransform;
    public float addX;
    public float addY;

    private void Start()
    {
        buttonRectTransform = GameObject.Find(gameObject.name + " Button").GetComponent<RectTransform>();

        buttonRectTransform.anchoredPosition = new Vector2(transform.position.x, transform.position.y + addY) * addX;

        if(buttonRectTransform.transform.childCount > 0)
        {
            buttonRectTransform.transform.GetChild(0).transform.SetParent(buttonRectTransform.parent);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private Product[] products;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button closeButton;

    private bool nextButtonBool, previousButtonBool;

    public void OpenShop()
    {
        previousButton.interactable = false;
        nextButton.interactable = true;

        products[0].gameObject.SetActive(true);
        products[0].ShowProduct();
        for (int i = 1; i < products.Length; i++)
        {
            products[i].gameObject.SetActive(false);
        }
    }

    public void NextProduct()
    {
        previousButton.interactable = true;
        for (int i = 0; i < products.Length; i++)
        {
            if (products[i].gameObject.activeInHierarchy)
            {
                products[i].gameObject.SetActive(false);
                products[i + 1].gameObject.SetActive(true);
                products[i + 1].ShowProduct();
                if (products[products.Length - 1].gameObject.activeInHierarchy)
                {
                    nextButton.interactable = false;
                }
                break;
            }
        }
    }
    public void PreviousProduct()
    {
        nextButton.interactable = true;
        for (int i = products.Length - 1; i > -1; i--)
        {
            if (products[i].gameObject.activeInHierarchy)
            {
                products[i].gameObject.SetActive(false);
                products[i - 1].gameObject.SetActive(true);
                products[i - 1].ShowProduct();
                if (products[0].gameObject.activeInHierarchy)
                {
                    previousButton.interactable = false;
                }
                break;
            }
        }
    }

    public void InteractableButtons(bool interactable)
    {
        if (interactable == false)
        {
            nextButtonBool = nextButton.interactable;
            previousButtonBool = previousButton.interactable;

            nextButton.interactable = false;
            previousButton.interactable = false;
            closeButton.interactable = false;
        }
        else
        {
            nextButton.interactable = nextButtonBool;
            previousButton.interactable = previousButtonBool;

            closeButton.interactable = true;
        }
    }
}

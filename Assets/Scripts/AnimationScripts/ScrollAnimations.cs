using UnityEngine;

public class ScrollAnimations : MonoBehaviour
{
    private Animator animator;
    private CompanyManager companyManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        companyManager = transform.parent.GetComponent<CompanyManager>();
    }

    public void OpenScroll()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        companyManager.ShowCompanies();
    }
    public void CloseScroll()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        if(transform.childCount > 1)
        {
            Destroy(gameObject.transform.GetChild(1).gameObject);
        }
    }

    public void CloseScrollAnimation()
    {
        animator.SetTrigger("Close Scroll");
    }

    public void HideScroll()
    {
        gameObject.SetActive(false);
    }
}

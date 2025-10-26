using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class RewardAnimations : MonoBehaviour
{
    private CompanyManager companyManager;

    [SerializeField] private GameObject replaceItem;
    [SerializeField] private GameObject currentItem, nextItem;

    [SerializeField] private Image itemImage;
    [SerializeField] private new ParticleSystem particleSystem;
    [SerializeField] private GameObject destroyItemEffect;

    [SerializeField] GameObject healthTarget, manaTarget, swordTarget, staveTarget, bookTarget, amuletTarget;
    [SerializeField] AudioClip botleClip, drinkPotion, swordClip, staveClip, bookClip, amuletClip;

    public float speed;
    private GameObject target;
    private RectTransform startRect;
    private Vector2 baseRect;
    private Item item;
    private bool minus;

    private bool destroyCurrentItem;

    private void Awake()
    {
        companyManager = transform.parent.GetChild(0).GetComponent<CompanyManager>();
    }

    private void Update()
    {
        if(target != null)
        {
            if (destroyCurrentItem || item.itemType == ItemType.LvlUpPotion || item.itemType == ItemType.MaxHealthPotion || item.itemType == ItemType.MaxManaPorion)
            {
                minus = true;
                target = null;
                StartCoroutine(MinusScale());
            }
            else
            {
                speed += 2.5f;
                transform.GetChild(0).transform.position = Vector2.MoveTowards(transform.GetChild(0).transform.position, target.transform.position, speed * Time.deltaTime);
                if (Vector2.Distance(transform.GetChild(0).transform.position, target.transform.position) <= 300 && minus == false)
                {
                    minus = true;
                    StartCoroutine(MinusScale());
                }
                if (Vector2.Distance(transform.GetChild(0).transform.position, target.transform.position) == 0)
                {
                    itemImage.rectTransform.sizeDelta = new Vector2(0, 0);
                }
            }
        }
    }

    public void TakeReward(Item Item)
    {
        item = Item;

        transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 150);
        itemImage.sprite = item.transform.GetChild(0).GetComponent<Image>().sprite;
        particleSystem.Play();
        baseRect = item.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
        startRect = item.transform.GetChild(0).GetComponent<RectTransform>();
        startRect.sizeDelta *= 1.6f;

        StartCoroutine(PlusScale());
    }

    public void MoveAnimation()
    {
        if (item.itemType == ItemType.HealthPotion)
        {
            target = healthTarget;
        }
        else if (item.itemType == ItemType.ManaPotion)
        {
            target = manaTarget;
        }
        else if (item.itemType == ItemType.Amulet)
        {
            target = amuletTarget;
        }
        else if (item.itemType == ItemType.Book)
        {
            target = bookTarget;
        }
        else if (item.itemType == ItemType.Stave)
        {
            target = staveTarget;
        }
        else if (item.itemType == ItemType.Sword)
        {
            target = swordTarget;
        }
        else if (item.itemType == ItemType.LvlUpPotion || item.itemType == ItemType.MaxHealthPotion || item.itemType == ItemType.MaxManaPorion)
        {
            target = manaTarget;
        }
    }

    IEnumerator PlusScale()
    {
        yield return new WaitForSeconds(0.01f);
        itemImage.rectTransform.sizeDelta += new Vector2(startRect.sizeDelta.x /75, startRect.sizeDelta.y / 75);

        if(itemImage.rectTransform.sizeDelta.x >= startRect.sizeDelta.x)
        {
            yield return new WaitForSeconds(0.25f);

            if (item.GetComponent<PotionItems>())
            {
                MoveAnimation();
            }
            else if (GameObject.Find(item.itemType.ToString()).transform.childCount > 0)
            {
                if(nextItem.transform.childCount > 0)
                {
                    Destroy(nextItem.transform.GetChild(0).gameObject);
                    Destroy(currentItem.transform.GetChild(0).gameObject);
                }

                target = null;

                replaceItem.SetActive(true);

                nextItem.GetComponent<Image>().sprite = itemImage.sprite;
                nextItem.GetComponent<RectTransform>().sizeDelta = baseRect;
                currentItem.GetComponent<Image>().sprite = GameObject.Find(item.itemType.ToString()).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite;
                currentItem.GetComponent<RectTransform>().sizeDelta = GameObject.Find(item.itemType.ToString()).transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
            }
            else
            {
                MoveAnimation();
            }

            StopCoroutine(PlusScale());
        }
        else
        {
            StartCoroutine(PlusScale());
        }
    }
    public IEnumerator MinusScale()
    {
        yield return new WaitForSeconds(0.01f);
        itemImage.rectTransform.sizeDelta -= new Vector2(startRect.sizeDelta.x / 25, startRect.sizeDelta.y / 25);
        if (itemImage.rectTransform.sizeDelta.x <= 0)
        {
            target = null;
            speed = 500;
            startRect.sizeDelta /= 1.6f;
            particleSystem.Stop();

            if (!destroyCurrentItem)
            {
                GetItem();
            }

            minus = false;

            yield return new WaitForSeconds(1.5f);

            destroyCurrentItem = false;
            item.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = baseRect;

            StopCoroutine(MinusScale());

            companyManager.OpenScrolls();

            yield return new WaitForSeconds(1f);

            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(MinusScale());
        }
    }

    void GetItem()
    {
        if (item.itemType == ItemType.HealthPotion)
        {
            FindObjectOfType<Potions>().AddHealthPotin();
            RewardSound(botleClip);
        }
        else if(item.itemType == ItemType.ManaPotion)
        {
            FindObjectOfType<Potions>().AddManaPotin();
            RewardSound(botleClip);
        }
        else if (item.itemType == ItemType.LvlUpPotion || item.itemType == ItemType.MaxHealthPotion || item.itemType == ItemType.MaxManaPorion)
        {
            RewardSound(drinkPotion);

            if (item.itemType == ItemType.MaxHealthPotion)
            {
                FindObjectOfType<Potions>().AddMaxValuePotion(item.GetComponent<PotionItems>().addMaxValue);
            }
            else if (item.itemType == ItemType.MaxManaPorion)
            {
                FindObjectOfType<Potions>().AddMaxValuePotion(0, item.GetComponent<PotionItems>().addMaxValue);
            }
            else
            {
                FindObjectOfType<Potions>().LvlUpPotion();
            }
        }
        else
        {
            if (GameObject.Find(item.itemType.ToString()).transform.childCount > 0)
            {
                GameObject.Find(item.itemType.ToString()).transform.GetChild(0).GetComponent<Item>().RemoveBonus();
                Destroy(GameObject.Find(item.itemType.ToString()).transform.GetChild(0).gameObject);
            }
            Instantiate(item, GameObject.Find(item.itemType.ToString()).transform);

            if(item.itemType == ItemType.Sword)
            {
                RewardSound(swordClip);
            }
            else if (item.itemType == ItemType.Stave)
            {
                RewardSound(staveClip);
            }
            else if (item.itemType == ItemType.Book)
            {
                RewardSound(bookClip);
            }
            else if (item.itemType == ItemType.Amulet)
            {
                RewardSound(amuletClip);
            }
        }
    }

    public void DestroyCurrentItem()
    {
        MoveAnimation();
    }
    public void DestroyNextItem()
    {
        Instantiate(destroyItemEffect, itemImage.transform.parent);
        particleSystem.Stop();
        destroyCurrentItem = true;
        MoveAnimation();
    }

    void RewardSound(AudioClip clip)
    {
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }
}
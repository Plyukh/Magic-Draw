using UnityEngine;
using UnityEngine.UI;

public class Chapter : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip openBookClip, closeBookClip;
    [SerializeField] private Button bookButton;
    [SerializeField] private Button leftButton, rightButoon;
    [SerializeField] private Bookmarks bookmarks;
    [SerializeField] private Bookmarks filterBookmarks, filterBookmarksItem;
    [SerializeField] private int numberPages, currentPage;

    private int numberIcons;
    private string filter;
    private GameObject[] icons, filterIcons;

    [SerializeField] private UnlockSystem unlockSystem;
    [SerializeField] private Pause pause;

    public Button BookButton
    {
        get
        {
            return bookButton;
        }
    }

    public void AddChapter(GameObject ChapterObject, string Filter)
    {
        filter = Filter;
        currentPage = 1;

        int IconsLength = 0;

        if (filter == "")
        {
            if(bookmarks.ChapterText.text == "Items" || bookmarks.ChapterText.text == "Предметы")
            {
                filterBookmarksItem.gameObject.SetActive(true);
                filterBookmarks.gameObject.SetActive(false);
            }
            else if (bookmarks.ChapterText.text == "Achievements" || bookmarks.ChapterText.text == "Settings" || 
                     bookmarks.ChapterText.text == "Достижения" || bookmarks.ChapterText.text == "Настройки" ||
                     bookmarks.ChapterText.text == "Credits" || bookmarks.ChapterText.text == "Благодарности")
            {
                filterBookmarksItem.gameObject.SetActive(false);
                filterBookmarks.gameObject.SetActive(false);
            }
            else
            {
                filterBookmarksItem.gameObject.SetActive(false);
                filterBookmarks.gameObject.SetActive(true);
            }

            if (numberIcons == 0)
            {
                for (int i = 0; i < ChapterObject.transform.childCount; i++)
                {
                    if (ChapterObject.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition.y > -ChapterObject.GetComponent<RectTransform>().sizeDelta.y)
                    {
                        numberIcons += 1;
                    }
                }
            }


            for (int i = 0; i < ChapterObject.transform.childCount; i++)
            {
                IconsLength += 1;
            }

            icons = new GameObject[IconsLength];

            for (int i = 0; i < ChapterObject.transform.childCount; i++)
            {
                icons[i] = ChapterObject.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < IconsLength; i++)
            {
                if (i < numberIcons)
                {
                    icons[i].SetActive(true);
                    icons[i].GetComponent<Icon>().UpdateLoked();
                }
                else
                {
                    icons[i].SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < icons.Length; i++)
            {
                if (icons[i].GetComponent<Icon>().engType == filter)
                {
                    icons[i].SetActive(true);
                    IconsLength += 1;
                }
                else
                {
                    icons[i].SetActive(false);
                }
            }

            filterIcons = new GameObject[IconsLength];

            for (int i = 0; i < icons.Length; i++)
            {
                if (icons[i].GetComponent<Icon>().engType == filter)
                {
                    for (int j = 0; j < filterIcons.Length; j++)
                    {
                        if(filterIcons[j] == null)
                        {
                            filterIcons[j] = icons[i];
                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < IconsLength; i++)
            {
                if (i < numberIcons)
                {
                    filterIcons[i].SetActive(true);
                    filterIcons[i].GetComponent<Icon>().UpdateLoked();
                }
                else
                {
                    filterIcons[i].SetActive(false);
                }
            }
        }

        float Numbers = numberIcons;
        numberPages = Mathf.CeilToInt(IconsLength / Numbers);

        OpenFirstIcon();

        rightButoon.transform.parent.gameObject.SetActive(false);
        leftButton.transform.parent.gameObject.SetActive(false);

        if (numberIcons < IconsLength)
        {
            rightButoon.transform.parent.gameObject.SetActive(true);
        }
    }

    public void ScrollingRight()
    {
        leftButton.transform.parent.gameObject.SetActive(true);
        currentPage += 1;

        if(filter == "")
        {
            for (int i = 0; i < icons.Length; i++)
            {
                if (i < numberIcons * currentPage && i >= numberIcons * (currentPage - 1))
                {
                    icons[i].gameObject.SetActive(true);
                    icons[i].GetComponent<Icon>().UpdateLoked();
                }
                else
                {
                    icons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < filterIcons.Length; i++)
            {
                if (i < numberIcons * currentPage && i >= numberIcons * (currentPage - 1))
                {
                    filterIcons[i].SetActive(true);
                    filterIcons[i].GetComponent<Icon>().UpdateLoked();
                }
                else
                {
                    filterIcons[i].SetActive(false);
                }
            }
        }


        OpenFirstIcon();

        if (currentPage == numberPages)
        {
            rightButoon.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            rightButoon.transform.parent.gameObject.SetActive(true);
        }

        rightButoon.onClick.RemoveListener(ScrollingRight);
    }
    public void ScrollingLeft()
    {
        rightButoon.transform.parent.gameObject.SetActive(true);
        currentPage -= 1;

        if(filter == "")
        {
            for (int i = 0; i < icons.Length; i++)
            {
                if (i >= numberIcons * (currentPage - 1) && i < numberIcons * currentPage)
                {
                    icons[i].gameObject.SetActive(true);
                    icons[i].GetComponent<Icon>().UpdateLoked();
                }
                else
                {
                    icons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < filterIcons.Length; i++)
            {
                if (i < numberIcons * currentPage && i >= numberIcons * (currentPage - 1))
                {
                    filterIcons[i].SetActive(true);
                    filterIcons[i].GetComponent<Icon>().UpdateLoked();
                }
                else
                {
                    filterIcons[i].SetActive(false);
                }
            }
        }

        OpenFirstIcon();

        if (currentPage == 1)
        {
            leftButton.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            leftButton.transform.parent.gameObject.SetActive(true);
        }

        leftButton.onClick.RemoveListener(ScrollingLeft);
    }

    void OpenFirstIcon()
    {
        bookmarks.ChapterText.gameObject.SetActive(true);
        bookmarks.InteractableBookmarks(true);
        filterBookmarks.InteractableBookmarks(true);
        filterBookmarksItem.InteractableBookmarks(true);

        for (int i = 0; i < icons.Length; i++)
        {
            if (icons[i].gameObject.activeInHierarchy)
            {
                if (icons[i].GetComponent<EnemyIcon>())
                {
                    icons[i].GetComponent<EnemyIcon>().OpenInfo();
                }
                else if (icons[i].GetComponent<LocationIcon>())
                {
                    icons[i].GetComponent<LocationIcon>().OpenInfo();
                }
                else
                {
                    icons[i].GetComponent<Icon>().OpenInfo();
                }

                break;
            }
        }
    }

    public void ClearPages()
    {
        icons[0].GetComponent<Icon>().CloseInfo();

        bookmarks.ChapterText.gameObject.SetActive(false);

        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].gameObject.SetActive(false);
        }
    }

    public void Open_Close_Book()
    {
        bookButton.interactable = false;
        bookmarks.gameObject.SetActive(false);
        filterBookmarks.gameObject.SetActive(false);
        filterBookmarksItem.gameObject.SetActive(false);

        rightButoon.transform.parent.gameObject.SetActive(false);
        leftButton.transform.parent.gameObject.SetActive(false);

        if (gameObject.activeInHierarchy)
        {
            FindObjectOfType<Tutorial>().CompleteTutorial(3);

            audioSource.clip = closeBookClip;
            audioSource.Play();

            ClearPages();
            GetComponent<Animator>().SetTrigger("Close");
        }
        else
        {
            pause.PauseGame(true);

            gameObject.SetActive(true);

            audioSource.clip = openBookClip;
            audioSource.Play();
        }
    }

    public void OpenBook()
    {
        bookmarks.gameObject.SetActive(true);
        filterBookmarks.gameObject.SetActive(true);
        bookmarks.SelectChapter(bookmarks.BookmarksMass[0]);

        unlockSystem.StopAllCoroutines();
        bookButton.transform.GetChild(0).GetComponent<Image>().color = new Color32(255,255,255,255);

        if (transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Icon>().IconInfo.AddPointButton.gameObject.activeInHierarchy)
        {
            bookButton.interactable = false;
        }
        else
        {
            bookButton.interactable = true;
        }
    }
    public void CloseBook()
    {
        pause.PauseGame(false);

        bookButton.interactable = true;
        gameObject.SetActive(false);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class Bookmarks : MonoBehaviour
{
    [SerializeField] private Chapter chapter;
    [SerializeField] private Bookmark[] bookmarks;
    [SerializeField] private Text chapterText;
    [SerializeField] private LanguageManager languageManager;

    public Bookmark[] BookmarksMass
    {
        get
        {
            return bookmarks;
        }
    }
    public Text ChapterText
    {
        get
        {
            return chapterText;
        }
    }

    public void SelectChapter(Bookmark Bookmark)
    {
        for (int i = 0; i < bookmarks.Length; i++)
        {
            if (bookmarks[i] == Bookmark)
            {
                Bookmark.GetComponent<RectTransform>().anchoredPosition = Bookmark.selectPosition;

                if (bookmarks[i].chapterObject != null)
                {
                    Bookmark.chapterObject.SetActive(true);
                    chapterText.gameObject.SetActive(true);
                    if(languageManager.currentLanguage == Language.English)
                    {
                        chapterText.text = Bookmark.englishChapterText;
                    }
                    else if (languageManager.currentLanguage == Language.Russian)
                    {
                        chapterText.text = Bookmark.russianChapterText;
                    }
                }
                chapter.AddChapter(Bookmark.chapterObject, Bookmark.filterType);
            }
            else
            {
                bookmarks[i].GetComponent<RectTransform>().anchoredPosition = bookmarks[i].basePosition;

                if(bookmarks[i].chapterObject != null)
                {
                    for (int j = 0; j < bookmarks[i].chapterObject.transform.childCount; j++)
                    {
                        if (bookmarks[i].chapterObject.transform.GetChild(j).gameObject.activeInHierarchy)
                        {
                            bookmarks[i].chapterObject.transform.GetChild(j).GetComponent<Icon>().CloseInfo();
                        }
                    }

                    bookmarks[i].chapterObject.SetActive(false);
                }
            }

            for (int j = 0; j < FindObjectsOfType<Bookmark>().Length; j++)
            {
                if (FindObjectsOfType<Bookmark>()[j].filterType != "")
                {
                    if(FindObjectsOfType<Bookmark>()[j] != Bookmark)
                    {
                        FindObjectsOfType<Bookmark>()[j].GetComponent<RectTransform>().anchoredPosition = FindObjectsOfType<Bookmark>()[j].basePosition;
                    }
                }
            }
        }
    }

    public void InteractableBookmarks(bool Interactable)
    {
        for (int i = 0; i < bookmarks.Length; i++)
        {
            bookmarks[i].GetComponent<Button>().interactable = Interactable;
        }
    }
}

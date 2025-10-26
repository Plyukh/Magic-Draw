using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    [SerializeField] private CanvasGroup imageBackrgound;

    private void Start()
    {
        LoadLevel(1);
    }

    public void LoadLevel(int index)
    {
        StartCoroutine(LoadAsynchronously(index));
    }

    IEnumerator LoadAsynchronously(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        float progress = Mathf.Clamp01(operation.progress / .9f);

        imageBackrgound.alpha = (-1 + progress) * (-1);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}

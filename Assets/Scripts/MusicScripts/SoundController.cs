using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;

    public float musicVolume;
    public float effectsVolume;

    [SerializeField] AudioClip mainClip;
    [SerializeField] AudioClip[] music;
    [SerializeField] AudioClip[] bossClip;

    private bool transition;

    private void Update()
    {
        if (!transition)
        {
            musicVolume = musicSlider.value / musicSlider.maxValue;
        }

        effectsVolume = effectsSlider.value / effectsSlider.maxValue;

        for (int i = 0; i < FindObjectsOfType<AudioSource>().Length; i++)
        {
            if (FindObjectsOfType<AudioSource>()[i].GetComponent<SoundController>())
            {
                if (!transition)
                {
                    FindObjectsOfType<AudioSource>()[i].volume = musicVolume;
                }
            }
            else
            {
                FindObjectsOfType<AudioSource>()[i].volume = effectsVolume;
            }
        }
    }

    public void NextMusic()
    {
        transition = false;
        StopAllCoroutines();

        if (FindObjectOfType<CompanyManager>().currentStage == 0)
        {
            StartCoroutine(Music(mainClip, musicVolume));
        }
        else if (FindObjectOfType<CompanyManager>().currentStage == 5)
        {
            StartCoroutine(Music(bossClip[0], musicVolume));
        }
        else if (FindObjectOfType<CompanyManager>().currentStage == 10)
        {
            StartCoroutine(Music(bossClip[1], musicVolume));
        }
        else if (FindObjectOfType<CompanyManager>().currentStage == 15)
        {
            StartCoroutine(Music(bossClip[2], musicVolume));
        }
        else if (FindObjectOfType<CompanyManager>().currentStage == 20)
        {
            StartCoroutine(Music(bossClip[3], musicVolume));
        }
        else if (FindObjectOfType<CompanyManager>().currentStage == 25)
        {
            StartCoroutine(Music(bossClip[4], musicVolume));
        }
        else
        {
            int random = Random.Range(0, music.Length);

            if (GetComponent<AudioSource>().clip != music[random])
            {
                StartCoroutine(Music(music[random], musicVolume));
            }
            else
            {
                NextMusic();
            }
        }
    }

    IEnumerator Music(AudioClip Clip, float LastVolume)
    {
        transition = true;

        yield return new WaitForSeconds(0.0005f);
        if(GetComponent<AudioSource>().clip != Clip)
        {
            if (GetComponent<AudioSource>().volume <= 0)
            {
                GetComponent<AudioSource>().clip = Clip;
                GetComponent<AudioSource>().Play();
            }
            else
            {
                GetComponent<AudioSource>().volume -= 0.005f;
            }
            StartCoroutine(Music(Clip, LastVolume));
        }
        else
        {
            if (GetComponent<AudioSource>().volume >= LastVolume)
            {
                GetComponent<AudioSource>().volume = LastVolume;
                transition = false;
                StopCoroutine(Music(Clip, LastVolume));
            }
            else
            {
                GetComponent<AudioSource>().volume += 0.005f;
                StartCoroutine(Music(Clip, LastVolume));
            }
        }
    }
}

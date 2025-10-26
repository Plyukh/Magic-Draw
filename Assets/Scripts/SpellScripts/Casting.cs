using UnityEngine;
using GestureRecognizer;
using System.Collections;

public class Casting : MonoBehaviour 
{
	[SerializeField] private SpellManager spellManager;

    [SerializeField] private Color32 airColor, fireColor, waterColor, earthColor;
    [SerializeField] private GameObject castingEffect;
    [SerializeField] private float clearLinesSeconds;
    private float currentSeconds;

    private GameObject newEffect;

    private bool mouseDown, spawnEffect;
    private bool pause;

    public bool Pause
    {
        get
        {
            return pause;
        }
        set
        {
            pause = value;
        }
    }

    public void OnRecognize(RecognitionResult result)
	{
        transform.GetChild(0).GetComponent<UILineRenderer>().raycastTarget = false;

        if (result != RecognitionResult.Empty)
        {
            if (GetComponent<DrawDetector>().line.SelectDirection() == "Right")
            {
                spellManager.CastingSpell(result.gesture.id, "Right");
            }
            else if (GetComponent<DrawDetector>().line.SelectDirection() == "Left")
            {
                spellManager.CastingSpell(result.gesture.id, "Left");
            }

            SelectColor(result);
        }

        currentSeconds = 0.1f;
    }

    private void SelectColor(RecognitionResult result)
    {
        for (int i = 0; i < spellManager.currentSpells.Length; i++)
        {
            if (spellManager.currentSpells[i] != null)
            {
                if (result.gesture.id == spellManager.currentSpells[i].Id)
                {
                    if (spellManager.currentSpells[i].Type == Elements.Air)
                        transform.GetChild(0).GetComponent<UILineRenderer>().color = airColor;
                    else if (spellManager.currentSpells[i].Type == Elements.Fire)
                        transform.GetChild(0).GetComponent<UILineRenderer>().color = fireColor;
                    else if (spellManager.currentSpells[i].Type == Elements.Water)
                        transform.GetChild(0).GetComponent<UILineRenderer>().color = waterColor;
                    else if (spellManager.currentSpells[i].Type == Elements.Earth)
                        transform.GetChild(0).GetComponent<UILineRenderer>().color = earthColor;

                    break;
                }
            }
        }
    }

    [System.Obsolete]
    private void Update()
    {
        if (pause)
        {
			transform.GetChild(0).GetComponent<UILineRenderer>().raycastTarget = false;
        }
        else
        {
			transform.GetChild(0).GetComponent<UILineRenderer>().raycastTarget = true;
		}

        if (currentSeconds > 0)
        {
            currentSeconds += Time.deltaTime;
            if (currentSeconds >= clearLinesSeconds)
            {
                gameObject.GetComponent<DrawDetector>().ClearLines();
                transform.GetChild(0).GetComponent<UILineRenderer>().raycastTarget = true;
                currentSeconds = 0;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            mouseDown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
            spawnEffect = false;
            if(newEffect != null)
            {
                newEffect.GetComponent<ParticleSystem>().loop = false;
                newEffect.transform.GetChild(0).GetComponent<ParticleSystem>().loop = false;
                newEffect.GetComponent<AudioSource>().Stop();
            }
        }

        if (GetComponent<DrawDetector>().line.Points.Length > 0 && mouseDown)
        {
            if(spawnEffect == false)
            {
                newEffect = Instantiate(castingEffect, Input.mousePosition, castingEffect.transform.rotation, gameObject.transform.parent);
                spawnEffect = true;
            }
            else
            {
                newEffect.transform.position = Input.mousePosition;
            }
        }
    }
}
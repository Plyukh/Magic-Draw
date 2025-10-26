using UnityEngine;
using System.Collections;

public enum Value
{
    Mana,
    Health
}

public class AddValueEffect : MonoBehaviour
{
    [HideInInspector] public PlayerStats playerStats;
    public Value valueType;
    public float delay;
    public float division;
    public float value;
    public float effectValue;
    float startValue;

    private void Start()
    {
        value = value / division;

        float newValue = value;
        float addValue = 0;

        while (newValue > 0)
        {
            addValue = value / 10;
            addValue = Mathf.Round(addValue);
            newValue -= addValue;
            effectValue += addValue;
        }

        if (valueType == Value.Mana)
        {
            FindObjectOfType<CanvasEffectManager>().SpawnMagicRecoveryEffect(Mathf.Round(effectValue).ToString());
        }
        else if(valueType == Value.Health)
        {
            FindObjectOfType<CanvasEffectManager>().SpawnHealEffect(Mathf.Round(effectValue).ToString());
        }
        startValue = value;
        StartCoroutine(AddValue(true));
    }

    private IEnumerator AddValue(bool first)
    {
        if (first)
        {
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(0.2f);

        if(value > 0)
        {
            float newValue = startValue / 10;
            newValue = Mathf.Round(newValue);
            if(valueType == Value.Mana)
            {
                playerStats.ApplyMagicRecovery(newValue);
            }
            else if (valueType == Value.Health)
            {
                playerStats.ApplyHeal(newValue);
            }
            value -= newValue;
            if(value <= 0)
            {
                Destroy(gameObject, delay);
            }
            StartCoroutine(AddValue(false));
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class CurrencyBase : MonoBehaviour
{
    [SerializeField] private UnlockSystem unlockSystem;

    public int earthPoints;
    public int waterPoints;
    public int firePoints;
    public int airPoints;
    public int darkPoints;

    public int maxPoints;

    private int[] values = new int[5];

    [SerializeField] private ParticleSystem[] pointsUI;
    [SerializeField] private Text[] pointsText;

    [SerializeField] float speed;
    [SerializeField] private GameObject[] targets;
    private Product currentProduct;

    [SerializeField] private bool[] moveParticle;

    [SerializeField] ParticleSystem moveEffect;

    [SerializeField] private List<ParticleSystem> currentEarthMoveCurency;
    [SerializeField] private List<ParticleSystem> currentWaterMoveCurency;
    [SerializeField] private List<ParticleSystem> currentFireMoveCurency;
    [SerializeField] private List<ParticleSystem> currentAirMoveCurency;
    [SerializeField] private List<ParticleSystem> currentDarkMoveCurency;

    public Product SetProduct
    {
        set
        {
            currentProduct = value;
        }
    }

    private void Update()
    {
        if (currentProduct != null)
        {
            SelectParticle(0, currentEarthMoveCurency);
            SelectParticle(1, currentWaterMoveCurency);
            SelectParticle(2, currentFireMoveCurency);
            SelectParticle(3, currentAirMoveCurency);
            SelectParticle(4, currentDarkMoveCurency);
        }
    }

    public void AddPoints(int Points, Elements Elements)
    {
        if(Elements == Elements.Earth)
        {
            earthPoints += Points;
            if (earthPoints > maxPoints)
            {
                earthPoints = maxPoints;
            }
        }
        else if (Elements == Elements.Water)
        {
            waterPoints += Points;
            if (waterPoints > maxPoints)
            {
                waterPoints = maxPoints;
            }
        }
        else if (Elements == Elements.Fire)
        {
            firePoints += Points;
            if (firePoints > maxPoints)
            {
                firePoints = maxPoints;
            }
        }
        else if (Elements == Elements.Air)
        {
            airPoints += Points;
            if (airPoints > maxPoints)
            {
                airPoints = maxPoints;
            }
        }
        else
        {
            darkPoints += Points;
            if (darkPoints > maxPoints)
            {
                darkPoints = maxPoints;
            }
        }

        UpdatePoints();
    }

    public IEnumerator MoveParticle(GameObject Target, int index)
    {
        SetTarget(Target, index);
        moveParticle[index] = true;
        for (int i = 0; i < currentProduct.price[index]; i++)
        {
            yield return new WaitForSeconds(0.05f);
            ParticleSystem Point = Instantiate(moveEffect, pointsUI[index].gameObject.transform.position, pointsUI[index].transform.rotation, pointsUI[index].transform.parent.parent.parent.parent);
            Point.startColor = pointsUI[index].startColor;
            if(index == 0)
            {
                currentEarthMoveCurency.Add(Point);
                AddPoints(-1, Elements.Earth);
            }
            else if(index == 1)
            {
                currentWaterMoveCurency.Add(Point);
                AddPoints(-1, Elements.Water);
            }
            else if (index == 2)
            {
                currentFireMoveCurency.Add(Point);
                AddPoints(-1, Elements.Fire);
            }
            else if (index == 3)
            {
                currentAirMoveCurency.Add(Point);
                AddPoints(-1, Elements.Air);
            }
            else if (index == 4)
            {
                currentDarkMoveCurency.Add(Point);
                AddPoints(-1, Elements.All);
            }
        }
        StopCoroutine(MoveParticle(Target, index));
    }

    public void SetTarget(GameObject Target, int i)
    {
        targets[i] = Target;
    }

    void SelectParticle(int index, List<ParticleSystem> list)
    {
        if (moveParticle[index])
        {
            if (currentProduct.currentValues[index] == currentProduct.price[index])
            {
                moveParticle[index] = false;

                foreach (var item in list)
                {
                    Destroy(item.gameObject);
                }

                list.Clear();

                UpdatePoints();
            }

            foreach (var item in list)
            {
                item.transform.position = Vector2.Lerp(item.transform.position, targets[index].transform.position, speed * Time.deltaTime);

                if (Vector2.Distance(targets[index].transform.position, item.transform.position) <= 1 && item.gameObject.activeInHierarchy)
                {
                    item.gameObject.SetActive(false);
                    currentProduct.AddValues(index);
                }
            }
        }
    }

    public void UpdatePoints()
    {
        values[0] = earthPoints;
        values[1] = waterPoints;
        values[2] = firePoints;
        values[3] = airPoints;
        values[4] = darkPoints;

        for (int i = 0; i < pointsUI.Length; i++)
        {
            pointsUI[i].maxParticles = values[i];
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[values[i]];
            pointsUI[i].GetParticles(particles);
            pointsUI[i].SetParticles(particles);
            pointsText[i].text = values[i].ToString() + "/" + maxPoints.ToString();
        }

        unlockSystem.SaveCurrency();
    }

    public void DeletePoints()
    {
        earthPoints = 0;
        waterPoints = 0;
        firePoints = 0;
        airPoints = 0;
        darkPoints = 0;

        for (int i = 0; i < pointsUI.Length; i++)
        {
            pointsUI[i].Clear();
        }
        UpdatePoints();
    }
}

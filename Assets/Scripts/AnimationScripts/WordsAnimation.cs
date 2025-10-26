using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsAnimation : MonoBehaviour
{
    private Animator animator;
    public float minSpeed, maxSpeed;
    public int wordNumbers;

    public int currentNumber;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void NextWord()
    {
        float speed = Random.Range(minSpeed, maxSpeed);
        int randomNumber = Random.Range(0, wordNumbers);

        if(randomNumber == currentNumber)
        {
            NextWord();
        }
        else
        {
            animator.SetFloat("Speed", speed);
            animator.SetInteger("Number", randomNumber);
            currentNumber = randomNumber;
        }
    }
}

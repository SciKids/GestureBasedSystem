using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MathGameV2 : MonoBehaviour
{
    public GameObject[] numbers = new GameObject[4];
    public Text equationText, scoreText, timer;
    public int maxNumberAvailable;

    private int level, ans, ansChosen, score;
    private const int SCORE_VAL = 10;
    private bool ready = false;
    // IEnumerator runLevelCoroutine;

    private void Awake()
    {
        // Hide all of the numbers
        for (int i = 0; i < 4; i++)
        {
            numbers[i].SetActive(false);
        }

        ans = -1;
        ansChosen = -1;
        level = 0;
    }

    void Start()
    {
        //runLevelCoroutine = RunLevel();
        StartCoroutine(TestTime());
        StartCoroutine(StartGameAfterTime(5f));
    }

    IEnumerator StartGameAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        ready = true;
    }

    private void Update()
    {
        if (ready)
        {
            Debug.Log("READY");
            StartCoroutine(RunLevel());
            ready = false;
        }

        if (score == 30)
        {
            scoreText.text = "Your score: " + score + " -> LEVEL UP!";
            level++;
        }
        if (score == 60)
        {
            scoreText.text = "Your score: " + score + " -> LEVEL UP!";
            level++;
        }
    }

    IEnumerator RunLevel()
    {
        if (level == 0)
        {
            CreateEquation(1, 4);
        }
        else if (level == 1)
        {
            CreateEquation(1, 10);
        }
        else
        {
            CreateEquation(1, 20);
        }
        StartCoroutine(TestTime());

        yield return new WaitForSeconds(5f);

        equationText.text = "";

        StartCoroutine(DestroySprites());

    }

    void CreateEquation(int min, int max)
    {
        System.Random rand = new System.Random();
        int first = rand.Next(min, max);
        int second = rand.Next(min, max);

        ans = first + second;

        Debug.Log("Equation created: " + first + " + " + second + " = " + ans);

        equationText.text = first.ToString() + " + " + second.ToString();

        LoadSprites(max);

    }

    void LoadSprites(int max)
    {
        System.Random rand = new System.Random();

        int randAnsIndex = rand.Next(0, 4); // Randomly place answer in array

        int i = 0;

        int[] numbersUsed = new int[3];

        for (int k = 0; k < 3; k++)
        {
            numbersUsed[k] = 0; // Placeholder numbers
        }

        int j = 0;

        while (i != 4)
        {
            if (i == randAnsIndex)
            {
                numbers[i].SetActive(true);
                Sprite ansSprite = Resources.Load<Sprite>("Images/MathGameImages/" + ans.ToString());
                numbers[i].GetComponent<SpriteRenderer>().sprite = ansSprite;
                numbers[i].SendMessage("SetNum", ans);
                numbers[i].SendMessage("SetCorrect", true);
                i++;
            }
            else
            {
                int randWrongAns = rand.Next(1, max);
                if (randWrongAns != ans && ContainsInt(numbersUsed, randWrongAns) == false)
                {
                    numbers[i].SetActive(true);
                    Sprite numSprite = Resources.Load<Sprite>("Images/MathGameImages/" + randWrongAns.ToString());
                    numbers[i].GetComponent<SpriteRenderer>().sprite = numSprite;
                    numbers[i].SendMessage("SetNum", randWrongAns);
                    numbers[i].SendMessage("SetCorrect", false);
                    numbersUsed[j] = randWrongAns;
                    i++;
                    j++;
                }
                else
                {
                    continue;
                }
            }
        }
    }

    IEnumerator DestroySprites()
    {
        for (int i = 0; i < 4; i++)
        {
            numbers[i].SetActive(false);
        }

        equationText.text = "";

        StartCoroutine(TestTime());
        yield return new WaitForSeconds(5f);

        ready = true;
    }

    public void PressAnswer(int num)
    {
        ansChosen = num;

        if (ansChosen == ans)
        {
            score += SCORE_VAL;
        }
        else
        {
            score -= SCORE_VAL;
        }

        scoreText.text = "Your score: " + score;

        StopAllCoroutines();

        StartCoroutine(DestroySprites());
    }

    IEnumerator TestTime(int time = 5)
    {
        for (int i = time; i > 0; i--)
        {
            timer.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        timer.text = "";
    }

    bool ContainsInt(int[] array, int num)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == num)
            {
                return true;
            }
        }
        return false;
    }
}

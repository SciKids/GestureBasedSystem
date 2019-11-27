using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MathGame : MonoBehaviour
{
    public static MathGame instance = null;
    public Text equationTxt, scoreText;
    public GameObject numPrefab, objectSpawner;
    public int maxNumber;

    private int level, ans, ansChosen, score;
    private bool ready = false;
    private GameObject[] objectsOnScreen = new GameObject[4];
    float[] positions = { -450, -150, 150, 450 };
    Sprite[] numberSprites;

    IEnumerator runLevelCoroutine;
    //Awake is called when the script instance is being loaded.
    void Awake()
    {
        //Load spritesheet
        // numberSprites = Resources.LoadAll<Sprite>("Images/MathGame/0thru13");

        /*for (int i = 0; i < numberSprites.Length; i++)
        {
            Debug.Log("Loaded " + numberSprites[i].name + " from spritesheet.");
        }
        */
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        numberSprites = new Sprite[maxNumber + 1];
        LoadAllSprites();
        //Begin();
    }
    private void Start()
    {
        runLevelCoroutine = RunLevel();
        StartCoroutine(StartGame(5f));
        StartCoroutine(TestTime(5));
        Debug.Log("la");
    }

    void LoadAllSprites()
    {
        for (int i = 0; i <= maxNumber; i++)
        {
            string numStr = i.ToString();

            numberSprites[i] = Resources.Load<Sprite>("Images/MathGameImages/" + numStr);

            Debug.Log(numberSprites[i].name + " was loaded");
        }
    }

    // Wrapper function that allows SegmentPaintV2 to begin game
    /*public void Begin()
    {
        StartCoroutine(StartGame(5f));
        StartCoroutine(TestTime(5));
    }
    */
    // Initializes all variables and run first level
    IEnumerator StartGame(float timeToWait)
    {
        ans = -1;
        ansChosen = -1;
        level = 0;
        yield return new WaitForSeconds(timeToWait);
        StartCoroutine(runLevelCoroutine);
    }

    /*void Test()
    {
        StartCoroutine(RunLevel());
    }
    */

    IEnumerator RunLevel()
    {
        try
        {
            Debug.Log("RUNLEVEL CALLED");

            ready = false;

            // First level = numbers being added are between 1 and 4.
            if (level == 0)
            {
                CreateEquation(1, 4);
            }
            // Second level = numbers being added are between 1 and 9.
            else if (level == 1)
            {
                CreateEquation(1, 9);
            }
            // Third level = numbers being added are between 1 and 19.
            else
            {
                CreateEquation(1, 19);
            }
        }
        catch(Exception e)
        {
            Debug.Log("CRASHED AT RunLevel W/ EXCEPTION - " + e);
        }
        

        StartCoroutine(TestTime(6));
        yield return new WaitForSeconds(6f);

        equationTxt.text = "";
        DestroySprites(true);
        //StartCoroutine(runLevelCoroutine);
        // ans = -1;
        // score = 0;

    }

    IEnumerator TestTime(int time)
    {
        int current = time;
        while (current != 0)
        {
            Debug.Log(current);
            current--;

            yield return new WaitForSeconds(1f);
        }

    }

    // Create a random equation between a min and max number.
    void CreateEquation(int min, int max)
    {
        try
        {
            System.Random rand = new System.Random();
            int first = rand.Next(min, max);
            int second = rand.Next(min, max);

            ans = first + second;

            Debug.Log("Equation created: " + first + " + " + second + " = " + ans);

            equationTxt.text = first.ToString() + " + " + second.ToString();

            LoadSprites(max);
        }
        catch(Exception e)
        {
            Debug.Log("CRASHED AT CreateEquation W/ EXCEPTION - " + e);
        }
    }

    // Load all sprites that will spawn at the top of the screen
    void LoadSprites(int max)
    {
        try
        {
            Debug.Log("Loading sprites");
            System.Random rand = new System.Random();

            int[] randArr = new int[4];

            // Place the answer randomly in the array.
            int randAnsIndex = rand.Next(0, 4);
            randArr[randAnsIndex] = ans;

            int i = 0;

            // Randomly place wrong numbers in the rest of array.
            while (i < 4)
            {
                if (i != randAnsIndex)
                {
                    int randWrongAns = rand.Next(1, max); // Get random wrong number

                    if (randWrongAns != ans && !randArr.Contains(randWrongAns))
                    {
                        //Debug.Log("Random wrong answer: " + randWrongAns);

                        randArr[i] = randWrongAns;

                        GameObject currentObj = Instantiate(numPrefab);

                        currentObj.name = "num" + i.ToString();

                        currentObj.transform.SetParent(objectSpawner.transform, true);

                        currentObj.GetComponent<SpriteRenderer>().sprite = numberSprites[randWrongAns];

                        Vector3 localPos = new Vector3(positions[i], (float)11.5, -9);

                        currentObj.transform.localPosition = localPos;

                        currentObj.SendMessage("SetNum", randWrongAns);

                        Debug.Log("Set WRONG num " + i + " as " + randWrongAns);

                        objectsOnScreen[i] = currentObj;

                        i++;

                    } // end inner if
                }// end outer if
                else
                {
                    GameObject currentObj = Instantiate(numPrefab);

                    currentObj.name = "num" + i.ToString();

                    currentObj.transform.SetParent(objectSpawner.transform, true);

                    currentObj.GetComponent<SpriteRenderer>().sprite = numberSprites[ans];

                    Vector3 localPos = new Vector3(positions[i], (float)11.5, -9);

                    currentObj.transform.localPosition = localPos;

                    currentObj.SendMessage("SetNum", ans);

                    Debug.Log("Set CORRECT num" + i + " as " + ans);

                    objectsOnScreen[i] = currentObj;

                    i++; // If we reached to the wrong answer index, skip over it.

                }
            }// end while loop

            /*for(int j = 0; j < 4; j++)
             {
                 Debug.Log("Object on screen: " + objectsOnScreen[j].name);
             }*/
        }
        catch(Exception e)
        {
            Debug.Log("CRASHED AT CreateEquation W/ EXCEPTION - " + e);
        }

    }

    public void DestroySprites(bool temp)
    {
        try
        {
            for (int i = 0; i < 4; i++)
            {
                // Debug.Log("Destroying " + objectsOnScreen[i].name);
                Destroy(objectsOnScreen[i]);
                objectsOnScreen[i] = null;

            }
            //StopAllCoroutines();
            //runLevelCoroutine = RunLevel();
            StartCoroutine(runLevelCoroutine);
            //StartCoroutine(RunLevel());
            // Test();
            // StartCoroutine(TestTime(3));
        }
        catch (Exception e)
        {
            Debug.Log("CRASHED AT DestroySprites W/ EXCEPTION - " + e);

        }
    }

    bool ArrayContainsInt(int[] arr, int num)
    {
        try
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == num)
                {
                    return true;
                }
            }

            return false;
        }
        catch(Exception e)
        {
            Debug.Log("CRASHED AT ArrayContainsInt W/ EXCEPTION - " + e);
            return false;
        }
        
    }

    public void PressAnswer(int userAns)
    {
        try
        {
            StopCoroutine(runLevelCoroutine);
            // StopAllCoroutines();

            if (userAns == ans)
            {
                score += 10;
            }
            else
            {
                score -= 10;
            }

            scoreText.text = "Score: " + score.ToString();

            DestroySprites(true);

            //runLevelCoroutine = RunLevel();
            //StartCoroutine(runLevelCoroutine);

            //StartCoroutine(RunLevel());
            //StartCoroutine(StartGame(3f));
            //StartCoroutine(StartGame(3f));
        }
        catch(Exception e)
        {
            Debug.Log("CRASHED AT PressAnswer W/ EXCEPTION - " + e);
        }


    }

}

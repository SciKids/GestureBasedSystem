using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckChairsTemp : MonoBehaviour
{
    public Camera cam;
    public GameObject[] chairs;
    public GameObject[] allWorkers;
    public Text feedbackText;
    public GameObject exitButton;

    private bool okToCheck = false;
    private bool wrongAnsPresent = false;
    private bool reset = false;
    private bool allAreCorrect = false;
    private GameObject[] workersInChairs;

    private void Start()
    {
        workersInChairs = new GameObject[chairs.Length];
        exitButton.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(wrongAnsPresent)
        {
            feedbackText.text = "Oops! Some of your answers were wrong. Press the reset button to try again.";
            wrongAnsPresent = false; // I don't want this to keep updating.
        }
        
        if(allAreCorrect)
        {
            feedbackText.text = "Good job! Everyone here plays an important role to the project.";
            allAreCorrect = false;
            exitButton.SetActive(true);
        }
        
    }
    
    public void ResetScreen()
    {
        Debug.Log("ResetScreen called");
        
        if(!(workersInChairs.Length == 0))
        {
            for (int i = 0; i < workersInChairs.Length; i++)
            {
                Debug.Log("Resetting " + workersInChairs[i].name);

                GameObject feedbackPic = workersInChairs[i].transform.Find("FeedbackPic").gameObject;

                feedbackPic.SetActive(false);

            }
        }
        
    }

    public void ReadyToCheck()
    {
        Debug.Log("ReadyToCheck called");
        StartCoroutine(Evaluate());
    }

    IEnumerator Evaluate()
    {
        Debug.Log("Evaluate called");

        yield return new WaitForSeconds(2);

        int workersInChairsIndex = 0;

        for (int i = 0; i < chairs.Length; i++)
        {
            GameObject chairCube = chairs[i].transform.Find("Cube").gameObject;

            Vector3 cubePos = chairCube.transform.position;

            for (int j = 0; j < allWorkers.Length; j++)
            {
                if (Mathf.Abs(allWorkers[j].transform.position.x - cubePos.x) <= 10)
                {
                    workersInChairs[workersInChairsIndex] = allWorkers[j]; // Saving the evaluated workers for resetting purposes

                    Debug.Log("Added " + workersInChairs[workersInChairsIndex].name + " to position " + workersInChairsIndex);

                    workersInChairsIndex++;

                    Debug.Log("Evaluating " + allWorkers[j].name);

                    GameObject feedback = allWorkers[j].transform.Find("FeedbackPic").gameObject;

                    feedback.SetActive(true);
                    
                    if (feedback.GetComponent<SpriteRenderer>().sprite.name == "Wrong")
                    {
                        wrongAnsPresent = true;
                    }

                    break;
                }
            }

        }

        if(!wrongAnsPresent)
        {
            allAreCorrect = true;
        }
    }
}

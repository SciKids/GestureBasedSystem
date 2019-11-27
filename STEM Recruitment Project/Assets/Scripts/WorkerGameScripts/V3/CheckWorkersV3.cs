using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckWorkersV3 : MonoBehaviour
{
    public GameObject exitButton, nextButton, resetButton, loadingCircle;
    public Text feedback;
    private GameObject[] workersToCheck = new GameObject[3];
    private bool okToCheck = false;
    private int numOfWorkers = 0;
    private bool workersChecked = false;
    private bool allIsCorrect = false;

    private void Start()
    {
        exitButton.SetActive(false);
        nextButton.SetActive(false);
        loadingCircle.SetActive(false);
    }
    private void Update()
    {
        if (okToCheck)
        {
            nextButton.SetActive(true);

            okToCheck = false;
        }

        if (allIsCorrect)
        {
            exitButton.SetActive(true);
            nextButton.SetActive(false);
            resetButton.SetActive(false);
            SendStatusMessageToAllWorkers(true);
            allIsCorrect = false;
        }
        if (!AllChairsFull())
        {
            nextButton.SetActive(false);
        }
    }

    public void AddWorker(GameObject newWorker)
    {
        Debug.Log("AddWorker called");
        if (!workerAlreadyInArray(newWorker))
        {
            for (int i = 0; i < 3; i++)
            {
                if (workersToCheck[i] != null)
                {
                    Debug.Log(workersToCheck[i].name + " is already in index " + i);
                }

                if (workersToCheck[i] == null)
                {
                    Debug.Log("index " + i + " is empty");
                    workersToCheck[i] = newWorker;
                    break;
                }
            }

            if (numOfWorkers < 3)
            {
                numOfWorkers++;
            }

            if (numOfWorkers == 3)
            {
                okToCheck = true;
            }

            else
            {
                okToCheck = false;
            }

            Debug.Log(newWorker.name + " has been added");
        }

        Debug.Log("Workers in list after ADD: ");
        for (int i = 0; i < workersToCheck.Length; i++)
        {
            if (workersToCheck[i] != null)
            {
                Debug.Log(workersToCheck[i].name);
            }
        }
    }

    public void DeleteWorker(GameObject newWorker)
    {
        Debug.Log("DeleteWorker called");
        for (int i = 0; i < 3; i++)
        {
            if (workersToCheck[i] == newWorker)
            {
                workersToCheck[i] = null;
                if (numOfWorkers > 0)
                {
                    numOfWorkers--;
                }

                if (numOfWorkers < 3)
                {
                    okToCheck = false;
                }

                Debug.Log(newWorker.name + " has been deleted");
            }
        }

        Debug.Log("Workers in list after DELETE: ");
        for (int i = 0; i < workersToCheck.Length; i++)
        {
            if (workersToCheck[i] != null)
            {
                Debug.Log(workersToCheck[i].name);
            }
        }
    }

    public void CallShowFeedback()
    {
        StartCoroutine(ShowFeedback());
    }

    public IEnumerator ShowFeedback()
    {
        BlockWorkers(true);

        loadingCircle.SetActive(true);

        yield return new WaitForSeconds(5.0f);

        loadingCircle.SetActive(false);

        for (int i = 0; i < workersToCheck.Length; i++)
        {
            GameObject feedbackPic = workersToCheck[i].transform.Find("FeedbackPic").gameObject;

            feedbackPic.SetActive(true);

            if (feedbackPic.GetComponent<SpriteRenderer>().sprite.name == "Wrong")
            {
                SendStatusMessageToAllWorkers(false); // Telling all workers that there is a wrong answer present
            }
        }
        allIsCorrect = RecheckCorrectness();

        BlockWorkers(false);

        workersChecked = true;
    }
    bool RecheckCorrectness()
    {
        for (int i = 0; i < workersToCheck.Length; i++)
        {
            GameObject feedbackPic = workersToCheck[i].transform.Find("FeedbackPic").gameObject;

            feedbackPic.SetActive(true);

            if (feedbackPic.GetComponent<SpriteRenderer>().sprite.name == "Wrong")
            {
                return false;
            }
        }

        return true;
    }
    void SendStatusMessageToAllWorkers(bool status)
    {
        for (int i = 1; i <= 6; i++)
        {
            string workerName = "Worker" + i.ToString();

            string fullPath = "/WorkerCanvas/WorkerScreen/" + workerName;

            GameObject worker = GameObject.Find(fullPath);

            GameObject canvas = worker.transform.Find("Canvas").gameObject;

            canvas.SendMessage("ReceiveCorrectness", status);

            // Only needs to send to one worker, since this effects an Update function.
            if (i == 1)
            {
                canvas.SendMessage("ShowOverallFeedback", true);
            }
        }


    }

    void BlockWorkers(bool status)
    {
        if (status) // If a worker has to be blocked, find both arms and block movement
        {
            for (int i = 1; i <= 6; i++)
            {
                string workerName = "Worker" + i.ToString();

                string fullPath = "/WorkerCanvas/WorkerScreen/" + workerName;

                GameObject worker = GameObject.Find(fullPath);

                GameObject rightArm = worker.transform.Find("RightArm").gameObject;

                GameObject leftArm = worker.transform.Find("LeftArm").gameObject;

                rightArm.SendMessage("SetOkToLift", false);

                leftArm.SendMessage("SetOkToLift", false);
            }
        }

        // If a worker can be moved, unblock both arms/
        else
        {
            for (int i = 1; i <= 6; i++)
            {
                string workerName = "Worker" + i.ToString();

                string fullPath = "/WorkerCanvas/WorkerScreen/" + workerName;

                GameObject worker = GameObject.Find(fullPath);

                GameObject rightArm = worker.transform.Find("RightArm").gameObject;

                GameObject leftArm = worker.transform.Find("LeftArm").gameObject;

                rightArm.SendMessage("SetOkToLift", true);

                leftArm.SendMessage("SetOkToLift", true);
            }
        }
    }

    public void Reset()
    {
        if (workersChecked)
        {
            for (int i = 0; i < workersToCheck.Length; i++)
            {
                workersToCheck[i].transform.Find("FeedbackPic").gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < numOfWorkers; i++)
        {
            if (workersToCheck[i] != null)
            {
                workersToCheck[i].SendMessage("SendBack", true);
            }
        }

        nextButton.SetActive(false);

        SendWorkersChecked(false);
        //SendStatusMessageToAllWorkers(true); // reset the "oneIsWrong" variable
    }

    public void SendWorkersChecked(bool status)
    {
        for (int i = 1; i < 7; i++)
        {
            string workerStr = "Worker" + i.ToString();

            GameObject workerObj = GameObject.Find("/WorkerCanvas/WorkerScreen/" + workerStr);

            GameObject workerCanvas = workerObj.transform.Find("Canvas").gameObject;

            workerCanvas.SendMessage("SetWorkersAreChecked", status);
        }
    }

    bool AllChairsFull()
    {
        if (Array.Exists(workersToCheck, null))
        {
            return false;
        }
        return true;
    }

    bool workerAlreadyInArray(GameObject worker)
    {
        for (int i = 0; i < 3; i++)
        {
            if (workersToCheck[i] == worker)
            {
                return true;
            }
        }

        return false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script Summary ////////////////////////////////////////////////////////////
/*
 * Adds and removes workers to and from array, checks each workers correctness, 
 * and can reset scene
 */ 

public class CheckWorkersV3 : MonoBehaviour
{
    public GameObject exitButton, nextButton, resetButton, loadingCircle;
    public Text feedback;
    private GameObject[] workersToCheck = new GameObject[3];
    private bool okToCheck = false; // Used to set next button to true/false
    private int numOfWorkers = 0; // Keeps track of workers currently in array
    private bool workersChecked = false; 
    private bool allIsCorrect = false;
    private GameObject dummyWorker;
    
    // Set necessary buttons and loading circle to inactive
    private void Start()
    {
        //exitButton.SetActive(false);
        nextButton.SetActive(false);
        loadingCircle.SetActive(false);
        dummyWorker = new GameObject("dummy");
        for(int i = 0; i < 3; i++)
        {
            workersToCheck[i] = dummyWorker;
        }

    }// end Start
    
    private void Update()
    {
        // If all 3 chairs are filled, set next button to true. 
        // This will allow the user to have the chosen workers evaluated.
        if (okToCheck)
        {
            nextButton.SetActive(true);

            okToCheck = false;
            
        }
        // If the user has chosen the 3 correct workers, show the exit button
        // and hide the next and reset buttons.
        if (allIsCorrect)
        {
            //exitButton.SetActive(true);
            nextButton.SetActive(false);
            resetButton.SetActive(false);
            SendStatusMessageToAllWorkers(true);
            BlockWorkers(true);// Blocking further worker movement
            allIsCorrect = false; // Resetting this to false so that this block only runs once
            
        }

        // Keep the next button inactive if all of the chairs are not full.
        if (!AllChairsFull())
        {
            nextButton.SetActive(false);
        }
    } // end Update

    // Adds worker to array. This is called from the chair objects once a worker is dropped into it.
    public void AddWorker(GameObject newWorker)
    {
        Debug.Log("AddWorker called");
        // If the worker is not already in the array, add the worker.
        if (!workerAlreadyInArray(newWorker))
        {
            for (int i = 0; i < 3; i++)
            {
                if (workersToCheck[i] != dummyWorker)
                {
                    // Just a print statement so that I can see what's going on.
                    Debug.Log(workersToCheck[i].name + " is already in index " + i);
                }
                // Find the empty spot in the array
                if (workersToCheck[i] == dummyWorker)
                {
                    Debug.Log("index " + i + " is empty");
                    workersToCheck[i] = newWorker; // Place worker in index.
                    break;
                }
            }
            // After adding a new worker, increment numOfWorkers. The max is 3.
            if (numOfWorkers < 3)
            {
                numOfWorkers++;
            }

            // Once numOfWorkers is maxed out, set okToCheck to true. This will allow the 
            // user to choose to evaluate the workers chosen.
            if (numOfWorkers == 3)
            {
                okToCheck = true;
            }

            // Block the next button while numOfWorkers < 3
            else
            {
                okToCheck = false;
            }

            Debug.Log(newWorker.name + " has been added");
        } // end if(!workerAlreadyInArray(newWorker))

        /*
        Debug.Log("Workers in list after ADD: ");
        for (int i = 0; i < workersToCheck.Length; i++)
        {
            if (workersToCheck[i] != null)
            {
                Debug.Log(workersToCheck[i].name);
            }
        }*/
    } // end AddWorker

    // Removes worker from array. This is called from a chair when a worker leaves it.
    public void DeleteWorker(GameObject newWorker)
    {
        Debug.Log("DeleteWorker called");
        // Find the worker being deleted
        for (int i = 0; i < 3; i++)
        {
            // Once we find the worker
            if (workersToCheck[i] == newWorker)
            {
                // Delete the worker
                workersToCheck[i] = dummyWorker;
                // Make sure numOfWorkers doesn't go below 0 before decrementing it.
                if (numOfWorkers > 0)
                {
                    numOfWorkers--;
                }
                // Make sure user is blocked from evaluating workers if there aren't 3 chosen.
                if (numOfWorkers < 3)
                {
                    okToCheck = false;
                }

                Debug.Log(newWorker.name + " has been deleted");
            }
        } //end for loop
        /*
        Debug.Log("Workers in list after DELETE: ");
        for (int i = 0; i < workersToCheck.Length; i++)
        {
            if (workersToCheck[i] != null)
            {
                Debug.Log(workersToCheck[i].name);
            }
        }*/
    }// end DeleteWorker

    // Wrapper function to start the ShowFeedback timer
    public void CallShowFeedback()
    {
        StartCoroutine(ShowFeedback());
    }

    // "Evaluates" the workers in the array after the loading circle completes a cycle.
    public IEnumerator ShowFeedback()
    {
        // Block workers from moving during evaluation
        BlockWorkers(true);

        // Set the loading circle to true. This will start the animation.
        loadingCircle.SetActive(true);

        // The loading circle takes 5 seconds to complete, so wait until this is done.
        yield return new WaitForSeconds(5.0f);

        loadingCircle.SetActive(false);

        // Go through the workersToCheck array
        for (int i = 0; i < workersToCheck.Length; i++)
        {
            // Get the feedback pic and activate it
            GameObject feedbackPic = workersToCheck[i].transform.Find("FeedbackPic").gameObject;

            feedbackPic.SetActive(true);

            // If the feedback pic's name is "wrong", make sure all workers know that there is a wrong answer.
            if (feedbackPic.GetComponent<SpriteRenderer>().sprite.name == "Wrong")
            {
                SendStatusMessageToAllWorkers(false); // Telling all workers that there is a wrong answer present
            }
        }// end for loop

        // Rechecks the array in order to assign a value to allIsCorrect.
        allIsCorrect = RecheckCorrectness();

        // Unblock workers
        //BlockWorkers(false);

        // Used in Reset()
        workersChecked = true;
    }// end ShowFeedback

    // Used to set allIsCorrect to a value
    bool RecheckCorrectness()
    {
        // Go through worker array
        for (int i = 0; i < workersToCheck.Length; i++)
        {
            // Find the feedback picture
            GameObject feedbackPic = workersToCheck[i].transform.Find("FeedbackPic").gameObject;

            feedbackPic.SetActive(true);

            // If the picture uses the "Wrong" sprite, return false
            if (feedbackPic.GetComponent<SpriteRenderer>().sprite.name == "Wrong")
            {
                return false;
            }
        }
        // If none of the sprites used the "Wrong" sprite, return true.
        return true;
    }// end RecheckCorrectness
    
    // Go through each worker to tell them if there is a wrong answer present or not.
    void SendStatusMessageToAllWorkers(bool status)
    {
        // Loop through each worker on the screen
        for (int i = 1; i <= 6; i++)
        {
            // Find the worker. Worker path & name is formatted as 
            // "/WorkerCanvas/WorkerScreen/Worker#"
            string workerName = "Worker" + i.ToString();
            
            string fullPath = "/WorkerCanvas/WorkerScreen/" + workerName;

            GameObject worker = GameObject.Find(fullPath);

            // Find the worker's canvas
            GameObject canvas = worker.transform.Find("Canvas").gameObject;

            // Send the status value to ReceiveCorrectness() in 
            canvas.SendMessage("ReceiveCorrectness", status);

            // Only needs to send to one worker, since this effects an Update function.
            // NOTE: Check this to make sure it is ok
            if (i == 1)
            {
                canvas.SendMessage("ShowOverallFeedback", true);
            }
        } // end for loop
    }// end SendStatusMessageToAllWorkers

    // Blocks all workers' movement
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

                rightArm.SendMessage("SetOkToLift", false); // Block the right arm from being lifted

                leftArm.SendMessage("SetOkToLift", false); // Block the left arm from being lifted
            }// end for loop
        }// end if(status)

        // If a worker can be moved, unblock both arms
        else
        {
            for (int i = 1; i <= 6; i++)
            {
                string workerName = "Worker" + i.ToString();

                string fullPath = "/WorkerCanvas/WorkerScreen/" + workerName;

                GameObject worker = GameObject.Find(fullPath);

                GameObject rightArm = worker.transform.Find("RightArm").gameObject;

                GameObject leftArm = worker.transform.Find("LeftArm").gameObject;

                rightArm.SendMessage("SetOkToLift", true); // Unblock right arm 

                leftArm.SendMessage("SetOkToLift", true); // Unblock left arm
            }// end for loop
        }// end else

    }// end BlockWorkers
    
    // Resets the screen by deactivating each workers' feedbackPic and sending them back to their
    // original position. 
    public void Reset()
    {
        // If workers has been checked, deactivate the feedbackPics
        if (workersChecked)
        {
            for (int i = 0; i < workersToCheck.Length; i++)
            {
                workersToCheck[i].transform.Find("FeedbackPic").gameObject.SetActive(false);
            }
        }

        // Send each worker in workersToCheck back to their original position
        for (int i = 0; i < numOfWorkers; i++)
        {
            if (workersToCheck[i] != dummyWorker)
            {
                workersToCheck[i].SendMessage("SendBack", true);
            }
        }
       
        nextButton.SetActive(false); // Hide next button

        SendWorkersChecked(false);

        BlockWorkers(false); // Make sure workers can be moved.
        //SendStatusMessageToAllWorkers(true); // reset the "oneIsWrong" variable
    }// end Reset

    // Goes through each worker to tell them if the user's choices has been evaluated or not.
    public void SendWorkersChecked(bool status)
    {
        // Find each worker on the screen
        for (int i = 1; i < 7; i++)
        {
            string workerStr = "Worker" + i.ToString();

            GameObject workerObj = GameObject.Find("/WorkerCanvas/WorkerScreen/" + workerStr);

            GameObject workerCanvas = workerObj.transform.Find("Canvas").gameObject;

            // Send the value of status to SetWorkersAreChecked in ShowTitleAndDescriptionV2
            workerCanvas.SendMessage("SetWorkersAreChecked", status);
        }
    }// end SendWorkersChecked

    // If there is one empty spot in the array, return false/
    bool AllChairsFull()
    {
        for (int i = 0; i < 3; i++)
        {
            if(workersToCheck[i] == dummyWorker)
            {
                return false;
            }
        }
        return true;
    }// end AllChairsFull

    // If a worker is already present in the array, return true.
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
    } // end workerAlreadyInArray()
}// end CheckWorkersV3

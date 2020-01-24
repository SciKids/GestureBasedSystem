using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script Summary ////////////////////////////////////////////////////////////
/*
 * Attached to chair object. Sends messages to CheckWorkersV3 as workers enter and leave 
 * the chair attached.
 */

public class HandleWorkerV2 : MonoBehaviour
{
    public GameObject rightHand, leftHand;

    private bool isFilled = false; // worker is/isn't in chair
    private bool readyToPlace = false;
    private bool readyToRemove = false;
    private bool blocked = false;
    private GameObject worker = null; //worker in chair
    private GameObject newWorker = null; //temporary worker that touched chair
    private GameObject placementCube;
    private GameObject workerScreen;

    // Find placement cube and worker screen.
    void Start()
    {
        // Get the cube, so I know where to place workers.
        placementCube = this.transform.Find("Cube").gameObject;

        // Get full worker screen
        workerScreen = GameObject.Find("/WorkerCanvas");
    }// end start

    // Checks for workers who are placed into chair
    private void Update()
    {
        // If the user has dropped a worker into the chair, call PutWorkerInChair
        if (readyToPlace && UserLetGo())
        {
            PutWorkerInChair(newWorker);

            readyToPlace = false; // Only wanna do this once at a time

            Debug.Log(this.name + " has " + worker.name);
        }// end if

    }// end update

    // If a worker enters the chair's collision box, temporarily save that worker
    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.tag == "Worker")
        {
            Debug.Log("Worker touched meh!!");
            newWorker = other.gameObject;

            readyToPlace = true;
        }

    }// end OnTriggerEnter

    // If a worker has left the collision box, delete worker 
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Worker")
        {
            Debug.Log("Worker left meh!!");
            WorkerLeftChair(other.gameObject);
        }

    }// end OnTriggerExit

    // Puts worker into array in CheckWorkersV3
    void PutWorkerInChair(GameObject workerObj)
    {
        Debug.Log("PutWorkerInChair called.");

        // If the chair is filled, kick previous worker out.
        if (isFilled && (workerObj != worker) && !blocked)
        {
            Debug.Log("Kicking out " + worker.name);

            workerScreen.SendMessage("DeleteWorker", worker);

            worker.SendMessage("SendBack", true); // Send previous worker back

            // This ensures that worker won't move to other chair while moving back to their original
            // position.
            BlockAllOtherChairs(true); 

        }// end if 

        Debug.Log("Placing worker " + workerObj.name + " in " + this.name);

        // send the workerObj to AddWorker in CheckWorkersV3. This adds the worker into the array found in 
        // CheckWorkersV3
        workerScreen.SendMessage("AddWorker", workerObj);

        // Change the worker's position to the placement cube's position.
        workerObj.transform.position = placementCube.transform.position;

        // save the workerObj to worker
        worker = workerObj;

        newWorker = null;

        // used if another worker wants to be let in (line 79)
        isFilled = true;

    }// end PutWorkerInChair

    // Deletes worker from variables here and from the array found in CheckWorkersV3
    void WorkerLeftChair(GameObject workerObj)
    {
        // Making sure that the chair is filled and the worker in the chair is the same as the given worker.
        if (isFilled && workerObj == worker)
        {
            isFilled = false;

            worker = null;

            // Delete from array in CheckWorkersV3
            workerScreen.SendMessage("DeleteWorker", workerObj);
        }
    }// end WorkerLeftChair

    // Reused function. If the distance between the user's left and right hand is greater than 3.5 unity units,
    // return true. Otherwise, return false.
    bool UserLetGo()
    {
        if (rightHand.transform.position.x - leftHand.transform.position.x > 3.5)
        {
            return true;
        }

        else if ((rightHand.transform.position.y - leftHand.transform.position.y > 3.5) ||
                (leftHand.transform.position.y - rightHand.transform.position.y > 3.5))
        {
            return true;
        }

        return false;
    }// end UserLetGo

    // Blocks other chairs from grabbing workers if they are being sent back to their original position.
    void BlockAllOtherChairs(bool status)
    {
        // Go through each chair
        for (int i = 1; i <= 3; i++)
        {
            // Find the chair object (labeled as "Chair#")
            string otherChairStr = "Chair" + i.ToString();
            GameObject otherChair = GameObject.Find("/WorkerCanvas/WorkerScreen/" + otherChairStr);

            // Skipping chair this script is attached to
            if (otherChair != this.gameObject)
            {
                // Send a message to BlockThisChair (below) to block chair.
                otherChair.SendMessage("BlockThisChair", status);
            }
        }// end for loop

    }// end BlockAllOtherChairs

    // Block this chair from grabbing workers
    public void BlockThisChair(bool status)
    {
        blocked = status;

    }// end BlockThisChair

}// end HandleWorkerV2

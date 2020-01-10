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

        workerScreen.SendMessage("AddWorker", workerObj);

        workerObj.transform.position = placementCube.transform.position;

        worker = workerObj;

        newWorker = null;

        isFilled = true;

    }// end PutWorkerInChair

    // 
    void WorkerLeftChair(GameObject workerObj)
    {
        if (isFilled && workerObj == worker)
        {
            isFilled = false;

            worker = null;

            workerScreen.SendMessage("DeleteWorker", workerObj);
        }
    }

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
    }

    void BlockAllOtherChairs(bool status)
    {
        for (int i = 1; i <= 3; i++)
        {
            string otherChairStr = "Chair" + i.ToString();

            GameObject otherChair = GameObject.Find("/WorkerCanvas/WorkerScreen/" + otherChairStr);

            if (otherChair != this.gameObject)
            {
                otherChair.SendMessage("BlockThisChair", status);
            }
        }
    }

    public void BlockThisChair(bool status)
    {
        blocked = status;
    }
}

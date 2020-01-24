using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script Summary ////////////////////////////////////////////////////////////
/*
 * This script blocks all workers from being picked up if the star is picked up 
 */

public class BlockAllWorkersV2 : MonoBehaviour
{
    public int numOfWorkers = 6;

    private SpriteRenderer rightImage, leftImage;
    private bool workerGrabbed = false;
    private GameObject workerScreen;

    // Find both halves of star & worker screen
    void Start()
    {
        GameObject rightHalf = this.transform.Find("RightHalfStar").gameObject;
        GameObject leftHalf = this.transform.Find("LeftHalfStar").gameObject;

        workerScreen = GameObject.Find("/WorkerCanvas/WorkerScreen");

        rightImage = rightHalf.GetComponent<SpriteRenderer>();
        leftImage = leftHalf.GetComponent<SpriteRenderer>();

    }// end Start

    // Blocks and unblocks workers as needed
    void Update()
    {
        // Making sure the worker screen is active to begin with.
        if(workerScreen.activeSelf)
        {
            // If the star or worker is grabbed, block all workers.
            if ((IsGrabbed(rightImage) && IsGrabbed(leftImage)) || workerGrabbed)
            {
                BlockWorkers(true);
            }
            // Otherwise, unblock all workers
            else
            {
                BlockWorkers(false);
            }
        }// end if worker screen active
    }// end Update

    // If a given image is enabled, return true.
    bool IsGrabbed(SpriteRenderer image)
    {
        if (image.enabled == true)
        {
            return true;
        }
        return false;
    }

    // Finds all workers on screen and blocks/unblocks their movement
    void BlockWorkers(bool status)
    {
        if (status) // If a worker has to be blocked, find both arms and block movement
        {
            for (int i = 1; i <= numOfWorkers; i++)
            {
                string workerName = "Worker" + i.ToString();

                string fullPath = "/WorkerCanvas/WorkerScreen/" + workerName;

                GameObject worker = GameObject.Find(fullPath);

                GameObject rightArm = worker.transform.Find("RightArm").gameObject;

                GameObject leftArm = worker.transform.Find("LeftArm").gameObject;

                rightArm.SendMessage("SetOkToLift", false);

                leftArm.SendMessage("SetOkToLift", false);
            }
        }// end if 

        // If a worker can be moved, unblock both arms/
        else
        {
            for (int i = 1; i <= numOfWorkers; i++)
            {
                string workerName = "Worker" + i.ToString();

                string fullPath = "/WorkerCanvas/WorkerScreen/" + workerName;

                GameObject worker = GameObject.Find(fullPath);

                GameObject rightArm = worker.transform.Find("RightArm").gameObject;

                GameObject leftArm = worker.transform.Find("LeftArm").gameObject;

                rightArm.SendMessage("SetOkToLift", true);

                leftArm.SendMessage("SetOkToLift", true);
            }
        }// end else
    }// end BlockWorkers

    // This is called through LiftWorkerAndDrag
    public void WorkerIsGrabbed(bool status)
    {
        workerGrabbed = status;
    }
}// end BlockAllWorkersV2

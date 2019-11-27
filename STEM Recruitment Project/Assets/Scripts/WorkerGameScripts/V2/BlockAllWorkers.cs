using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script just blocks all workers from being picked up if the star is picked up 
 */

public class BlockAllWorkers : MonoBehaviour
{
    public int numOfWorkers = 6;

    private SpriteRenderer rightImage, leftImage;
    private bool workerGrabbed = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject rightHalf = this.transform.Find("RightHalfStar").gameObject;
        GameObject leftHalf = this.transform.Find("LeftHalfStar").gameObject;
        
        rightImage = rightHalf.GetComponent<SpriteRenderer>();
        leftImage = leftHalf.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if ((IsGrabbed(rightImage) && IsGrabbed(leftImage)) || workerGrabbed)
        {
            BlockWorkers(true);
        }

        else
        {
            BlockWorkers(false);
        }
    }

    bool IsGrabbed(SpriteRenderer image)
    {
        if (image.enabled == true)
        {
            return true;
        }
        return false;
    }

    void BlockWorkers(bool status)
    {   
        if(status) // If a worker has to be blocked, find both arms and block movement
        {
            for(int i = 1; i <= numOfWorkers; i++)
            {
                string workerName = "Worker" + i.ToString();

                string fullPath = "/WorkerCanvas/WorkerScreen/Canvas/" + workerName;

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
            for (int i = 1; i <= numOfWorkers; i++)
            {
                string workerName = "Worker" + i.ToString();

                string fullPath = "/WorkerCanvas/WorkerScreen/Canvas/" + workerName;

                GameObject worker = GameObject.Find(fullPath);

                GameObject rightArm = worker.transform.Find("RightArm").gameObject;

                GameObject leftArm = worker.transform.Find("LeftArm").gameObject;

                rightArm.SendMessage("SetOkToLift", true);

                leftArm.SendMessage("SetOkToLift", true);
            }
        }
    }

    public void WorkerIsGrabbed(bool status)
    {
        workerGrabbed = status;
    }
}

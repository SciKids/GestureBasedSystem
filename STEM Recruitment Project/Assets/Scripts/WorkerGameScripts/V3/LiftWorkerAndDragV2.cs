using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script Summary ////////////////////////////////////////////////////////////
/*
 * If both hands are "lifted" (both up-arm images are enabled), the user can 
 * drag the worker around.
 */

public class LiftWorkerAndDragV2 : MonoBehaviour
{
    public GameObject rightHand, leftHand;
    public int totalNumOfWorkers = 6;
    public bool orthographic = true;

    private GameObject workerRightArm, workerLeftArm;
    private bool isFalling, touchedChair, sendWorkerBack;
    private Vector3 originalPos;
    private float t = 0.0f;
    private float minX, maxX, minY, maxY;
    
    // Find worker's right and left arm, initialize some triggers, and find
    // the worker's original position (in case they need to be sent back). 
    void Start()
    {
        // Find right & left arm objects
        workerRightArm = this.transform.Find("RightArm").gameObject;
        workerLeftArm = this.transform.Find("LeftArm").gameObject;

        // Initialize triggers
        touchedChair = false;
        sendWorkerBack = false;

        // Make sure feedback pic is hidden
        this.transform.Find("FeedbackPic").gameObject.SetActive(false);

        // Find worker's original position in case they are sent back
        originalPos = this.transform.localPosition;
        
    }

    // Moves worker around
    void Update()
    {
        // If both of the workers arms are grabbed, move the worker.
        if (ArmGrabbed(workerRightArm) && ArmGrabbed(workerLeftArm))
        {
            //using the position of the right and left hands to move worker
            Vector3 pos1 = leftHand.transform.position;
            Vector3 pos2 = rightHand.transform.position;

            // Find the midpoint of the user's left & right hands
            Vector3 midPoint = (pos2 - pos1) / 2;

            // Make the worker's postion the midpoint
            this.transform.position = midPoint + pos1;

            // Block all other workers' movement. This is to avoid workers being
            // "vaccuumed up" by the user. 
            AllWorkersCanMove(false);
            
            // Let the star selection know that a worker is grabbed so it doesn't undo the block
            GameObject star = GameObject.Find("/WorkerCanvas/YellowStar3");

            // Let the star know that a worker was grabbed. This is a trigger variable in
            // BlockAllWorkersV2
            star.SendMessage("WorkerIsGrabbed", true); 

            // Block star's movement while worker is being moved. This is found in MoveStarV2.
            star.SendMessage("BlockStar", true); // block star

            if (UserLetGo())
            {
                // Disable the up image and enable the down image in both arms
                LetArmGo(workerRightArm);
                LetArmGo(workerLeftArm);

                // Unblock all other workers' movement
                AllWorkersCanMove(true);

                // Let star know that a worker was let go
                star.SendMessage("WorkerIsGrabbed", false);

                // Unblock star
                star.SendMessage("BlockStar", false);

                // If the user hasn't touched the table, send user back using Mathf.Lerp. 
                // Here I'm getting all the needed variables for lerp.
                // Documentation on lerp: https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html
                if (!touchedChair)
                {
                    minX = this.transform.localPosition.x;
                    minY = this.transform.localPosition.y;

                    maxX = originalPos.x;
                    maxY = originalPos.y;

                    sendWorkerBack = true;
                    
                }

                // If the user did touch the table, send worker over to FillChairsV2
                else
                {
                    sendWorkerBack = false; // making sure worker won't go back to original position.
                }
            }// end if user let go

        }// end if both arms grabbed
        
        // This is where the lerp function happens. This smoothly moves the worker back to their 
        // original position.
        if (sendWorkerBack)
        {
            this.transform.localPosition = new Vector3(Mathf.Lerp(minX, maxX, t), Mathf.Lerp(minY, maxY, t), originalPos.z);

            t += 0.5f * Time.deltaTime;

            // Once we hit the target distance, reset needed variables.
            if (t > 1.0f)
            {
                t = 0.0f;

                sendWorkerBack = false;

                BlockAllOtherChairs(false);
            }
        }// end if sendWorkerBack

        // If the right arm is grabbed and the left isn't, ungrab the right arm after a certain
        // amount of time.
        if (ArmGrabbed(workerRightArm) && !ArmGrabbed(workerLeftArm))
        {
            StartCoroutine(LetArmGoAfterTime(workerRightArm, workerLeftArm));
        }

        // If the left arm is grabbed and the right isn't, ungrab the left arm after a certain 
        // amount of time.
        if (ArmGrabbed(workerLeftArm) && !ArmGrabbed(workerRightArm))
        {
            StartCoroutine(LetArmGoAfterTime(workerLeftArm, workerRightArm));
        }

    }// end Update

    // Turns on touchedChair trigger if worker his a chair's collision box.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Chair")
        {
            // Debug.Log("Touched table!!");
            touchedChair = true;
        }

    }// end OnTriggerEnter

    // Turns off touchedChair trigger if worker leaves chair's collision box.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Chair")
        {
            touchedChair = false;
        }

    }// end OnTriggerExit

    // If the "up" image is enabled, return true. Otherwise, return false.
    private bool ArmGrabbed(GameObject arm)
    {
        GameObject upArm = arm.transform.Find("Up").gameObject;

        if (upArm.activeSelf)
        {
            return true;
        }

        return false;

    }// end ArmGrabbed

    // Disables the up arm image and enables the down arm image. 
    private void LetArmGo(GameObject arm)
    {
        GameObject upArm, downArm;
        upArm = arm.transform.Find("Up").gameObject;
        downArm = arm.transform.Find("Down").gameObject;

        upArm.SetActive(false);
        downArm.SetActive(true);
    }// end LetArmGo

    // Takes in 2 arm and checks if one's up arm image is enabled after 2 seconds. If so, 
    // Force the arm down. 
    IEnumerator LetArmGoAfterTime(GameObject arm1, GameObject arm2)
    {
        // Wait 2 seconds before doing anything.
        yield return new WaitForSeconds(2);

        GameObject upArm1, downArm1, upArm2;

        // Find the up & down arms of arm 1
        upArm1 = arm1.transform.Find("Up").gameObject;
        downArm1 = arm1.transform.Find("Down").gameObject;

        // Find the up arm of arm 2
        upArm2 = arm2.transform.Find("Up").gameObject;

        // If arm1 is up and arm 2 isn't, force arm1 down.
        if (upArm1.activeSelf == true && upArm2.activeSelf == false)
        {
            upArm1.SetActive(false);
            downArm1.SetActive(true);
        }
    }// end LetArmGoAfterTime

    // Sends a message to each arm of each worker of whether it can be moved or not. This is 
    // called when a worker is being moved.
    public void AllWorkersCanMove(bool status)
    {
        string thisName = this.name;

        // Go through each worker
        for (int i = 1; i <= totalNumOfWorkers; i++)
        {
            // worker's name = "Worker#"
            string workerName = "Worker" + i.ToString();

            // I'm ignoring the worker being moved.
            if (workerName != thisName)
            {
                // Find the worker game object
                string fullPath = "/WorkerCanvas/WorkerScreen/" + workerName;
                GameObject worker = GameObject.Find(fullPath);

                // Find the worker's right and left arm objects
                GameObject rightArm = worker.transform.Find("RightArm").gameObject;
                GameObject leftArm = worker.transform.Find("LeftArm").gameObject;

                // Block/unblock the arm
                rightArm.SendMessage("SetOkToLift", status);
                leftArm.SendMessage("SetOkToLift", status);

            }

            else
            {
                // Pass over the worker being moved
                continue;
            }

        }// end for loop

    }// end AllWorkersCanMove

    // This is called from other classes. Triggers the lerp call in the Update function.
    public void SendBack(bool status)
    {
        minX = this.transform.localPosition.x;
        minY = this.transform.localPosition.y;

        maxX = originalPos.x;
        maxY = originalPos.y;

        sendWorkerBack = status;
    }// end SendBack

    // Reused function. If the user's hands are a certain distance apart (3.5 unity units for othogonal
    // games), return true. Otherwise, return false.
    bool UserLetGo()
    {
        if (rightHand.transform.position.x - leftHand.transform.position.x > 350)
        {
            // Debug.Log("DragWithHandlebars -> User let go with distance at " + 
            //     (rightHand.transform.position.x - leftHand.transform.position.x));
            return true;
        }
        else if ((rightHand.transform.position.y - leftHand.transform.position.y > 350) ||
                (leftHand.transform.position.y - rightHand.transform.position.y > 350))
        {
            //Debug.Log("DragWithHandlebars -> User let go with distance at " +
            //    (rightHand.transform.position.y - leftHand.transform.position.y));
            return true;
        }

        if (orthographic)
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
        }

        return false;
    }// end UserLetGo

    // Blocks other chairs from grabbing a worker.
    void BlockAllOtherChairs(bool status)
    {
        // Go through all 3 chairs
        for (int i = 1; i <= 3; i++)
        {
            string otherChairStr = "Chair" + i.ToString();

            GameObject otherChair = GameObject.Find("/WorkerCanvas/WorkerScreen/" + otherChairStr);

            if (otherChair != this.gameObject)
            {
                otherChair.SendMessage("BlockThisChair", status);
            }

        }// end for loop

    }// end BlockAllOtherchairs

}// end LiftWorkerAndDragV2

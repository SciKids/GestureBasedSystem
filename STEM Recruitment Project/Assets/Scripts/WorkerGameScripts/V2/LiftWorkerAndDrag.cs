using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftWorkerAndDrag : MonoBehaviour
{
    public GameObject rightHand, leftHand;
    public int totalNumOfWorkers = 6;

    private GameObject workerRightArm, workerLeftArm;
    private bool isFalling, touchedChair, sendWorkerBack;
    private Vector3 originalPos;
    private float t = 0.0f;
    private float minX, maxX, minY, maxY;
    //private GameObject tableTop;
    // private bool otherWorkerIsGrabbed = false;
    // Start is called before the first frame update
    void Start()
    {
        workerRightArm = this.transform.Find("RightArm").gameObject;
        workerLeftArm = this.transform.Find("LeftArm").gameObject;
        isFalling = false;
        touchedChair = false;
        sendWorkerBack = false;

        this.transform.Find("FeedbackPic").gameObject.SetActive(false);
        originalPos = this.transform.position;

        //tableTop = GameObject.Find("/WholeGame/WorkerScreen/Table/Top");
    }

    // Update is called once per frame
    void Update()
    {

        if(isFalling)
        {
            this.transform.Translate(Vector3.down * 100.0f * Time.deltaTime, Space.World);
        }

        if(ArmGrabbed(workerRightArm) && ArmGrabbed(workerLeftArm))
        {
            //using the position of the right and left hands to move the whole object
            Vector3 pos1 = leftHand.transform.position;
            Vector3 pos2 = rightHand.transform.position;

            Vector3 midPoint = (pos2 - pos1) / 2;

            this.transform.position = midPoint + pos1;

            AllWorkersCanMove(false);

            // Let the star selection know that a worker is grabbed so it doesn't undo the block
            GameObject star = GameObject.Find("/WholeGame/WorkerScreen/Canvas/YellowStar3");

            star.SendMessage("WorkerIsGrabbed", true); // block all other workers
            star.SendMessage("BlockStar", true); // block star
            if (UserLetGo())
            {
                LetArmGo(workerRightArm);
                LetArmGo(workerLeftArm);

                AllWorkersCanMove(true);

                // Let star know that a worker was let go
                star.SendMessage("WorkerIsGrabbed", false);

                // Unblock star
                star.SendMessage("BlockStar", false);
                // If the user hasn't touched the table, send user back using Mathf.Lerp. 
                // Here I'm getting all the needed variables for lerp.
                if (!touchedChair)
                {
                    minX = this.transform.position.x;
                    minY = this.transform.position.y;

                    maxX = originalPos.x;
                    maxY = originalPos.y;

                    sendWorkerBack = true;

                    //tableTop.SendMessage("RemoveWorkerFromChair", this.gameObject);
                }

                // If the user did touch the table, send worker over to FillChairsV2
                else
                {
                   // tableTop.SendMessage("AssignWorkerToChair", this.gameObject);

                    sendWorkerBack = false; // making sure worker won't go back to original position.
                }
            }
        }
        
        if(sendWorkerBack)
        {
            this.transform.position = new Vector3(Mathf.Lerp(minX, maxX, t), Mathf.Lerp(minY, maxY, t), originalPos.z);

            t += 0.5f * Time.deltaTime;

            if(t > 1.0f)
            {
                t = 0.0f;

                sendWorkerBack = false;

                BlockAllOtherChairs(false);
            }
        }

        if(ArmGrabbed(workerRightArm) && !ArmGrabbed(workerLeftArm))
        {
            StartCoroutine(LetArmGoAfterTime(workerRightArm, workerLeftArm));
        }
        
        if(ArmGrabbed(workerLeftArm) && !ArmGrabbed(workerRightArm))
        {
            StartCoroutine(LetArmGoAfterTime(workerLeftArm, workerRightArm));
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Chair")
        {
           // Debug.Log("Touched table!!");
            touchedChair = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Chair")
        {
            touchedChair = false;
        }
    }
    
    private bool ArmGrabbed(GameObject arm)
    {
        GameObject upArm = arm.transform.Find("Up").gameObject;
        
        if(upArm.activeSelf)
        {
            return true;
        }

        return false;
        
    }

    private void LetArmGo(GameObject arm)
    {
        GameObject upArm, downArm;
        upArm = arm.transform.Find("Up").gameObject;
        downArm = arm.transform.Find("Down").gameObject;

        upArm.SetActive(false);
        downArm.SetActive(true);
    }

    IEnumerator LetArmGoAfterTime(GameObject arm1, GameObject arm2)
    {
        yield return new WaitForSeconds(2);

        GameObject upArm1, downArm1, upArm2;

        upArm1 = arm1.transform.Find("Up").gameObject;
        downArm1 = arm1.transform.Find("Down").gameObject;

        upArm2 = arm2.transform.Find("Up").gameObject;
        
        if(upArm1.activeSelf == true && upArm2.activeSelf == false)
        {
            upArm1.SetActive(false);
            downArm1.SetActive(true);
        }
    }
    
    // Send a message to each arm of each worker of whether it can be moved or not.
    public void AllWorkersCanMove(bool status)
    {
        string thisName = this.name;
        
        for(int i = 1; i <= totalNumOfWorkers; i++)
        {
            //Find each worker other than this one
            string workerName = "Worker" + i.ToString();
            
            if(workerName != thisName)
            {
                string fullPath = "/WholeGame/WorkerScreen/" + workerName;

                GameObject worker = GameObject.Find(fullPath);

                // Block both arms from lifting
                GameObject rightArm = worker.transform.Find("RightArm").gameObject;

                GameObject leftArm = worker.transform.Find("LeftArm").gameObject;

                rightArm.SendMessage("SetOkToLift", status);

                leftArm.SendMessage("SetOkToLift", status);

            }

            else
            {
                continue;   
            }
        }
    }

    public void SendBack(bool status)
    {
        minX = this.transform.position.x;
        minY = this.transform.position.y;

        maxX = originalPos.x;
        maxY = originalPos.y;

        sendWorkerBack = status;
    }
    
    
    bool UserLetGo()
    {
        if (rightHand.transform.position.x - leftHand.transform.position.x > 350)
        {
            return true;
        }

        else if ((rightHand.transform.position.y - leftHand.transform.position.y > 350) ||
                (leftHand.transform.position.y - rightHand.transform.position.y > 350))
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

            GameObject otherChair = GameObject.Find("/WholeGame/WorkerScreen/" + otherChairStr);

            if (otherChair != this.gameObject)
            {
                otherChair.SendMessage("BlockThisChair", status);
            }
        }
    }
}

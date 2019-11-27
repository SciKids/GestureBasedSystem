using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillChairsV2 : MonoBehaviour
{
    /// <summary>
    /// Designed to be placed on the top of the table. Each chair needs 
    /// to be labeled as "Chair#", starting from 1. 
    /// </summary>
     
    public int numberOfChairs;
    public GameObject workerScreenScripts;
    public Text feedback;
    public Button nextButton;

    private Chair[] chairList;
    private GameObject workerToPlace = null;

    // Start is called before the first frame update
    void Start()
    {
        chairList = new Chair[numberOfChairs];

        // Find all chairs in the WorkerScreen game object.
        for(int i = 1; i <= numberOfChairs; i++)
        {
            string chairStr = "/WholeGame/WorkerScreen/Chair" + i.ToString();

            GameObject newChairObj = GameObject.Find(chairStr);

            //GameObject newCube = newChairObj.transform.Find("Cube").gameObject;

            // Add chair and cube to the chair list. Worker's default is null.
            Chair newChair = new Chair(newChairObj, null, false);

            chairList[i - 1] = newChair;   
        }
    }

    private void Update()
    {
        if(GetNextAvailableChair() == -1)
        {
            nextButton.gameObject.SetActive(true);
        }
        else
        {
            nextButton.gameObject.SetActive(false);
        }
    }

    // Receives message from LiftWorkerAndDrag
    public void AssignWorkerToChair(GameObject worker)
    {
        int availableChair = GetNextAvailableChair();

        if(availableChair == -1)
        {
            //Debug.Log("All chairs are filled!");

            feedback.text = "All chairs are filled! Either drag a worker out or reset the entire table.";

            workerScreenScripts.SendMessage("ResetWorker", worker);
        }
        else
        {
            GameObject cube = chairList[availableChair].chair.gameObject.transform.Find("Cube").gameObject;

            worker.transform.position = cube.transform.position;

            chairList[availableChair].worker = worker;

            chairList[availableChair].isFilled = true;
        }
    }

    // Receives message from LiftWorkerAndDrag
    public void RemoveWorkerFromChair(GameObject workerToRemove)
    {
        for(int i = 0; i < numberOfChairs; i++)
        {
            if(chairList[i].worker == workerToRemove)
            {
                chairList[i].worker = null;
                chairList[i].isFilled = false;

                break;
            }
        }
    }

    private int GetNextAvailableChair()
    {
        for (int i = 0; i < numberOfChairs; i++)
        {
            //Debug.Log("Chair " + i + " status: " + chairs[i].isFilled);
            if (!chairList[i].isFilled)
            {
                //Debug.Log("Returning chair " + i);
                return i;
            }
        }

        return -1;
    }
    
    // Sends entire chair list to CheckWorkersV2.
    public void SendChairsToCheck()
    {
        GameObject scripts = GameObject.Find("/WholeGame/WorkerScreen");

        for(int i = 0; i < numberOfChairs; i++)
        {
            scripts.SendMessage("ReceiveChair", chairList[i].chair);
            scripts.SendMessage("ReceiveWorker", chairList[i].worker);
            scripts.SendMessage("ReceiveCube", chairList[i].chair.transform.Find("Cube").gameObject);
            scripts.SendMessage("IncreaseIndex", 1);
        }

        scripts.SendMessage("OkToCheck", true);
    }
    
    public class Chair
    {
        public GameObject chair, worker;
        public bool isFilled;

        public Chair(GameObject newChair, GameObject newWorker, bool filled)
        {
            chair = newChair;
            worker = newWorker;
            isFilled = filled;
        }
        
    }
}

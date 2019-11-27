using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetV2 : MonoBehaviour
{
    /// <summary>
    /// This script is meant to be placed on the "WorkerScreen" game object.
    /// </summary>

    public int numOfWorkers;

    private Worker[] allWorkers;

    // Start is called before the first frame update
    void Start()
    {
        allWorkers = new Worker[numOfWorkers];

        for(int i = 1; i <= numOfWorkers; i++)
        {
            string workerStr = "Worker" + i.ToString();

            GameObject workerObj = GameObject.Find(workerStr);

            Worker newWorker = new Worker(workerObj, workerObj.transform.position);

            allWorkers[i - 1] = newWorker;

           // Debug.Log("Added " + allWorkers[i - 1].worker.name + " to position " + i);
        }
        
    }

    public void RestartAllWorkers()
    {
        for(int i = 0; i < numOfWorkers; i++)
        {
            // Reset the worker's position.
            allWorkers[i].worker.transform.position = allWorkers[i].originalPos;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetWorker(GameObject workerToReset)
    {
        for(int i = 0; i < numOfWorkers; i++)
        {
            if(allWorkers[i].worker == workerToReset)
            {
                workerToReset.transform.position = allWorkers[i].originalPos;
                break;
            }     
        }
    }

    // Worker node
    private class Worker
    {
        public GameObject worker;
        public Vector3 originalPos;
        
        public Worker(GameObject newWorker, Vector3 pos)
        {
            worker = newWorker;
            originalPos = pos;


        }
        
    }
}

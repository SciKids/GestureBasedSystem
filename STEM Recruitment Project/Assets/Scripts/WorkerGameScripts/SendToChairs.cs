using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SendToChairs : MonoBehaviour
{
    /// SendToChairs sends workers to next available chair if the worker is 
    /// dropped on the table.
    public GameObject[] Workers, ChairObjects;
    public Text feedback;
    public GameObject nextButton;
    private Chair[] chairs;
    private Worker[] allWorkers;

    private void Start()
    {
        allWorkers = new Worker[Workers.Length];
        chairs = new Chair[ChairObjects.Length];

        // Fill up chair list
        for (int i = 0; i < ChairObjects.Length; i++)
        {
            Chair chair = new Chair(ChairObjects[i], false);
            chairs[i] = chair;
        }

        int j = 0;

        // Fill all workers list
        for (int i = 0; i < Workers.Length; i++)
        {
            Worker newWorker = new Worker(Workers[i], Workers[i].transform.position);
            allWorkers[i] = newWorker;
            Debug.Log(i + ": Added worker " + allWorkers[i].worker.name + " with position " + allWorkers[i].initialPos);
            j++;
        }

        nextButton.SetActive(false);

    }
    private void Update()
    {
        // If all of the chairs are filled, activate the next button so that the user can
        // evaluate their results.
        if (GetNextAvailableChair() == -1)
        {
            nextButton.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Worker")
        {
            int chairNum = GetNextAvailableChair();

            // If there is an available chair, move worker into it.
            if (chairNum != -1)
            {
                // The cube is a marker for the exact place I want the worker to be.
                GameObject chairCube = chairs[chairNum].chair.transform.Find("Cube").gameObject;
                collision.transform.position = chairCube.transform.position;
                chairs[chairNum].isFilled = true;
            }

            // If there is no available chair, return worker to their initial position.
            else
            {
                Vector3 returnPos = GetWorkerInitPosition(collision);

                if (returnPos == Vector3.zero)
                {
                    Debug.Log("(CheckWorkers) Something went wrong - 0 vector was returned");
                }

                collision.transform.position = returnPos;
                feedback.text = "All seats are already filled!";
            }
        }
    }

    private int getItemIndex(GameObject item, GameObject[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] == item)
            {
                return i;
            }
        }
        return -1;
    }

    private int GetNextAvailableChair()
    {
        for (int i = 0; i < chairs.Length; i++)
        {
            //Debug.Log("Chair " + i + " status: " + chairs[i].isFilled);
            if (chairs[i].isFilled == false)
            {
                //Debug.Log("Returning chair " + i);
                return i;
            }
        }

        return -1;
    }

    private Vector3 GetWorkerInitPosition(Collision collision)
    {
        // Find the worker in question and return their initial position.
        for (int i = 0; i < allWorkers.Length; i++)
        {
            if (collision.gameObject.name == allWorkers[i].worker.name)
            {
                return allWorkers[i].initialPos;
            }
        }

        // If something goes wrong, return 0,0,0.
        return Vector3.zero;
    }

    // Chair node
    public class Chair
    {
        public GameObject chair;
        public bool isFilled;

        public Chair(GameObject chairObject, bool filled)
        {
            chair = chairObject;
            filled = isFilled;
        }
    }

    // Worker node
    public class Worker
    {
        public GameObject worker;
        public Vector3 initialPos;

        public Worker(GameObject workerObject, Vector3 pos)
        {
            worker = workerObject;
            initialPos = pos;
        }
    }
}

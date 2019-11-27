using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CheckWorkers : MonoBehaviour
{
    public GameObject[] chairObjects, allWorkers;
    public Vector3 loadingScreenPos;
    public Camera camera;
    public GameObject nextButton;
    private Chair[] chairs;
    private bool okTocheck, chairListFilled;

    private void Start()
    {
        chairs = new Chair[chairObjects.Length];

        okTocheck = false;

        chairListFilled = false;

    }

    private void Update()
    {
        if(okTocheck)
        {
            if (!chairListFilled)
            {
                FillChairList();
                chairListFilled = true;
            }
            
            // Go through each worker in the chairs and show their feedback pic.
            for (int i = 0; i < chairs.Length; i++)
            {
                //GameObject canvas = chairs[i].worker.transform.Find("Canvas").gameObject;
                GameObject feedbackPic = chairs[i].worker.transform.Find("FeedbackPic").gameObject;

                feedbackPic.SetActive(true);

                Debug.Log("Feedbackpic active.");

            }
            nextButton.SetActive(false);
            okTocheck = false;
        }

        if(camera.transform.position == loadingScreenPos)
        {
            Debug.Log("Ok to check");
            okTocheck = true;
        }
    }

    private void FillChairList()
    {
        for(int i = 0; i < chairObjects.Length; i++)
        {
            GameObject worker = GetWorkerInChair(chairObjects[i]);

            Chair chair = new Chair(chairObjects[i], worker);

            chairs[i] = chair;
        }
    }

    private GameObject GetWorkerInChair(GameObject chair)
    {
        GameObject chairCube = chair.transform.Find("Cube").gameObject;

        for (int i = 0; i < allWorkers.Length; i++)
        {
            if (allWorkers[i].transform.position == chairCube.transform.position)
            {
                return allWorkers[i];
            }
        }

        return null;
    }
    // Chair node
    public class Chair
    {
        public GameObject chair;
        public bool isFilled;
        public GameObject worker;

        public Chair(GameObject chairObject, GameObject workerObject)
        {
            chair = chairObject;
            worker = workerObject;
        }
    }
}

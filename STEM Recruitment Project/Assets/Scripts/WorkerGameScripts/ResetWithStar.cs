using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetWithStar : MonoBehaviour
{
    public GameObject clearStar, rightHand, leftHand;
    public GameObject[] workers;

    private GameObject rightHalf, leftHalf;
    private bool touchedReset;
    private Vector3 starInitPos;
    private Vector3[] workerInitialPositions;

    // Start is called before the first frame update
    void Start()
    {
        touchedReset = false;
        starInitPos = this.transform.position;
        
        rightHalf = this.transform.Find("RightHalf").gameObject;
        leftHalf = this.transform.Find("LeftHalf").gameObject;

        FillWorkerVectorList();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isGrabbed(rightHalf) && isGrabbed(leftHalf))
        {
            //using the position of the right and left hands to move the whole object
            Vector3 pos1 = leftHand.transform.position;
            Vector3 pos2 = rightHand.transform.position;

            Vector3 midPoint = (pos2 - pos1) / 2;

            this.transform.position = midPoint + pos1;
            // If the user let go and the picture is not attached to an answer,
            // send picture back to original spot.
            if (userLetGo())
            {
                // Turn bars to black to prevent further movement
                TurnAllToBlack();

                if(touchedReset)
                {
                    this.transform.position = starInitPos;
                    touchedReset = false;

                }
            }
        }

        if (isGrabbed(rightHalf) && !isGrabbed(leftHalf) && userLetGo())
        {
            TurnOneToBlack(rightHalf);
        }
        if (isGrabbed(leftHalf) && !isGrabbed(rightHalf) && userLetGo())
        {
            TurnOneToBlack(leftHalf);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.gameObject.name == "ClearStar")
        {
            touchedReset = true;
            this.transform.position = starInitPos;
        }
    }

    void FillWorkerVectorList()
    {
        for(int i = 0; i < workers.Length; i++)
        {
            /*workerInitialPositions[i] = workers[i].transform.position;
            Debug.Log("Added worker " + workers[i] + " with position " + workers[i].transform.position);
        */}
    }

    void ResetAll()
    {
        for(int i = 0; i < workers.Length; i++)
        {
            workers[i].transform.position = workerInitialPositions[i];
        }
    }

    bool userLetGo()
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

        return false;
    }

    bool isGrabbed(GameObject starHalf)
    {
        GameObject segment;
        Material segColor;

        segment = starHalf.transform.Find("Capsule1").gameObject;

        segColor = segment.GetComponent<Renderer>().material;
        
        if (segColor.color == Color.green)
        {
            return true;
        }

        return false;
    }

    void TurnAllToBlack()
    {
        // Turn right half to black
        for (int i = 1; i < 6; i++)
        {
            GameObject segment;
            Material segColor;
            string segName = "Capsule" + i.ToString();

            segment = rightHalf.transform.Find(segName).gameObject;
            segColor = segment.GetComponent<Renderer>().material;

            segColor.color = Color.black;
        }

        // Turn left half to black
        for (int i = 1; i < 6; i++)
        {
            GameObject segment;
            Material segColor;
            string segName = "Capsule" + i.ToString();

            segment = leftHalf.transform.Find(segName).gameObject;
            segColor = segment.GetComponent<Renderer>().material;

            segColor.color = Color.black;
        }
    }

    void TurnOneToBlack(GameObject starHalf)
    {
        for (int i = 1; i < 6; i++)
        {
            GameObject segment;
            Material segColor;
            string segName = "Capsule" + i.ToString();

            segment = starHalf.transform.Find(segName).gameObject;
            segColor = segment.GetComponent<Renderer>().material;

            segColor.color = Color.black;
        }
    }

}


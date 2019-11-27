using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragWorkers : MonoBehaviour
{
    public GameObject leftBar, rightBar, leftHand, rightHand, workerSphere;
    private Vector3 originalPos;
    private Canvas textCanvas;
    private bool isFalling;
    // public GameObject[] answers;
    public float moveSpeed;
    void Start()
    {
        // Get original position
        originalPos = this.transform.position;

        textCanvas = GetComponentInChildren<Canvas>();

        textCanvas.enabled = false;

        isFalling = false;
    }

    void Update()
    {
        // Update current position
        //Vector3 currentPos = this.transform.position; 
        if (isFalling)
        {
            // sphere.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;

            this.transform.Translate(Vector3.down * 100.0f * Time.deltaTime, Space.World);

            //Debug.Log("Worker Pos: " + sphere.transform.position);
        }
        if (isGrabbed(rightBar) && isGrabbed(leftBar))
        {
            //using the position of the right and left hands to move the whole object
            Vector3 pos1 = leftHand.transform.position;
            Vector3 pos2 = rightHand.transform.position;

            Vector3 midPoint = (pos2 - pos1) / 2;

            this.transform.position = midPoint + pos1;
            //text.enabled = true;
            // If the user let go and the picture is not attached to an answer,
            // send picture back to original spot.
            if (userLetGo())
            {
                // Turn bars to black to prevent further movement
                turnAllToBlack(rightBar, leftBar);
                isFalling = true;
                // text.enabled = false;
            }
        }
        if (isGrabbed(rightBar) && !isGrabbed(leftBar) && userLetGo())
        {
            turnOneToBlack(rightBar);
        }
        if (isGrabbed(leftBar) && !isGrabbed(rightBar) && userLetGo())
        {
            turnOneToBlack(leftBar);
        }


    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Table")
        {
            isFalling = false;
            //sphere.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
           // Debug.Log("Reached the bottom");
        }

        if (collision.gameObject.tag == "Hand")
        {
            textCanvas.enabled = true;
        }

        if (collision.gameObject.tag == "Floor")
        {
            isFalling = false;
            this.transform.position = originalPos;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        textCanvas.enabled = false;
    }

    // Method determines if a bar is being grabbed if at least one segment of the bar is green.
    // Class GrabHandles changes segments from black to green.
    bool isGrabbed(GameObject bar)
    {
        GameObject seg1, seg2;
        Material seg1Mat, seg2Mat;

        seg1 = bar.transform.Find("Seg1").gameObject;
        seg1Mat = seg1.GetComponent<Renderer>().material;

        seg2 = bar.transform.Find("Seg2").gameObject;
        seg2Mat = seg2.GetComponent<Renderer>().material;

        if (seg1Mat.color == Color.green && seg2Mat.color == Color.green)
        {
            return true;
        }

        return false;
    }

    // Resets all segments to black to prevent player from moving object.
    void turnAllToBlack(GameObject bar1, GameObject bar2)
    {
        GameObject b1seg1, b1seg2, b2seg1, b2seg2, b1knob1, b1knob2, b2knob1, b2knob2;
        Material b1seg1Mat, b1seg2Mat, b2seg1Mat, b2seg2Mat, b1knob1Mat, b1knob2Mat, b2knob1Mat, b2knob2Mat;

        b1seg1 = bar1.transform.Find("Seg1").gameObject;
        b1seg2 = bar1.transform.Find("Seg2").gameObject;
        b2seg1 = bar2.transform.Find("Seg1").gameObject;
        b2seg2 = bar2.transform.Find("Seg2").gameObject;


        b1seg1Mat = b1seg1.GetComponent<Renderer>().material;
        b1seg2Mat = b1seg2.GetComponent<Renderer>().material;
        b2seg1Mat = b2seg1.GetComponent<Renderer>().material;
        b2seg2Mat = b2seg2.GetComponent<Renderer>().material;

        b1seg1Mat.color = Color.black;
        b1seg2Mat.color = Color.black;
        b2seg1Mat.color = Color.black;
        b2seg2Mat.color = Color.black;

        // Get knob objects, materials, and turn to black.
        b1knob1 = b1seg2.transform.Find("Knob1").gameObject;
        b1knob2 = b1seg2.transform.Find("Knob2").gameObject;
        b2knob1 = b2seg2.transform.Find("Knob1").gameObject;
        b2knob2 = b2seg2.transform.Find("Knob2").gameObject;

        b1knob1Mat = b1knob1.GetComponent<Renderer>().material;
        b1knob2Mat = b1knob2.GetComponent<Renderer>().material;
        b2knob1Mat = b2knob1.GetComponent<Renderer>().material;
        b2knob2Mat = b2knob2.GetComponent<Renderer>().material;

        b1knob1Mat.color = Color.black;
        b1knob2Mat.color = Color.black;
        b2knob1Mat.color = Color.black;
        b2knob2Mat.color = Color.black;

    }

    // Set one bar to black
    void turnOneToBlack(GameObject bar)
    {
        GameObject seg1, seg2, knob1, knob2;
        Material seg1Mat, seg2Mat, knob1Mat, knob2Mat;

        seg1 = bar.transform.Find("Seg1").gameObject;
        seg2 = bar.transform.Find("Seg2").gameObject;

        seg1Mat = seg1.GetComponent<Renderer>().material;
        seg2Mat = seg2.GetComponent<Renderer>().material;

        seg1Mat.color = Color.black;
        seg2Mat.color = Color.black;

        // Get knob objects, materials, and turn to black.
        knob1 = seg2.transform.Find("Knob1").gameObject;
        knob2 = seg2.transform.Find("Knob2").gameObject;
        knob1Mat = knob1.GetComponent<Renderer>().material;
        knob2Mat = knob2.GetComponent<Renderer>().material;
        knob1Mat.color = Color.black;
        knob2Mat.color = Color.black;

    }

    // Check if one bar is green
    bool isGreen(GameObject bar)
    {
        GameObject seg;
        Material segMat;

        seg = bar.transform.Find("Seg1").gameObject;

        segMat = seg.GetComponent<Renderer>().material;

        if (segMat.color == Color.green)
        {
            return true;
        }
        return false;

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
}

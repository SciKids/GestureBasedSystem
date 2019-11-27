using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragWithHandlebars : MonoBehaviour
{
    public GameObject leftHand, rightHand, leftBar, rightBar, outlineFront, outlineBack;
    //public Text scoreText;
    //private int score;
    private Vector3 originalPosition, outlinePos;
    private GameObject cube;
    private Material outlineMat;
    private RigidbodyConstraints constraints;
    private bool isCorrect;
    
    void Start()
    {
        originalPosition = this.transform.position;
        cube = this.transform.Find("Cube").gameObject;
        outlinePos = outlineFront.transform.position;
        outlineMat = outlineBack.GetComponent<Renderer>().material;

        isCorrect = false;
        
        //score = int.Parse(scoreText.text);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = this.transform.position;

        if(isCorrect)
        {
            // Move the picture to its outline
            this.transform.position = outlinePos;

            // Freeze the picture
            constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            
            // Turn bars to black to prevent user from moving the picture from the outline.
            turnToBlack(rightBar);
            turnToBlack(leftBar);

            outlineMat.color = Color.green;

        }

        // If the picture is not being grabbed and it's not in its correct position, reset
        // the picture to its original position.
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, originalPosition, 1);
           // this.transform.position = originalPosition;
        }

        // If we're in the range of the picture's outline
        if (currentPos.x <= outlinePos.x + 50 || currentPos.x <= outlinePos.x - 50)
        {
            if (currentPos.y < outlinePos.y + 50 || currentPos.y <= outlinePos.y - 50)
            {
                isCorrect = true;

                //Debug.Log(this.name + " CORRECT");
                
            }

        }

        if (isGrabbed(rightBar) && isGrabbed(leftBar))
        {
            //using the position of the right and left hands to move the whole object
            Vector3 pos1 = leftHand.transform.position;
            Vector3 pos2 = rightHand.transform.position;
            
            Vector3 midPoint = (pos2 - pos1)/2;

            this.transform.position = midPoint + pos1;
        }

    }

    public void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Outline")
        {
            outlineMat.color = Color.green;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        outlineMat.color = Color.black;

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
        
        if (seg1Mat.color == Color.green || seg2Mat.color == Color.green)
        {
            return true;
        }

        return false;
    }

    // Resets all segments to black to prevent player from moving object.
    void turnToBlack(GameObject bar)
    {
        GameObject seg1, seg2;
        Material seg1Mat, seg2Mat;

        seg1 = bar.transform.Find("Seg1").gameObject;
        seg1Mat = seg1.GetComponent<Renderer>().material;

        seg2 = bar.transform.Find("Seg2").gameObject;
        seg2Mat = seg2.GetComponent<Renderer>().material;
        
        seg2Mat.color = Color.black;
           
        seg1Mat.color = Color.black;
         
    }

   /* void MoveObjectBack(Vector3 start, Vector3 end)
    {

    }*/
    
}

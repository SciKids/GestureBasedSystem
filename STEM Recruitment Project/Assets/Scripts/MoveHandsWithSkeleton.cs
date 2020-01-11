using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script Summary ////////////////////////////////////////////////////////////
/*
 * Uses Nuitrack's skeleton tracker module to track the user's right and left hands.
 */
public class MoveHandsWithSkeleton : MonoBehaviour
{
    ///// public variables - for skeleton tracking & scaling /////
    public nuitrack.JointType[] typeJoint;
    GameObject[] CreatedJoint;
    public GameObject rightHand, leftHand;
    public Camera camera;
   // public Canvas canvas;
    public float ZPosition;
    public bool orthographic;
    public bool hoverToClick;
    ///// Private variables for button clicking.//////
    // Locks for right and left hand z positions - determines whether or not to start measuring distances between z positions.
    private bool rightZLock, leftZLock;
    // Button that is found and assgned in PressButtonWithHands script
    private Button button;
    // Z positions to measure initial and final z positions to determine button click.
    private float rightZPos1, rightZPos2, leftZPos1, leftZPos2;
    // right and left hand positions are needed throughout methods
    private Vector3 rightPosGlobal, leftPosGlobal;
    // initPosLock needed to ensure that inital z position is recorded once
    private bool buttonClicked, initPosLock;

    private int testCounter = 0;

    private string originalBtnText;

    void Start()
    {
        // NOTE: right and left hands are mixed up. Don't know if it's on my end
        // or nuitrack's. For now, a fix is to mirror the image and set the right hand
        // image to the left hand value.
        NuitrackManager.DepthSensor.SetMirror(true);

        // Lock z positions of right and left hands.
        rightZLock = true;
        leftZLock = true;

        // Values and locks for button clicking.
        button = null;
        buttonClicked = false;
        initPosLock = false;
    }// end Start

    private void OnDestroy()
    {
        // Be sure to unmirror image before switching scripts.
        NuitrackManager.DepthSensor.SetMirror(false);

    }// end OnDestroy

    // Update is called once per frame
    void Update()
    {
        float initMousePos = Input.GetAxis("Mouse X");

        if (CurrentUserTracker.CurrentUser != 0) // If there is a user in frame...
        {
            // Get the user's skeleton from nuitrack's scripts.
            nuitrack.Skeleton skeleton = CurrentUserTracker.CurrentSkeleton;

            // Get the right and left joint positions.
            nuitrack.Joint rightJoint = skeleton.GetJoint(typeJoint[0]);
            Vector3 rightPos = rightJoint.ToVector3();

            nuitrack.Joint leftJoint = skeleton.GetJoint(typeJoint[1]);
            Vector3 leftPos = leftJoint.ToVector3();

            ///// Tracking user with camera. //////

            // Get right hand positions
            Vector3 rightNewPos = new Vector3(rightPos.x + camera.transform.position.x, rightPos.y + camera.transform.position.y, ZPosition);
            if (orthographic)
            {
                rightNewPos.x = rightNewPos.x / 100;
                rightNewPos.y = rightNewPos.y / 100;
            }

            rightHand.transform.position = rightNewPos;
            //Debug.Log("Right: " + rightNewPos);

            // Get left hand positions
            Vector3 leftNewPos = new Vector3(leftPos.x + camera.transform.position.x, leftPos.y + camera.transform.position.y, ZPosition);
            if (orthographic)
            {
                leftNewPos.x = leftNewPos.x / 100;
                leftNewPos.y = leftNewPos.y / 100;
            }
            leftHand.transform.position = leftNewPos;
            //Debug.Log("Left: " + leftNewPos);

           
        } // end if user is in frame

    }// end Update

    // This is for the hover to click functionality. MAYBE DELETE?
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Button" && hoverToClick)
        {
            Button button = other.GetComponent<Button>();

            StartCoroutine(ClickButton(button));
            StartCoroutine(ClickButton(button));
        }
    }// end OnTriggerEnter

    // This is for hover to click functionality. MAYBE DELETE?
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Button" && hoverToClick)
        {
            StopAllCoroutines();

            other.GetComponent<Button>().GetComponent<Text>().text = originalBtnText;
        }
    }

    // This is for hover to click functionality. MAYBE DELETE?
    IEnumerator ClickButton(Button button)
    {
        yield return new WaitForSeconds(3f);

        button.onClick.Invoke();
    }// end ClickButton

    // This is for hover to click functionality. MAYBE DELETE?
    IEnumerator ShowTimer(Button button)
    {
        int timeLeft = 3;

        originalBtnText = button.GetComponent<Text>().text;

        while (timeLeft >= 0)
        {
            yield return new WaitForSeconds(1);

            Debug.Log(timeLeft);

            button.GetComponent<Text>().text = originalBtnText + " (" + timeLeft.ToString() + ")"; 
        }

        button.GetComponent<Text>().text = originalBtnText;
    }// end ShowTimer

    ////// Scripts that can receive messages from other scripts.//////
  

    ///// Test function to make sure button is being pressed.//////
    public void ClickTest(Text message)
    {
        testCounter++;

        message.text = "Button clicked: " + testCounter.ToString();
    }

}

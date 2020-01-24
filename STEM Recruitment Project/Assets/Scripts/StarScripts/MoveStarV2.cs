using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script Summary ////////////////////////////////////////////////////////////
/*
 * Moves star around if the user has both halves "grabbed".
 */

public class MoveStarV2 : MonoBehaviour
{
    public GameObject rightHand, leftHand;
    public bool orthographic;

    //private GameObject rightHalf, leftHalf;
    private SpriteRenderer rightImage, leftImage;
    private Button button = null;
    private Vector3 originalPos;
    private Animator anim;
    private bool starBlocked = false;

    // Finds and saves both halves of the star, saves original position, and saves the
    // star's animation component for future use.
    private void Start()
    {
        GameObject rightHalf, leftHalf;

        rightHalf = this.transform.Find("RightHalfStar").gameObject;
        leftHalf = this.transform.Find("LeftHalfStar").gameObject;

        rightImage = rightHalf.GetComponent<SpriteRenderer>();
        leftImage = leftHalf.GetComponent<SpriteRenderer>();

        originalPos = this.transform.localPosition;

        anim = this.GetComponent<Animator>();
    }// end Start

    // Moves star if both halves are "grabbed". Also checks if one half is grabbed and the other
    // isn't, and fixes that.
    void Update()
    {
        // If both halves of the star are grabbed, and the star isn't blocked, move the star
        if ((IsGrabbed(rightImage) && IsGrabbed(leftImage)) && !starBlocked)
        {
            //using the position of the right and left hands to move the whole object
            Vector3 pos1 = leftHand.transform.position;
            Vector3 pos2 = rightHand.transform.position;

            // Find the midpoint. 
            Vector3 midPoint = (pos2 - pos1) / 2;

            // Continuously set the star's position to the midpoint.
            this.transform.position = midPoint + pos1;

           //if the user let go, stop moving the star
            if (UserLetGo())
            {
                // If the user let go over a button, click the button
                if (button != null)
                {
                    StartCoroutine(ClickButton(button));
                }

                // Disables both images to "let go"
                DisableImage(rightImage);
                DisableImage(leftImage);

            }// end if userletgo

        }// end if both halves are grabbed

        // If the star is blocked, make sure both halves remain disabled.
        if(starBlocked)
        {
            DisableImage(rightImage);
            DisableImage(leftImage);
        }

        // If the right half is enabled and the left isn't, let go of the right after a certain amount
        // of time
        if (IsGrabbed(rightImage) && !IsGrabbed(leftImage) && UserLetGo())
        {
            DisableImage(rightImage);
        }

        // If the left half is enabled and the right isn't, let go of the right after a certain amount
        // of time
        if (IsGrabbed(leftImage) && !IsGrabbed(rightImage) && UserLetGo())
        {
            DisableImage(leftImage);
        }
    }// end Update

    // If the star is over a button, temporarily save the button.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Button")
        {
            button = other.gameObject.GetComponent<Button>();
        }
    }// end OnTriggerEnter

    // If the star leaves a button, make sure no button is saved.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Button")
        {
            button = null;
        }
    }// end OnTriggerExit

    // Returns true if a given image is enabled. False if otherwise.
    bool IsGrabbed(SpriteRenderer image)
    {
        if (image.enabled == true)
        {
            return true;
        }
        return false;
    }// end IsGrabbed

    // Forces given image to be disabled.
    void DisableImage(SpriteRenderer image)
    {
        image.enabled = false;
    }// end DisableImage

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

    // Invokes the onClick function on a given button after the star's animation is done.
    IEnumerator ClickButton(Button thisButton)
    {
        // Change the star's position to the button's position.
        this.transform.position = new Vector3(button.transform.position.x, button.transform.position.y, this.transform.position.z);
        
        anim.Play("StarSelect");// Play the animation

        yield return new WaitForSeconds(0.70f); // This is how long it takes for the star to shrink

        thisButton.onClick.Invoke(); // Click button

        this.transform.localPosition = originalPos; // Set star's position back to its original

        button = null; // delete button that was saved

    }// end ClickButton

    // This can be accessed from other methods. Blocks star from moving (in case the user is dragging something
    // else).
    public void BlockStar(bool status)
    {
        starBlocked = status;
    }// end BlockStar

}// end MoveStarV2

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
        GameObject rightHalf, leftHalf;

        rightHalf = this.transform.Find("RightHalfStar").gameObject;
        leftHalf = this.transform.Find("LeftHalfStar").gameObject;

        rightImage = rightHalf.GetComponent<SpriteRenderer>();
        leftImage = leftHalf.GetComponent<SpriteRenderer>();

        originalPos = this.transform.localPosition;

        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((IsGrabbed(rightImage) && IsGrabbed(leftImage)) && !starBlocked)
        {
            //using the position of the right and left hands to move the whole object
            Vector3 pos1 = leftHand.transform.position;
            Vector3 pos2 = rightHand.transform.position;

            Vector3 midPoint = (pos2 - pos1) / 2;

            this.transform.position = midPoint + pos1;

           // Debug.Log(this.transform.position);
            if (UserLetGo())
            {
                // If the user let go over a button, click the button
                if (button != null)
                {
                    StartCoroutine(ClickButton(button));
                }

                DisableImage(rightImage);
                DisableImage(leftImage);

            }
        }
        if(starBlocked)
        {
            DisableImage(rightImage);
            DisableImage(leftImage);
        }

        if (IsGrabbed(rightImage) && !IsGrabbed(leftImage) && UserLetGo())
        {
            DisableImage(rightImage);
        }

        if (IsGrabbed(leftImage) && !IsGrabbed(rightImage) && UserLetGo())
        {
            DisableImage(leftImage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Button")
        {
            button = other.gameObject.GetComponent<Button>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Button")
        {
            button = null;
        }
    }

    bool IsGrabbed(SpriteRenderer image)
    {
        if (image.enabled == true)
        {
            return true;
        }
        return false;
    }

    void DisableImage(SpriteRenderer image)
    {
        image.enabled = false;
    }

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
    }

    IEnumerator ClickButton(Button thisButton)
    {
        this.transform.position = new Vector3(button.transform.position.x, button.transform.position.y, this.transform.position.z);

        anim.Play("StarSelect");

        yield return new WaitForSeconds(0.70f); // This is how long it takes for the star to shrink

        thisButton.onClick.Invoke(); // Click button

        this.transform.localPosition = originalPos;

        button = null;

    }

    public void BlockStar(bool status)
    {
        starBlocked = status;
    }

}

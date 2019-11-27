using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reset : MonoBehaviour
{
    public GameObject resetButton;
    public GameObject rightHand, leftHand;
    public Text showText;

    private float minX, minY, maxX, maxY;
    private bool hovering;
    private Vector3 initPos, resetPos, rightHandPos, leftHandPos;

    private void Start()
    {
        // Getting initial position of objects
        initPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        resetPos = new Vector3(resetButton.transform.position.x, resetButton.transform.position.y, resetButton.transform.position.z);

        // Getting ranges for resetButton
        minX = resetPos.x - 5;
        maxX = resetPos.x + 5;
        minY = resetPos.y - 5;
        maxY = resetPos.y + 5;

        hovering = false;
    }
    
    private void Update()
    {
        rightHandPos = new Vector3(rightHand.transform.position.x, rightHand.transform.position.y, rightHand.transform.position.z);
        leftHandPos = new Vector3(leftHand.transform.position.x, leftHand.transform.position.y, leftHand.transform.position.z);
        
        // While the user is hovering over the reset button
        if(isOverResetButton())
        {
            StartCoroutine(resetObject());
            StartCoroutine(showTime());
        }
        
        StopCoroutine(resetObject());
        StopCoroutine(showTime());
        
    }

    bool isOverResetButton()
    {
        // If the right hand is over the reset button, reset the object
        if ((rightHandPos.x >= minX && rightHandPos.x <= maxX) || (rightHandPos.y >= minY && rightHandPos.y <= maxY))
        {
            return true;
        }

        // If the left hand is over the reset button, reset the object
        else if ((leftHandPos.x >= minX && leftHandPos.x <= maxX) || (leftHandPos.y >= minY && leftHandPos.y <= maxY))
        {
            return true;
        }

        return false;
    }
    // resets objects after 3 seconds
    IEnumerator resetObject()
    {
        yield return new WaitForSeconds(3);

        // Send object to initial position
        this.transform.position = initPos;

    }

    // Countdown timer for object to reset.
    IEnumerator showTime()
    {
        int timeLeft = 3;

        while (timeLeft >= 0)
        {
            string newText = "(" + timeLeft + ")";

            showText.text = newText;

            yield return new WaitForSeconds(1);

            timeLeft--;
        }

        showText.text = "";
        
    }
}

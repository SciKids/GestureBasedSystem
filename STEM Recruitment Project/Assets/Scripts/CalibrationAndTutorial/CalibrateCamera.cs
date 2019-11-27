using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrateCamera : MonoBehaviour
{
    public GameObject rightHand, leftHand;
    public Camera camera;
    public float timeToKeepStill = 4.0f;
    public Text status, finals;
    public GameObject loadingCircle;
    
    private bool readyToCalibrate = false;
    private Vector3 rightPos, leftPos;
    private bool rightOk = false;
    private bool leftOk = false;
    private bool switchHands = false;
    private bool readyToResize = false;
    private float initialCameraSize, initialHandSize;

    void Start()
    {
        loadingCircle.SetActive(false);
        
        camera.orthographicSize = 6;

        initialCameraSize = camera.orthographicSize;

        initialHandSize = rightHand.transform.localScale.x;

        Debug.Log("screen width: " + Screen.width + " , height: " + Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        if(readyToCalibrate)
        {
            if(!switchHands)
            {
                StartCoroutine(CheckRightHand());
            }

            if(switchHands)
            {
                StartCoroutine(CheckLeftHand());
            }
        }

        if(readyToResize)
        {
            ResizeCamera();
        }
    }

    public void SetReady()
    {
        readyToCalibrate = true;

        switchHands = false;
    }

    public void ResetCameraSize()
    {
        camera.orthographicSize = initialCameraSize;

        rightHand.transform.localScale = new Vector3(initialHandSize, initialHandSize, initialHandSize);

        leftHand.transform.localScale = new Vector3(initialHandSize, initialHandSize, initialHandSize);

        status.text = "Camera size reset";

        finals.text = "";
    }

    IEnumerator CheckRightHand()
    {
        Vector3 firstPos, secondPos;

        firstPos = rightHand.transform.position;

        float timeLeft = timeToKeepStill;

        while(timeLeft >= 0)
        {
            yield return new WaitForSeconds(1f);

            secondPos = rightHand.transform.position;

            if (Mathf.Abs(secondPos.x - firstPos.x) > 2 || Mathf.Abs(secondPos.y - firstPos.y) > 2)
            {
                status.text = "Reading right hand";

                timeLeft = timeToKeepStill;

                firstPos = secondPos;

                loadingCircle.SetActive(false);
            }

            else
            {
                timeLeft -= 1f;

                loadingCircle.SetActive(true);

                loadingCircle.transform.position = rightHand.transform.position;
            }
        }

        secondPos = rightHand.transform.position;

        finals.text = "Right hand: " + rightHand.transform.position.ToString();

        Vector3 finalPos = (firstPos + secondPos) / 2;

        rightPos = finalPos;
        
        loadingCircle.SetActive(false);
        
        switchHands = true;
    
        StopAllCoroutines();
    }

    IEnumerator CheckLeftHand()
    {
        Vector3 firstPos, secondPos, temp;

        firstPos = leftHand.transform.position;

        float timeLeft = timeToKeepStill;

        while (timeLeft >= 0)
        {
            yield return new WaitForSeconds(1f);

            temp = leftHand.transform.position;

            if (Mathf.Abs(temp.x - firstPos.x) > 2 || Mathf.Abs(temp.y - firstPos.y) > 2)
            {
                status.text = "Reading left hand";

                timeLeft = timeToKeepStill;

                firstPos = temp;

                loadingCircle.SetActive(false);
            }
            else
            {
                timeLeft -= 1f;

                loadingCircle.SetActive(true);

                loadingCircle.transform.position = leftHand.transform.position;
            }
        }

        secondPos = leftHand.transform.position;

        finals.text += "\n Left hand: " + leftHand.transform.position.ToString();

        Vector3 finalPos = (firstPos + secondPos) / 2;

        leftPos = finalPos;

        leftOk = true;

        loadingCircle.SetActive(false);

        timeLeft = timeToKeepStill;

        StopAllCoroutines();

        readyToCalibrate = false;

        readyToResize = true;
    }

    void ResizeCamera()
    {
        status.text = "Initial camera size: " + camera.orthographicSize.ToString();

        // Formula is: (new_width * screen_height)/ (screen_width * 2)
        // https://www.youtube.com/watch?v=3xXlnSetHPM

        float top = Mathf.Abs(rightPos.x - leftPos.x) * Screen.height;
        float bottom = Screen.width / 2;

        float newSize = (top / bottom)/3;

        status.text += "\nNew size: " + newSize.ToString();

        camera.orthographicSize = newSize;

        float scale = newSize / initialCameraSize;

        float newHandSize = rightHand.transform.localScale.x * scale;

        rightHand.transform.localScale = new Vector3(newHandSize, newHandSize, newHandSize);

        leftHand.transform.localScale = new Vector3(newHandSize, newHandSize, newHandSize);

        readyToResize = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveScreen : MonoBehaviour
{
    public Vector3 newCamPos;
    public Camera camera;
    private bool okToChange;
    public Text timeDisplay;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3f;

    private void Start()
    {
        okToChange = false;
    }

    private void Update()
    {
        if(okToChange)
        {
            //camera.transform.position = Vector3.SmoothDamp(camera.transform.position, newCamPos, ref velocity, smoothTime);
            camera.transform.position = newCamPos;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay");
        if(other.gameObject.tag == "Hand")
        {
            StartCoroutine(ChangeScreen());
            StartCoroutine(ShowTime());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
        timeDisplay.text = "";
        okToChange = false;
    }

    IEnumerator ChangeScreen()
    {
        yield return new WaitForSeconds(3);

        okToChange = true;
    }

    IEnumerator ShowTime()
    {
        int timeLeft = 3;
        while (timeLeft >= 0)
        {
            Debug.Log(timeLeft);

            timeDisplay.text = timeLeft.ToString();

            yield return new WaitForSeconds(1);

            timeLeft--;
        }
        
    }

}

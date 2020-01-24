using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script Summary ////////////////////////////////////////////////////////////
/*
 * This is attached to the bar at the bottom of the worker screen. If a user hovers over
 * the bar, the job description appears.
 */

public class ShowJobDescriptionV2 : MonoBehaviour
{
    public GameObject bubble;

    Text jobDescription;
    string textToShow;

    // Makes sure bubble is hidden and find text box of job description
    private void Start()
    {
        bubble.SetActive(false);

        jobDescription = bubble.transform.Find("Canvas/Text").GetComponent<Text>();

    }// end Start

    // If a user hand collides with the bar, start the DelayBubble coroutine
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hand")
        {
            // Debug.Log("Hand collided with tab, calling DelayBubble()");
            StartCoroutine(DelayBubble());
        }
    }// end OnTriggerEnter

    // If the user hand moves away from the bar (and was already collided with it), hide bubble 
    // and description
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Hand")
        {
            StopAllCoroutines();

            // Debug.Log("DelayBubble stopped");

            bubble.SetActive(false);

            jobDescription.text = "";

            SendDescriptionStatus(false);

            ShowFeedback(true);
        }
    }// end OnTriggerExit

    // After half a second, show the bubble and job description.
    IEnumerator DelayBubble()
    {
        //Debug.Log("DelayBubble called");

        yield return new WaitForSeconds(0.5f);

        SendDescriptionStatus(true);

        bubble.SetActive(true);

        jobDescription.enabled = true;

        jobDescription.text = textToShow;

    }// end DelayBubble

    // Lets all workers know that the job description is showing/not showing
    private void SendDescriptionStatus(bool status)
    {
        for (int i = 1; i <= 6; i++)
        {
            string workerName = "Worker" + i.ToString();

            string fullPath = "/WorkerCanvas/WorkerScreen/" + workerName;

            GameObject worker = GameObject.Find(fullPath);

            GameObject canvas = worker.transform.Find("Canvas").gameObject;

            canvas.SendMessage("ReceiveDescriptBubbleStatus", status);
        }
    }// end SendDescriptionStatus

    // Tells workers to show feedback after evaluation.
    private void ShowFeedback(bool status)
    {
        for (int i = 1; i <= 6; i++)
        {
            string workerName = "Worker" + i.ToString();

            string fullPath = "/WorkerCanvas/WorkerScreen/" + workerName;

            GameObject worker = GameObject.Find(fullPath);

            GameObject canvas = worker.transform.Find("Canvas").gameObject;

            canvas.SendMessage("ShowOverallFeedback", status);
        }
    }// end ShowFeedback

    // Receives the job description from LoadGameInfoV2
    public void ReceiveDescript(string newDescript)
    {
        textToShow = newDescript;
    }// end ReceiveDescript
    
}// end ShowDescriptionV2

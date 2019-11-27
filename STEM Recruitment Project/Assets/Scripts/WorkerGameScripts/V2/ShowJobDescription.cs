using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowJobDescription : MonoBehaviour
{
    public GameObject bubble;

    Text jobDescription;
    string textToShow;
    private void Start()
    {
        bubble.SetActive(false);

        jobDescription = bubble.transform.Find("Text").GetComponent<Text>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hand")
        {
           // Debug.Log("Hand collided with tab, calling DelayBubble()");
            StartCoroutine(DelayBubble());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Hand")
        {
            StopAllCoroutines();

           // Debug.Log("DelayBubble stopped");

            bubble.SetActive(false);

            jobDescription.text = "";

            SendDescriptionStatus(false);

            ShowFeedback(true);
        }
    }

    IEnumerator DelayBubble()
    {
        //Debug.Log("DelayBubble called");

        yield return new WaitForSeconds(0.5f);

        SendDescriptionStatus(true);

        bubble.SetActive(true);

        jobDescription.enabled = true;

        jobDescription.text = textToShow;
        
    }

    private void SendDescriptionStatus(bool status)
    {
        for (int i = 1; i <= 6; i++)
        {
            string workerName = "Worker" + i.ToString();

            string fullPath = "/WholeGame/WorkerScreen/" + workerName;

            GameObject worker = GameObject.Find(fullPath);

            GameObject canvas = worker.transform.Find("Canvas").gameObject;

            canvas.SendMessage("ReceiveDescriptBubbleStatus", status);
        }
    }

    private void ShowFeedback(bool status)
    {
        for (int i = 1; i <= 6; i++)
        {
            string workerName = "Worker" + i.ToString();

            string fullPath = "/WholeGame/WorkerScreen/" + workerName;

            GameObject worker = GameObject.Find(fullPath);

            GameObject canvas = worker.transform.Find("Canvas").gameObject;

            canvas.SendMessage("ShowOverallFeedback", status);
        }
    }

    public void ReceiveDescript(string newDescript)
    {
        textToShow = newDescript;
    }
    
    
}

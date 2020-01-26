using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgeFeedback : MonoBehaviour
{
    string feedback;
    int score;
    GameObject judgeBubble;

    private void Awake()
    {
        judgeBubble = GameObject.Find("/Canvas/interviewPanel/JudgeBubble");
        judgeBubble.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hand")
        {
            StartCoroutine(DelayFeedback());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Hand")
        {
            StopAllCoroutines();

            judgeBubble.SetActive(false);
        }
    }

    // OnMouse methods for desktop use.
    private void OnMouseEnter()
    {
        StartCoroutine(DelayFeedback());
    }

    private void OnMouseExit()
    {
        StopAllCoroutines();

        judgeBubble.SetActive(false);
    }

    public void ReceiveFeedback(string newFeedback)
    {
        feedback = newFeedback;
    }

    public void ReceiveScore(int newScore)
    {
        score = newScore;
        ShowScore();
       // GameObject.Find(this.name + "/Text").GetComponent<Text>().text = newScore.ToString();
    }

    void ShowScore()
    {
        this.transform.Find("Text").GetComponent<Text>().text = score.ToString();
    }

    IEnumerator DelayFeedback()
    {
        yield return new WaitForSeconds(0.5f);

        judgeBubble.SetActive(true);

        judgeBubble.transform.Find("Canvas/Text").GetComponent<Text>().text = feedback;
    }
}

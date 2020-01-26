using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadThroughInterviewee : MonoBehaviour
{
    public GameObject[] otherTwo = new GameObject[2];
    private Text candidateText;
    private GameObject speechBubble, judge;

    // These 4 can be changed later. I only have these values so that I can cycle 
    // through each array and see the candidate's answers, score, and feedback
    // in that order.
    //private int ansIndex, fbIndex, scoresIndex, arrID;
    private int currentIndex = 0;
    private bool showAnswer = false;
    // Info found in ParseInterviewFile.
    private string[] answers, feedback;
    private int[] scores;
    private string pros, cons;
    // Start is called before the first frame update
    void Start()
    {
        speechBubble = GameObject.Find("Canvas/Speechbubble");

        judge = GameObject.Find("Canvas/Judge");
    
        speechBubble.SetActive(false);

        judge.SetActive(false);
        
    }

    // If a user hand collides with the worker, show the worker's answer after a given amount of time.
    private void OnTriggerEnter(Collider other)
    {
        if (showAnswer && other.tag == "Hand")
        {
           // Debug.Log("Hand collided with me!!");
            StartCoroutine(DelayAnswer());
        }
        
    }// end OnTriggerEnter

    // If a user hand leaves a worker, hide the worker's answer.
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hand")
        {
            StopAllCoroutines();

            speechBubble.SetActive(false);
        }
    }// end OnTriggerExit

    // For using desktop, rather than camera.
    private void OnMouseEnter()
    {
        StartCoroutine(DelayAnswer());
    }

    private void OnMouseExit()
    {
        StopAllCoroutines();

        speechBubble.SetActive(false);
    }

    // Delays answer by 1/2 a second. This helps with usability.
    IEnumerator DelayAnswer()
    {
        yield return new WaitForSeconds(0.5f);

        speechBubble.SetActive(true);

        speechBubble.transform.Find("Response").GetComponent<Text>().text = answers[currentIndex];
    }

    /*private void DeactivateOtherBubbles()
    {
        otherTwo[0].SendMessage("SetBubbleActive", false);
        otherTwo[1].SendMessage("SetBubbleActive", false);
    }

    public void SetBubbleActive(bool status)
    {
        speechBubble.SetActive(status);
    }*/
    

    // Enables/disables other two candidates' buttons
    public void EnableOtherTwoCandidates(bool status)
    {
        otherTwo[0].transform.Find("Canvas/Button").GetComponent<Button>().enabled = status;
        otherTwo[1].transform.Find("Canvas/Button").GetComponent<Button>().enabled = status;
    }

    // Accessed through ReadThroughQuestions. Ensures that the question and candidate answer index are the same.
    public void ChangeIndex(int num)
    {
        currentIndex = num;
    }

    // Allows bubble to be shown. 
    public void AllowBubble(bool status)
    {
        showAnswer = status;
    }
    
    // Enables/disables judge from other scripts
    public void EnableJudge(bool status)
    {
        judge.SetActive(status);
    }

    // Called from ButtonManagementV2. If this candidate is selected, activate the candidate's judge and 
    // send necessary info.
    public void SelectMe(bool status)
    {
        judge.SetActive(true);
        
        judge.SendMessage("ReceiveScore", scores[currentIndex]);

        judge.SendMessage("ReceiveFeedback", feedback[currentIndex]);
    }

    ///////////////////////// PUBLIC RECEIVE FUNCTIONS ///////////////////////////

    /*
     * These functions get all the candidate's information through Unity's 
     * SendMessage function. Look at function SendOutIntervieweeInfo in 
     * ParseInterviewFile
     */

    public void ReceiveAnswers(string[] newAnswers)
    {
        answers = newAnswers;
       // Debug.Log(this.name + "'s answer: " + answers[0]);
      /* for(int i = 0; i < newAnswers.Length; i++)
        {
            Debug.Log(this.name + "'s answer " + i + ": " + newAnswers[i]);
        }*/
    }

    public void ReceiveFeedback(string[] newFeedback)
    {
        feedback = newFeedback;
        /*for (int i = 0; i < feedback.Length; i++)
        {
            Debug.Log(this.name + "'s feedback " + i + ": " + feedback[i]);
        }*/
        // Debug.Log(this.name + "'s feedback: " + feedback[0]);
    }
    public void ReceiveScores(int[] newScores)
    {
        scores = newScores;
      //  Debug.Log(this.name + "'s score: " + scores[0]);
    }
    public void ReceivePros(string newPros)
    {
        pros = newPros;
      //  Debug.Log(this.name + "'s pros: " + pros);
    }
    public void ReceiveCons(string newCons)
    {
        cons = newCons;
      //  Debug.Log(this.name + "'s cons: " + cons);
    }
    /////////////////////// END PUBLIC GET FUNCTIONS /////////////////////////
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadThroughInterviewee : MonoBehaviour
{
    public GameObject[] otherTwo = new GameObject[2];
    private Text candidateText;
    private GameObject speechBubble;

    // These 4 can be changed later. I only have these values so that I can cycle 
    // through each array and see the candidate's answers, score, and feedback
    // in that order.
    private int ansIndex, fbIndex, scoresIndex, arrID;

    // Info found in ParseInterviewFile.
    private string[] answers, feedback;
    private int[] scores;
    private string pros, cons;

    // Start is called before the first frame update
    void Start()
    {
        string candidateName = this.name + "/";
        arrID = 1;
        ansIndex = 0;
        scoresIndex = 0;
        fbIndex = 0;
        speechBubble = GameObject.Find(candidateName+"Canvas/Speechbubble");
        candidateText = speechBubble.transform.Find("Text").gameObject.GetComponent<Text>();
        speechBubble.SetActive(false);
    }
    
    // This is just a tester function. Cycles through the answers, scores, and feedback
    // to make sure everything transferred fine. Used through the button.
    public void CycleThroughInfo()
    {
        speechBubble.SetActive(true);
        DeactivateOtherBubbles();
        // read answers array
        if (arrID == 1)
        {
            candidateText.text = answers[ansIndex];
            ansIndex++;
            arrID++;
        }

        // read score array
        else if (arrID == 2)
        {
            candidateText.text = "Score: " + scores[scoresIndex];
            scoresIndex++;
            arrID++;
        }

        // read feedback array
        else if (arrID == 3)
        {
            candidateText.text = "Feedback: " + feedback[fbIndex];
            fbIndex++;

            // If we've reach the end of all of the arrays, go to pros & cons
            if (fbIndex == feedback.Length)
            {
                arrID++;
            }

            // If not, go back to the answers array.
            else
            {
                arrID = 1;
            }
        }

        // read pros
        else if (arrID == 4)
        {
            candidateText.text = "PROS: " + pros;
            arrID++;
        }
        
        // read cons
        else if (arrID == 5)
        {
            candidateText.text = "CONS: " + cons;
        }
        
        // something screwed up if the index is > 5. 
        else
        {
            Debug.Log("ARRAY ID ERROR. ID IS " + arrID);
        }
    }

    public void SetBubbleActive(bool status)
    {
        speechBubble.SetActive(status);
    }

    private void DeactivateOtherBubbles()
    {
        otherTwo[0].SendMessage("SetBubbleActive", false);
        otherTwo[1].SendMessage("SetBubbleActive", false);
    }

    ///////////////////////// PUBLIC GET FUNCTIONS ///////////////////////////

    /*
     * These functions get all the candidate's information through Unity's 
     * SendMessage function. Look at function SendOutIntervieweeInfo in 
     * ParseInterviewFile
     */

    public void ReceiveAnswers(string[] newAnswers)
    {
        answers = newAnswers;
        Debug.Log(this.name + "'s answer: " + answers[0]);
    }
    public void ReceiveFeedback(string[] newFeedback)
    {
        feedback = newFeedback;
    }
    public void ReceiveScores(int[] newScores)
    {
        scores = newScores;
    }
    public void ReceivePros(string newPros)
    {
        pros = newPros;
    }
    public void ReceiveCons(string newCons)
    {
        cons = newCons;
    }

    /////////////////////// END PUBLIC GET FUNCTIONS /////////////////////////
}

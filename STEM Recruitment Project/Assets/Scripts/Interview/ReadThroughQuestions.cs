using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadThroughQuestions : MonoBehaviour
{
    // Changing things up a bit - I'm gonna use SerializeField to keep fields private while still
    // being able to access it in the editor.
    [SerializeField]
    float letterPause = 0.05f;

    [SerializeField]
    GameObject[] candidates, judges;

    [SerializeField]
    GameObject buttonManagement;

    string[] questions;
    int questionsIndex = 0;
    Text questionsText;
    //bool sentenceDone;

    private void Awake()
    {
        // I'm finding the component here, since this method is called in the start method in ParseInterviewFileV2.
        questionsText = this.GetComponent<Text>();
        questionsText.text = "";
        
    }// endAwake.
    
    // This is called through the Continue button. Goes to next question and lets candidate objects know that we're
    // moving on.
    public void NextQuestion()
    {
        questionsText.text = "";

        questionsIndex++;

        Debug.Log("Questions index: " + questionsIndex);

        for(int i = 0; i < candidates.Length; i++)
        {
            candidates[i].SendMessage("ChangeIndex", questionsIndex);
            candidates[i].SendMessage("EnableJudge", false);
        }

        StartCoroutine(TypeText());
    }// end NextQuestion

    public void ReceiveQuestions(string[] newQuestions)
    {
        Debug.Log("Receive Questions Called");
        
        questions = newQuestions;

        Debug.Log(questions[0]);

        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        // Allow sentence to be fully typed out before a candidate can be chosen.
        buttonManagement.SendMessage("EnableCandidates", false);

        // Block continue button, since user has to choose a candidate before going to next question
        buttonManagement.SendMessage("EnableContinueButton", false);
        
        // Type out sentence
        foreach (char letter in questions[questionsIndex].ToCharArray())
        {
            questionsText.text += letter;
            //questionsText.text += letter;
            yield return new WaitForSeconds(letterPause);
        }

        // Re-enable candidate after sentence is completed.
        buttonManagement.SendMessage("EnableCandidates", true);
       // sentenceDone = true;

    }// end TypeText
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadThroughQuestions : MonoBehaviour
{
    string[] questions;
    int questionsIndex;
    Text questionsText;

    // Start is called before the first frame update
    void Start()
    {
        questionsText = this.GetComponent<Text>();
    }
    public void NextQuestion()
    {
        if(questionsIndex != questions.Length)
        {
            questionsText.text = questions[questionsIndex];
            questionsIndex++;
        }
    }

    public void ReceiveQuestions(string[] newQuestions)
    {
        questions = newQuestions;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsPage : MonoBehaviour
{
    [SerializeField]
    private GameObject parsing;

    [SerializeField]
    private Button previousButton;

    private string[] questions, splash, allText;
    private Text title, description;
    private int questionsIndex = 0, splashIndex = 0;
    private bool ready = false;

    // Start is called before the first frame update
    void Start()
    {
        title = GameObject.Find("Title").GetComponent<Text>();
        description = GameObject.Find("Description").GetComponent<Text>();
        previousButton.interactable = false;
    }
    

    public void ReceiveTitle(string newTitle)
    {
        title.text = newTitle;
    }

    public void ReceiveSplashPage(string[] newSplash)
    {
        splash = newSplash;
    }

    public void ReceiveQuestions(string[] newQuestions)
    {
        questions = newQuestions;
       
    }

    public void StartInstructions(bool start)
    {
        if(start)
        {
            description.text = splash[splashIndex];
        }
    }
    
    public void NextText()
    {
        previousButton.interactable = true;
        
        if(ready)
        {
            parsing.SendMessage("StartGame", true);
        }

        else if(splashIndex != splash.Length - 1)
        {
            splashIndex++;

            description.text = splash[splashIndex];

            Debug.Log(splashIndex);
        }
        
        else
        {
            description.text = ""; 
            for(int i = 0; i < questions.Length; i++)
            {
                description.text += questions[i];

                description.text += "\n";
            }

            ready = true;
        }
        
    }

    public void PreviousText()
    {
        if(ready)
        {
            ready = false;

            splashIndex = splash.Length - 1;

            description.text = splash[splashIndex];
        }

        else
        {
            splashIndex--;

            description.text = splash[splashIndex];
        }

        if(splashIndex == 0)
        {
            previousButton.interactable = false;
        }
    }

}// end InstructionsPage

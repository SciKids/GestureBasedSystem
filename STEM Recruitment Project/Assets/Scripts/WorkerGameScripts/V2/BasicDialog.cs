using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicDialog : MonoBehaviour
{
    public float letterPause = 0.05f;
    public string[] sentences;
    public Text dialogBox;
    public Button nextButton, nextScreenButton;

    private bool sentenceDone;
    private int index = 0;
    private int allSentences;
    
    void Start()
    {
     //   StartDialog();  
    }

    // Receives messages from LoadGameInfo
    public void ReceiveDialog(string[] dialog)
    {
        sentences = dialog;

        StartDialog();
    }

    void Update()
    {
        if (sentenceDone)
        {
            // If we've reached the end of the sentences list, 
            // deactivate next sentence button and activate 
            // next screen button
            if (index == allSentences)
            {
                nextButton.gameObject.SetActive(false);
                nextButton.enabled = false;

                nextScreenButton.gameObject.SetActive(true);
                nextScreenButton.enabled = true;
            }
            // otherwise, only activate next sentence button.
            else
            {
                nextButton.gameObject.SetActive(true);
                nextButton.enabled = true;
            }
        }
        // While a sentence isn't done, keep next sentence button
        // deactivated
        else
        {
            nextButton.gameObject.SetActive(false);
            nextButton.enabled = false;
        }
    }


    IEnumerator TypeText()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            dialogBox.text += letter;
            yield return new WaitForSeconds(letterPause);
        }

        sentenceDone = true;
        index++;
    }
    
    // This is called in the OnClick() in the nextSentenceButton.
    public void NextSentence()
    {
        sentenceDone = false;

        dialogBox.text = "";

        StartCoroutine(TypeText());
    }
    // Can be called elsewhere
    public void StartDialog()
    {
        index = 0;
        StopAllCoroutines();
        
        dialogBox.text = "";
        sentenceDone = false;
        allSentences = sentences.Length;

        StartCoroutine(TypeText());

        nextButton.gameObject.SetActive(false);
        nextButton.enabled = false;

        nextScreenButton.gameObject.SetActive(false);
        nextScreenButton.enabled = false;
    }
}

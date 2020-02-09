using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/* class to handle panel changes and button presses for continue and results panel. */

public class sceneChange : MonoBehaviour
{

    // initalize variables 
    private int index = 0;
    private float typingSpeed; 
    private string[] sentences;
    private Text questionDisplay;
    private GameObject jobPanel;
    private GameObject resultsPanel; 
    private GameObject continueBtn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            questionDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        continueBtn.SetActive(false); // hide continue button
        continueBtn.GetComponentInChildren<Text>().text = "Continue";
        // checks if at the end of sentece
        if (index < sentences.Length - 1)
        {
            index++;
            questionDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            questionDisplay.text = "";
            continueBtn.SetActive(false); // hide continue button
            continueBtn.GetComponentInChildren<Text>().text = "Continue";
        }
    }

    public void jobDescriptionShow()
    {
        // makes job description visable 
        jobPanel.SetActive(true);
        resultsPanel.SetActive(false); 
    }

    public void resultsShow()
    {
        // makes job description visable 
        resultsPanel.SetActive(true);
        jobPanel.SetActive(false); 
    }

    public void hidePanels()
    {
        // hides job description 
        jobPanel.SetActive(false);
        resultsPanel.SetActive(false); 
    }
}

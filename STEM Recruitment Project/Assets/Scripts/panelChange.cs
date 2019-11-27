using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class panelChange : MonoBehaviour
{
    public GameObject gamePanel;
    public GameObject choicePanel;
    public GameObject resultPanel;
    public GameObject finalPanel; 

    // For making sure text is accurate.
    public Button choiceBtn;
    public Button backBtn;
    //public GameObject backBtn;
    int test = 1; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changePanel()
    {
        gamePanel.gameObject.SetActive(false);
        choicePanel.gameObject.SetActive(true);
       // choiceBtn.GetComponentInChildren<Text>().text = "Job Details"; // resetting text
    }
    
    public void backPanel()
    {
        gamePanel.gameObject.SetActive(true);
        //backBtn.GetComponentInChildren<Text>().text = "Back"; // resetting text
        choicePanel.gameObject.SetActive(false);
    }

    public void endPanel()
    {
        gamePanel.gameObject.SetActive(false);
        resultPanel.gameObject.SetActive(true);
    }

    public void gameReturn()
    {
        gamePanel.gameObject.SetActive(false);
        resultPanel.gameObject.SetActive(false);
        finalPanel.gameObject.SetActive(true); 
    }
}

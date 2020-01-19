using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonManagement : MonoBehaviour
{
    // Intsance variables
    public int size;
    public string[] sentences;
    public string userChoice;
    public string[] gregAns;
    public string[] lisaAns;
    public string[] tyroneAns;
    public string[] userFeedback;
    public string[] gScore2;
    public string[] lScore2;
    public string[] tScore2;
    public string[] noscore;
    public string[] gfeed, lfeed, tfeed;

    private int index = 0;
    public int counter = 1;
    public int anscount = 0; // determines what index of answers arrays to display
    public int ans;
    public int clickcnt = 0;
    public int cClick = 0;
    public int totalcount = 0; // keeps track of how many questions have been answered
    public float typingSpeed; // determines the speed of text apperance for questions
    public int person1, person2, person3;
    public int final1, final2, final3;
    public int winner;
    public int temp = 1;

    // Objects in Unity
    public GameObject continueBtn;
    public GameObject choiceBtn;
    public GameObject p1Btn, p2Btn, p3Btn;
    public GameObject p1B2, p2B2, p3B2;
    public GameObject gregsp, lisasp, tyronesp;
    public GameObject p1, p2, p3;
    public GameObject jobdescription;
    public GameObject feedBtn;
    public GameObject score2, gscoreF, lscoreF, tscoreF;
    public GameObject gscored, lscored, tscored;  // minijuge scores
    public GameObject jresp, jtext;

    public Text textDisplay; // Questions
    public Text gregR, lisaR, tyroneR; //  Responses of interviewies
    public Text crit; // responses to the answers for each interview question
    public Text result; // Displays who the user chose and summary of interviewies. 9286079754
    public Text feedback, feedFinal; // Dispalys developer feedback on user choice
    public Text pro, con; // pros and cons of each individual

    //private string continueBtnStr = "Continue";
    // Start is called before the first frame update
    void Start()
    {
        // disables character name buttons
        p1Btn.GetComponent<Button>().interactable = false;
        p2Btn.GetComponent<Button>().interactable = false;
        p3Btn.GetComponent<Button>().interactable = false;

        continueBtn.SetActive(false);

        // hides speech from characters
        gregR.enabled = false;
        lisaR.enabled = false;
        tyroneR.enabled = false;

        // hides feedback of character on feedback panel
        crit.enabled = false;
        jresp.SetActive(false);

        // hides speech bubble image
        gregsp.SetActive(false);
        lisasp.SetActive(false);
        tyronesp.SetActive(false);

        // hides minijudges
        gscored.SetActive(false);
        lscored.SetActive(false);
        tscored.SetActive(false);

        // feedBtn.GetComponent<Button>().interactable = false;
        feedBtn.SetActive(false);

        StartCoroutine(Type());
    }

    // Update is called once per frame
    void Update()
    {
        displayScore();

        // name buttons uninteractable with user when not prompting person choice
        p1Btn.GetComponent<Button>().interactable = true;
        p2Btn.GetComponent<Button>().interactable = true;
        p3Btn.GetComponent<Button>().interactable = true;

        Debug.Log("CLICK count: " + clickcnt);
        Debug.Log("Continue CLICK: " + cClick);
        p1Btn.GetComponentInChildren<Text>().text = "Greg";
        p2Btn.GetComponentInChildren<Text>().text = "Lisa";
        p3Btn.GetComponentInChildren<Text>().text = "Tyrone";

        if (sentences[index] == "Choose who you would hire, then click the results button.")
        {
            Debug.Log("Switching buttons");
            p1Btn.SetActive(false);
            p2Btn.SetActive(false);
            p3Btn.SetActive(false);

            p1B2.SetActive(true);
            p2B2.SetActive(true);
            p3B2.SetActive(true);

            //resultsBtn.SetActive(true);
            //feedBtn.SetActive(true);
        }

        int temp = counter - 1;

        if (temp != counter)
        {
            //Debug.Log("Temp: " + temp + " Counter: " + counter);
            continueBtn.GetComponent<Button>().interactable = true;
        }

        if (anscount == totalcount)
        {
            feedBtn.GetComponent<Button>().interactable = false;
            //totalcount =+ 1;
        }

    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
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
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = "";
            continueBtn.SetActive(false); // hide continue button
            continueBtn.GetComponentInChildren<Text>().text = "Continue";
        }
    }

    public void continueClick()
    {
        // hides minijudges
        gscored.SetActive(false);
        lscored.SetActive(false);
        tscored.SetActive(false);

        // counts clicks
        cClick += 1;
        //continueBtn.GetComponent<Button>().interactable = false;
        continueBtn.SetActive(false);

    }

    public void ButtonClick(Button btn)
    {
        winner = 0;

        if (btn.name == "p1Btn")
        {
            person1 += 1;
            winner += 5;
            Debug.Log("Updated player 1");
            //   totalcount += 1;
            //feedBtn.GetComponent<Button>().interactable = true;
            p1Btn.GetComponent<Button>().interactable = false;
            p2Btn.GetComponent<Button>().interactable = false;
            p3Btn.GetComponent<Button>().interactable = false;
        }

        else if (btn.name == "p2Btn")
        {
            person2 += 1;
            winner += 10;
            Debug.Log("Updated Player 2");
            //    totalcount += 1;
            // feedBtn.GetComponent<Button>().interactable = true;
            p1Btn.GetComponent<Button>().interactable = false;
            p2Btn.GetComponent<Button>().interactable = false;
            p3Btn.GetComponent<Button>().interactable = false;
        }
        else if (btn.name == "p3Btn")
        {
            person3 += 1;
            winner += 15;
            Debug.Log("Updated Player 3");
            //    totalcount += 1;
            //feedBtn.GetComponent<Button>().interactable = true;
            p1Btn.GetComponent<Button>().interactable = false;
            p2Btn.GetComponent<Button>().interactable = false;
            p3Btn.GetComponent<Button>().interactable = false;
        }
        else
        {
            Debug.Log("No button press detected");
        }
        clickcnt += 1;

        counter += 1;
        continueBtn.SetActive(true);
        continueBtn.GetComponent<Button>().interactable = true; // after choice made, returns continue button to screen

        p1Btn.GetComponentInChildren<Text>().text = "Greg";
        p2Btn.GetComponentInChildren<Text>().text = "Lisa";
        p3Btn.GetComponentInChildren<Text>().text = "Tyrone";

        temp = anscount - 1;
        Debug.Log(person1 + ", " + person2 + ", " + person3);

        //continueBtn.GetComponent<Button>().interactable = true;
        continueBtn.SetActive(true);
    }

    // determines what happens when the mouse moves over characters
    public void gregOver()
    {
        System.Threading.Thread.Sleep(1500); // delays display of text 
        //System.Threading.Thread.Sleep(1000); // delays display of text 
        gregR.enabled = true; //enables text response to answer
        gregsp.SetActive(true); // shows speech bubble image
        Debug.Log(anscount);
        if (cClick > 2) { cClick = 2; } // Stops array out of bounds error
        gregR.GetComponent<Text>().text = gregAns[cClick];

    }

    public void lisaOver()
    {

        System.Threading.Thread.Sleep(1000); // delays display of text 
        lisaR.enabled = true;
        lisasp.SetActive(true);
        if (cClick > 2) { cClick = 2; } // Stops array out of bounds error
        lisaR.GetComponent<Text>().text = lisaAns[cClick];
    }

    public void tyroneOver()
    {
        System.Threading.Thread.Sleep(1000); // delays display of text 
        tyroneR.enabled = true;
        tyronesp.SetActive(true);
        if (cClick > 2) { cClick = 2; } // Stops array out of bounds error
        tyroneR.GetComponent<Text>().text = tyroneAns[cClick];
    }

    public void gsOver()
    {
        System.Threading.Thread.Sleep(1000); // delays display of text 
        jresp.SetActive(true); // shows critisim speech bubble
        if (cClick > 2) { cClick = 2; } // Stops array out of bounds error
        jtext.GetComponent<Text>().text = gfeed[cClick]; // displays jude repsonse text

    }

    public void lsOver()
    {
        System.Threading.Thread.Sleep(1000); // delays display of text 
        jresp.SetActive(true); // shows critisim speech bubble
        if (cClick > 2) { cClick = 2; } // Stops array out of bounds error
        jtext.GetComponent<Text>().text = lfeed[cClick]; // displays jude repsonse text
    }

    public void tsOver()
    {
        System.Threading.Thread.Sleep(1000); // delays display of text 
        jresp.SetActive(true); // shows critisim speech bubble
        if (cClick > 2) { cClick = 2; } // Stops array out of bounds error
        jtext.GetComponent<Text>().text = tfeed[cClick]; // displays jude repsonse text
    }

    public void Response()
    {
        if (cClick > 2) { cClick = 2; } // stops array out of bounds error
        Debug.Log("Ans Response Value " + (cClick));
        if (winner == 5)
        {
            crit.enabled = true;
            crit.GetComponent<Text>().text = gfeed[cClick];
        }
        else if (winner == 10)
        {
            crit.enabled = true;
            crit.GetComponent<Text>().text = lfeed[cClick];
        }

        else if (winner == 15)
        {
            crit.enabled = true;
            crit.GetComponent<Text>().text = tfeed[cClick];
        }
    }

    // removes critisizim text from screen
    public void endfeed()
    {
        crit.enabled = false;
        crit.enabled = false;
        crit.enabled = false;
        // totalcount = +1;
    }

    public void mouseLeave()
    {
        // hides speech bubble image
        gregsp.SetActive(false);
        lisasp.SetActive(false);
        tyronesp.SetActive(false);

        // hides minijudge response
        jresp.SetActive(false);
    }

    public void displayScore()
    {
        //summary.GetComponentInChildren<Text>().text = "Greg answeered " + person1 + " correct, Lisa answered " + person2 + " correct, and Tyrone answered " + person3 + " correct.";
        if (cClick > 2) { cClick = 2; } // stops index out of bounds error

        if (winner == 5) //Greg
        {
            Debug.Log("temp " + temp);
            //score1.GetComponentInChildren<Text>().text = gScore1[cClick];
            score2.GetComponentInChildren<Text>().text = gScore2[cClick];
            gscoreF.GetComponentInChildren<Text>().text = gScore2[cClick];
            gscored.SetActive(true);  // displays minijudge
                                      //score3.GetComponentInChildren<Text>().text = gScore3[cClick];
        }

        else if (winner == 10) //Lisa
        {
            //score1.GetComponentInChildren<Text>().text = lScore1[cClick];
            score2.GetComponentInChildren<Text>().text = lScore2[cClick];
            lscoreF.GetComponent<Text>().text = lScore2[cClick];
            lscored.SetActive(true);
            //score3.GetComponentInChildren<Text>().text = lScore3[cClick];
        }

        else if (winner == 15) //Tyrone
        {
            //score1.GetComponentInChildren<Text>().text = tScore1[cClick];
            score2.GetComponentInChildren<Text>().text = tScore2[cClick];
            tscoreF.GetComponentInChildren<Text>().text = tScore2[cClick];
            tscored.SetActive(true);
            //score3.GetComponentInChildren<Text>().text = tScore3[cClick];
        }

        // Displays 'X' for score
        else
        {
            //score1.GetComponentInChildren<Text>().text = noscore[0];
            score2.GetComponentInChildren<Text>().text = noscore[0];
            gscoreF.GetComponentInChildren<Text>().text = noscore[0];
            lscoreF.GetComponent<Text>().text = noscore[0];
            tscoreF.GetComponentInChildren<Text>().text = noscore[0];
            //score3.GetComponentInChildren<Text>().text = noscore[0];
        }
    }
}
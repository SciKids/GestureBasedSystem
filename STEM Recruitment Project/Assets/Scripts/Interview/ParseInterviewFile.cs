using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParseInterviewFile : MonoBehaviour
{
    private string[] allFileInfo;
    private Interviewee[] interviewees;
    private string[] questions;
    private string[] otherJobInfo;

    // Start is called before the first frame update
    void Start()
    {
        // Load all interview files
        string path = "InterviewGameInfo/";
        TextAsset[] allFiles = Resources.LoadAll<TextAsset>(path);

        // Count number of available files.
        int numOfFiles = allFiles.Length;

        // Randomly pick interview file
        System.Random rand = new System.Random();
        int fileNum = rand.Next(1, numOfFiles + 1);

        // Get randomized file
        TextAsset file = Resources.Load<TextAsset>(path + "Interview" + fileNum.ToString());

        // Put info into array by splitting text with ';'.
        string[] fileInfo = file.text.Split(';');

        // Getting rid of newlines
        for (int i = 0; i < fileInfo.Length; i++)
        {
            // Lines 1 and 38 - 42 can have newlines. Everything else can't.
            // CHANGE if this isn't the case.
            if (i != 1 && i < 40)
            {
                fileInfo[i] = fileInfo[i].Replace("\n", string.Empty);
            }

            // Test print statements. Making sure everything's in the array.
            //Debug.Log(fileInfo[i]);
        }

        /////// Start organizing file./////////////

        // Organize all interviewee info
        OrganizeInterviewees(fileInfo);

        // Organize all interview questions
        OrganizeInterviewQuestions(fileInfo);

        // Organize all other info
        OrganizeOtherJobInfo(fileInfo);

        // TEST STUFF
        Debug.Log("Lisa's stuff:");
        string[] testAnswers = interviewees[1].GetAnswers();
        string[] testFeedback = interviewees[1].GetFeedback();
        int[] testScores = interviewees[1].GetScores();
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("Answer for Q" + (i+1) + ": " + testAnswers[i]);
            Debug.Log("Feedback for Q" + (i+1) + ": " + testFeedback[i]);
            Debug.Log("Score for Q" + (i+1) + ": " + testScores[i]);
        }

        Debug.Log("PROS: " + interviewees[1].GetPros());
        Debug.Log("CONS: " + interviewees[1].GetCons());

        // Send out info to interviewees
        SendInterviewInfo();
    } // end Start


      ///////////////////////////////////// START OF ORGANIZATIONAL FUNCTIONS ///////////////////////////////////////////
    
      /***************************************************************************************************************
       ** NOTE: Maybe we should change the layout of the template...like group each interviewee's info together? Rather
       ** than structure it by the flow of the game. I think this would make it easier to parse through, especially
       ** if there are more than 3 questions.
       ** The next 3 functions are pretty hard-coded. If more content is added, the 3 organize functions need to be
       ** changed.
       ***************************************************************************************************************/

    void OrganizeInterviewees(string[] fileInfo)
    {
        // Get greg's info
        string[] gregAnswers = { fileInfo[3], fileInfo[13], fileInfo[23] };
        string[] gregFeedback = { fileInfo[6], fileInfo[16], fileInfo[26] };
        int[] gregScores = { Int32.Parse(fileInfo[7]), Int32.Parse(fileInfo[17]), Int32.Parse(fileInfo[27]) };
        string gregPros = fileInfo[32];
        string gregCons = fileInfo[33];

        // Get lisa's info
        string[] lisaAnswers = { fileInfo[4], fileInfo[14], fileInfo[24] };
        string[] lisaFeedback = { fileInfo[8], fileInfo[18], fileInfo[28] };
        int[] lisaScores = { Int32.Parse(fileInfo[9]), Int32.Parse(fileInfo[19]), Int32.Parse(fileInfo[29]) };
        string lisaPros = fileInfo[34];
        string lisaCons = fileInfo[35];

        // Get tyrone's info
        string[] tyAnswers = { fileInfo[5], fileInfo[15], fileInfo[25] };
        string[] tyFeedback = { fileInfo[10], fileInfo[20], fileInfo[30] };
        int[] tyScores = { Int32.Parse(fileInfo[11]), Int32.Parse(fileInfo[21]), Int32.Parse(fileInfo[31]) };
        string tyPros = fileInfo[36];
        string tyCons = fileInfo[37];

        // Save all interviewee info
        Interviewee greg = new Interviewee(gregAnswers, gregFeedback, gregScores, gregPros, gregCons);
        Interviewee lisa = new Interviewee(lisaAnswers, lisaFeedback, lisaScores, lisaPros, lisaCons);
        Interviewee tyrone = new Interviewee(tyAnswers, tyFeedback, tyScores, tyPros, tyCons);

        interviewees = new Interviewee[3]{ greg, lisa, tyrone };
        
    } // end OrganizeInterviewees.

    // Get the interview questions
    void OrganizeInterviewQuestions(string[] fileInfo)
    {
        questions = new string[3] { fileInfo[2], fileInfo[12], fileInfo[22] };
    } // end OrganizeInterviewQuestions

    // Get all other info.
    void OrganizeOtherJobInfo(string[] fileInfo)
    {
        otherJobInfo = new string[7]{ fileInfo[0], fileInfo[1], fileInfo[38], fileInfo[39], fileInfo[40], fileInfo[41], fileInfo[42] };
    } // end OrganizeOtherJobInfo

    ///////////////////////////////////// END OF ORGANIZATIONAL FUNCTIONS ///////////////////////////////////////////

    // Sends out all info for interviewees.
    void SendInterviewInfo()
    {
        // Find each interviewee. Each person is a child object of the canvas.
        GameObject greg = GameObject.Find("Greg");
        GameObject lisa = GameObject.Find("Lisa");
        GameObject tyrone = GameObject.Find("Tyrone");

        // Send out greg's information
        greg.SendMessage("ReceiveAnswers", interviewees[0].GetAnswers());
        greg.SendMessage("ReceiveFeedback", interviewees[0].GetFeedback());
        greg.SendMessage("ReceiveScores", interviewees[0].GetScores());
        greg.SendMessage("ReceivePros", interviewees[0].GetPros());
        greg.SendMessage("ReceiveCons", interviewees[0].GetCons());

        // Send out lisa's information
        lisa.SendMessage("ReceiveAnswers", interviewees[1].GetAnswers());
        lisa.SendMessage("ReceiveFeedback", interviewees[1].GetFeedback());
        lisa.SendMessage("ReceiveScores", interviewees[1].GetScores());
        lisa.SendMessage("ReceivePros", interviewees[1].GetPros());
        lisa.SendMessage("ReceiveCons", interviewees[1].GetCons());

        // Send out greg's information
        tyrone.SendMessage("ReceiveAnswers", interviewees[2].GetAnswers());
        tyrone.SendMessage("ReceiveFeedback", interviewees[2].GetFeedback());
        tyrone.SendMessage("ReceiveScores", interviewees[2].GetScores());
        tyrone.SendMessage("ReceivePros", interviewees[2].GetPros());
        tyrone.SendMessage("ReceiveCons", interviewees[2].GetCons());

        // Send questions
        GameObject questionsText = GameObject.Find("/Canvas/Questions");
        questionsText.SendMessage("ReceiveQuestions", questions);

    }
    // Node that holds all info about specfific interviewee.
    class Interviewee
    {
        // Assuming that each candidate has 3 questions to answer. 
        // CHANGE if this isn't the case.
        string[] answers = new string[3];
        string[] feedback = new string[3];
        int[] scores = new int[3];
        string pros, cons;

        public Interviewee(string[] newAnswers, string[] newFeedback, int[] newScores, string newPros, string newCons)
        {
            answers = newAnswers;
            feedback = newFeedback;
            scores = newScores;
            pros = newPros;
            cons = newCons;
        }

        public string[] GetAnswers()
        {
            return answers;
        }

        public string[] GetFeedback()
        {
            return feedback;
        }

        public int[] GetScores()
        {
            return scores;
        }

        public string GetPros()
        {
            return pros;
        }

        public string GetCons()
        {
            return cons;
        }
    }
    
}

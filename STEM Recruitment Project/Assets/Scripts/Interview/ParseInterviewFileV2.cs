using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using System;
using System.IO;

public class ParseInterviewFileV2 : MonoBehaviour
{
    [SerializeField]
    private GameObject[] candidateObjects;

    [SerializeField]
    private GameObject questionsScript, jobPanel, resultsPanel, instructionsCanvas, mainGameCanvas;
    
    private Interviewee[] candidates;
    private JobInfo jobInfoNode;

    // Temporarily writes all info in a file that's easier to read from.
    private void Awake()
    {
        candidates = new Interviewee[candidateObjects.Length];
        // Load all interview files
        string path = "InterviewGameInfo2/";
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

        // Get new file path
        string filePath = GetFilePath();

        // Create new file
        try
        {
            // Create new file or empty existing file
            File.WriteAllText(filePath, string.Empty);

            TextWriter writer = new StreamWriter(filePath);

            // Write all info into file
            for(int i = 0; i < fileInfo.Length; i++)
            {
                writer.Write(fileInfo[i]);
            }

            writer.Close();
        }

        catch(Exception e)
        {
            Debug.Log("Could not read/write temp interview file.");
            Debug.Log(e);
        }
    }// end Awake

    // Start is called before the first frame update
    void Start()
    {
        string filePath = GetFilePath();

        try
        {
            StreamReader reader = File.OpenText(filePath);
            
            int numOfLines = File.ReadAllLines(filePath).Count();

            string[] fileInfo = File.ReadAllLines(filePath);

            // list of markers that are found within the file
            string jobStartString = "SPLASH_PAGE";
            string jobEndString = "QUESTIONS_END";
            string candidateStartString = "ANSWERS";
            string candidateEndString = "CONS_END";

            int jobStart = 0, jobEnd = 0, candidateStart = 0, candidateEnd = 0;

            // Check for markers in the file, and save the markers' locations.
            // First, I'm splitting the job stuff from the candidate stuff.
            for (int i = 0; i < fileInfo.Length; i++)
            {
                string str = fileInfo[i];

                if (str.Equals(jobStartString))
                {   
                    jobStart = i;
                }
                if (str.Equals(jobEndString))
                {   
                    jobEnd = i;
                }
                if (str.Equals(candidateStartString))
                {   
                    candidateStart = i;
                }
                if (str.Equals(candidateEndString))
                {
                    candidateEnd = i;
                }
            }// end for loop

            Debug.Log("Job start at index " + jobStart);
            Debug.Log("Job end at index " + jobEnd);
            Debug.Log("Candidate start at index " + candidateStart);
            Debug.Log("Candidate end at index " + candidateEnd);

            OrganizeJobInfo(fileInfo, jobStart, jobEnd);
            OrganizeCandidates(fileInfo, candidateStart, candidateEnd);
            SendInfoToInstructionsPage();
            //SendCandidateInfo();
           // SendJobInfo();
            //Debug.Log(numOfLines);
            reader.Close();
        }// end try
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }// end Start

    public void StartGame(bool status)
    {
        mainGameCanvas.SetActive(true);
        instructionsCanvas.SetActive(false);
        SendCandidateInfo();
        SendJobInfo();
    }

    public void SendResultsPanelInfo(int index)
    {
        resultsPanel.SendMessage("ReceiveName", candidateObjects[index].name);
        resultsPanel.SendMessage("ReceivePros", candidates[index].GetPros());
        resultsPanel.SendMessage("ReceiveCons", candidates[index].GetCons());
    }

    // Further organizes all the job stuff. This includes splash screen text, job title & information,
    // and interview questions
    void OrganizeJobInfo(string[] fileInfo, int startIndex, int endIndex)
    {
        // All needed variables for new node
        int splashPageStart = 0, infoStart = 0, questionsStart = 0, infoEnd = 0, splashPageEnd = 0, questionsEnd = 0;
        
        // Get a sub array with just job information
        //int len = endIndex - startIndex;

        // Get start points & lengths of everything
        for(int i = 0; i < fileInfo.Length-1; i++)
        {
            if(fileInfo[i].Equals("SPLASH_PAGE"))
            {
                splashPageStart = i;
            }

            if (fileInfo[i].Equals("INFORMATION"))
            {
                infoStart = i;
            }

            if (fileInfo[i].Equals("QUESTIONS"))
            {
                questionsStart = i;
            }

            if(fileInfo[i].Equals("SPLASH_PAGE_END"))
            {
                splashPageEnd = i;
            }

            if(fileInfo[i].Equals("INFORMATION_END"))
            {
                infoEnd = i;
            }

            if(fileInfo[i].Equals("QUESTIONS_END"))
            {
                questionsEnd = i;
                break; // this is all we need at this point.
            }
        }// end for loop

        string[] splashPageInfo = GetArrayInfo(fileInfo, splashPageStart+1, splashPageEnd-1);
        string[] questions = GetArrayInfo(fileInfo, questionsStart+1, questionsEnd-1);

        string[] allInfoArr = GetArrayInfo(fileInfo, infoStart, infoEnd);

        // Get job title, which is right after the INFORMATION tag
        string jobTitle = fileInfo[infoStart + 1];

        string[] jobInfo = GetArrayInfo(fileInfo, infoStart + 2, infoEnd - 1);
        // Get the rest of the job information.

        // Save all info into JobInfo node
        jobInfoNode = new JobInfo(splashPageInfo, questions, jobTitle, jobInfo);
        //Debug.Log(jobInfoNode.GetJobTitle());
        Debug.Log("Questions length: " + jobInfoNode.GetQuestions().Length);
        // Get job information
        //jobTitle = splashPageInfo[infoStart];

    }// end OrganizeJobInfo
    
    // Further organizes all of the candidate's information. This includes candidate answers, feedback,
    // scores, pros, and cons.
    void OrganizeCandidates(string[] fileInfo, int startIndex, int endIndex)
    {
        int answersStart = 0, feedbackStart = 0, scoresStart = 0, prosStart = 0, consStart = 0;
        int answersEnd = 0, feedbackEnd = 0, scoresEnd = 0, prosEnd = 0, consEnd = 0;

        // Check for markers in the file, and save the start & end spots of each section.
        for(int i = 0; i < fileInfo.Length; i++)
        {
            if (fileInfo[i].Equals("ANSWERS"))
            {
                answersStart = i+1;
            }
            if (fileInfo[i].Equals("ANSWERS_END"))
            {
                answersEnd = i-1;
            }
            if (fileInfo[i].Equals("FEEDBACK"))
            {
                feedbackStart = i+1;
            }
            if (fileInfo[i].Equals("FEEDBACK_END"))
            {
                feedbackEnd = i-1;
            }
            if (fileInfo[i].Equals("SCORES"))
            {
                scoresStart = i+1;
            }
            if (fileInfo[i].Equals("SCORES_END"))
            {
                scoresEnd = i-1;
            }
            if (fileInfo[i].Equals("PROS"))
            {
                prosStart = i+1;
            }
            /*if (fileInfo[i].Equals("PROS_END"))
            {
                prosEnd = i-1;
            }*/
            if (fileInfo[i].Equals("CONS"))
            {
                consStart = i+1;
                break;
            }
            /*if (fileInfo[i].Equals("CONS_END"))
            {
                consEnd = i-1;
                break;
            }*/
        }// end for

        int questionsLen = jobInfoNode.GetQuestions().Length;
        /*Debug.Log("Answers start at index " + answersStart);
        Debug.Log("Feedback end at index " + feedbackStart);
        Debug.Log("Scores start at index " + scoresStart);
        Debug.Log("Pros start at index " + prosStart);
        Debug.Log("Cons start at index " + consStart);
        */
        int currentCandidate = 1;

        // Goes through each candidate
        for(int i = 0; i < 3; i++)
        {
            Interviewee candidate;
            string[] answers = new string[questionsLen];
            string[] feedback = new string[questionsLen];
            int[] scores = new int[questionsLen];
            string pros, cons;

            int index = 0;

            // Save each answers from candidate i
            for(int j = answersStart; j < answersStart + questionsLen; j++)
            {
                answers[index] = fileInfo[j];
                index++;
            }

            index = 0;

            // Save each feedback sentence from candidate i
            for(int j = feedbackStart; j < feedbackStart + questionsLen; j++)
            {
                feedback[index] = fileInfo[j];
                index++;
            }

            index = 0; 
            
            // Save each score for candidate i.
            for(int j = scoresStart; j < scoresStart + questionsLen; j++)
            {
                scores[index] = Int32.Parse(fileInfo[j]);
                index++;
            }

            pros = fileInfo[prosStart + i];
            cons = fileInfo[consStart + i];

            candidate = new Interviewee(questionsLen, answers, feedback, scores, pros, cons);

            candidates[i] = candidate;

            // Go to next candidate's answer, feedback, and scores list.
            answersStart += questionsLen;
            feedbackStart += questionsLen;
            scoresStart += questionsLen;
        }// end for loop

    }// end OrganizeCandidates

    // Sends information to each candidate.
    private void SendCandidateInfo()
    {
        for(int i = 0; i < 3; i++)
        {
            candidateObjects[i].SendMessage("ReceiveAnswers", candidates[i].GetAnswers());
            candidateObjects[i].SendMessage("ReceiveFeedback", candidates[i].GetFeedback());
            candidateObjects[i].SendMessage("ReceiveScores", candidates[i].GetScores());
            candidateObjects[i].SendMessage("ReceivePros", candidates[i].GetPros());
            candidateObjects[i].SendMessage("ReceiveCons", candidates[i].GetCons());
        }
    }

    // Sends job information (questions, description, etc.) to needed areas.
    private void SendJobInfo()
    {
        // send over questions to the questionTxt object
        questionsScript.SendMessage("ReceiveQuestions", jobInfoNode.GetQuestions());

        // Set the job panel to active in order to send over the job description.
        jobPanel.SetActive(true);
        jobPanel.SendMessage("ReceiveDescription", jobInfoNode.GetDescription());
        jobPanel.SetActive(false);
    }// end SendJobInfo

    private void SendInfoToInstructionsPage()
    {
        instructionsCanvas.SendMessage("ReceiveQuestions", jobInfoNode.GetQuestions());
        instructionsCanvas.SendMessage("ReceiveSplashPage", jobInfoNode.GetSplash());
        instructionsCanvas.SendMessage("ReceiveTitle", jobInfoNode.GetJobTitle());
        instructionsCanvas.SendMessage("StartInstructions", true);
    }

    private void CopySubArray(int startIndex, int endIndex, string[] originalArray,
                              string[] newArrStr = null, int[] newArrInt = null)
    {
        if(newArrStr != null)
        {
            for(int i = startIndex; i <= endIndex; i++)
            {
                newArrStr[i] = originalArray[i];
            }
        }

        if(newArrInt != null)
        {
            for(int i = startIndex; i <= endIndex; i++)
            {
                newArrInt[i] = Int32.Parse(originalArray[i]);
            }
        }
    }// end CopySubArray

    /////////////////// Helper functions /////////////////////////////////
    static string GetFilePath()
    {
        if (Application.isEditor)
        {
            return Application.dataPath + "/Plugins/TempInterviewFile.txt";
        }
        else
        {
            return Application.persistentDataPath + "/TempInterviewFile.txt";
        }
    }

    string[] GetArrayInfo(string[] fileInfo, int start, int end)
    {
       // Debug.Log("GETARRAYINFO CALLED");
        int len = end - start + 1;

        string[] returnArr = new string[len];

        for (int i = 0; i < len; i++)
        {
            returnArr[i] = fileInfo[start];
           // Debug.Log(returnArr[i]);
            start++;
        }

        return returnArr;
    }// end GetArrayInfo


    /////////////////// Nodes //////////////////////////
    // Node that holds all info about specfific interviewee.
    class Interviewee
    {
        string[] answers;
        string[] feedback;
        int[] scores;
        string pros, cons;

        public Interviewee(int numOfQuestions, string[] newAnswers, string[] newFeedback, int[] newScores, string newPros, string newCons)
        {
            // initializing arrays using the given numOfQuestions
            answers = new string[numOfQuestions];
            feedback = new string[numOfQuestions];
            scores = new int[numOfQuestions];

            // Assigning new values
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
    class JobInfo
    {
        string[] splashPageJobDescript, interviewQuestions, jobDescription;
        string jobTitle;

        public JobInfo(string[] newSplash, string[] newQuestions, string newTitle, string[] newDescript)
        {
            splashPageJobDescript = newSplash;
            interviewQuestions = newQuestions;
            jobTitle = newTitle;
            jobDescription = newDescript;
        }

        public string[] GetSplash()
        {
            return splashPageJobDescript;
        }
        public string[] GetQuestions()
        {
            return interviewQuestions;
        }
        public string GetJobTitle()
        {
            return jobTitle;
        }
        public string[] GetDescription()
        {
            return jobDescription;
        }
    }
}

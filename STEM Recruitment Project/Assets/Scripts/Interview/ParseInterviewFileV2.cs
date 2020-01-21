using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using System;

public class ParseInterviewFileV2 : MonoBehaviour
{
    [SerializeField]
    private GameObject[] candidatesObjects;

    private Interviewee[] candidates;
    // Start is called before the first frame update
    void Start()
    {
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

        // Job info: SPLASH PAGE -> QUESTIONS END
        // Candidate info: ANSWERS -> CONS END
        int jobStart, jobEnd, candidateStart, candidateEnd;

        string jobStartString = "SPLASH_PAGE";
        string jobEndString = "QUESTIONS_END";
        string candidateStartString = "ANSWERS";
        string candidateEndString = "CONS_END";

        for(int i = 0; i < fileInfo.Length; i++)
        {
            fileInfo[i] = fileInfo[i].Replace("\n", string.Empty);
        }

        // THIS IS WHERE I'M HAVING TROUBLE. It's returning true for SPLASH_PAGE, but that's it. 
        // When I tried making my own function (StringEquals, line 93), it returns true only for the last line, 
        // which is nothing. 
        for(int i = 0; i < fileInfo.Length; i++)
        {
            string str = fileInfo[i];
            //str.Trim();
            //Debug.Log(fileInfo[i]);
            if(String.Equals(str, jobStartString, StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log("Job start at index " + i);
                jobStart = i;
            }

            if(str == jobEndString)
            {
                Debug.Log("Job end at index " + i);
                jobEnd = i;
            }

            if(String.Equals(str, candidateStartString, StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log("Candidate info start at index " + i);
                candidateStart = i;
            }

            if(String.Equals(str, candidateEndString, StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log("Candidate info end at index " + i);
                candidateEnd = i;
            }   
        }
    }

    void OrganizeJobInfo(string[] fileInfo, int startIndex, int endIndex)
    {

    }

    void OrganizeCandidates(string[] fileInfo, int startIndex, int endIndex)
    {

    }

    // My own method to use for string comparisons...
    bool StringEqual(string str1, string str2)
    {
        str1.Trim();
        str2.Trim();

        char[] charArr = str1.ToCharArray();
        char[] charArr2 = str2.ToCharArray();

        for(int i = 0; i < charArr.Length; i++)
        {
            if(charArr[i] != charArr2[i])
            {
                Debug.Log(str1 + " IS NOT " + str2);
                return false;
            }
        }
        Debug.Log(str1 + " IS " + str2);
        return true;
    }// end StringEqual

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
        string[] splashPageJobDescript, interviewQuestions;
        string jobTitle, jobDescription;

        public JobInfo(string[] newSplash, string[] newQuestions, string newTitle, string newDescript)
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
        public string GetDescription()
        {
            return jobDescription;
        }
    }
}

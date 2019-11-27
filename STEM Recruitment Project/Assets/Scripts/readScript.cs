using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 

/* Reads the text files and inputs information into arrays to be accessed in
interview game */

public class readScript : MonoBehaviour
{
    public GameObject[] questions, answers, critic; 

    void start ()
    {
        string path = "InterviewGameInfo/";
        
        // pulls number of files from path directory 
        TextAsset[] allJobs = Resources.LoadAll<TextAsset>(path); 

        int numOfJobs = allJobs.Length; // number of files in path 
    }

    /*
    void readTextFile(string file_path)
    {
    StreamReader inp_stm = new StreamReader(file_path);

    while (!inp_stm.EndOfStream)
    {
        string inp_ln = inp_stm.ReadLine();
    }

    inp_stm.Close();
    }*/
}

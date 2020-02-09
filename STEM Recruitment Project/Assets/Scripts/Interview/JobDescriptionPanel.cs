using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobDescriptionPanel : MonoBehaviour
{
    Text jobDescription;

    private void Awake()
    {
        jobDescription = GameObject.Find("Canvas/jobText").GetComponent<Text>();
    }

    public void ReceiveDescription(string[] description)
    {
        jobDescription.text = "";
        for (int i = 0; i < description.Length; i++)
        {
            jobDescription.text += description[i];
        }
    }
}

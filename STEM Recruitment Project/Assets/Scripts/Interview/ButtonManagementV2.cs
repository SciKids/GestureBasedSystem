using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManagementV2 : MonoBehaviour
{
    [SerializeField]
    GameObject[] candidates;

    [SerializeField]
    Button continueButton, resultsButton;


    public void EnableCandidates(bool status)
    {
        for(int i = 0; i < candidates.Length; i++)
        {
            // Get the candidates button
            Button button = candidates[i].transform.Find("Canvas/Button").GetComponent<Button>();

            button.interactable = status;

            // Allows candidate to "answer" question.
            candidates[i].SendMessage("AllowBubble", status);
        }
    }
    
    public void EnableContinueButton(bool status)
    {
        continueButton.interactable = status;
    }

    public void EnableResultsButton(bool status)
    {
        resultsButton.interactable = status;
    }
    
    public void SelectCandidate(GameObject candidate)
    {
        candidate.SendMessage("SelectMe", true);
    }
}

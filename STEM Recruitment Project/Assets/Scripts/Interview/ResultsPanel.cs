using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsPanel : MonoBehaviour
{
    [SerializeField]
    GameObject parseFile, resultsCanvas;

    [SerializeField]
    Text prosConsText;

    string pros, cons;
    
    // Call parsing file for pros & cons on candidate
    public void ChooseCandidate(int id)
    {
        ShowResults();
        parseFile.SendMessage("SendResultsPanelInfo", id);
    }
    
    public void ReceiveName(string name)
    {
        prosConsText.text = "You chose " + name + "! \n\n";
    }

    public void ReceivePros(string newPros)
    {
        pros = newPros;
        prosConsText.text += ("Pros: " + pros + "\n\n");
    }

    public void ReceiveCons(string newCons)
    {
        cons = newCons;
        prosConsText.text += ("Cons: " + cons + "\n");
    }

    private void ShowResults()
    {
        GameObject.Find("ChooseCandidateCanvas").SetActive(false);
        resultsCanvas.SetActive(true);
    }
}

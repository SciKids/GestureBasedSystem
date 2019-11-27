using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayProjectTitle : MonoBehaviour
{
    public TextMeshProUGUI title;
    
    void ReceiveTitle(string newTitle)
    {
        title.text = newTitle;
    }
}

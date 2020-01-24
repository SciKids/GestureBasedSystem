using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Script Summary ////////////////////////////////////////////////////////////
/*
 * Displays project title (ex: Project Headquarters - 
 * Today recruiting for... Smartphone Development)
 */

public class DisplayProjectTitle : MonoBehaviour
{
    public TextMeshProUGUI title;
    
    void ReceiveTitle(string newTitle)
    {
        title.text = newTitle;
    }
}

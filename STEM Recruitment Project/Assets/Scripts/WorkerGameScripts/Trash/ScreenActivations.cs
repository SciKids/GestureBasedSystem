using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenActivations : MonoBehaviour
{
    public GameObject thisScreen, screenOne, screenTwo;
    
    // This script prevents Update() occuring if the camera is not on a certain screen.
    void Start()
    {
        ActivateScreen();
    }

    public void ChangeScreen(GameObject newScreen)
    {
        if(newScreen == screenOne)
        {
            screenOne = thisScreen;
            thisScreen = newScreen;
            ActivateScreen();
        }
        else if(newScreen == screenTwo)
        {
            screenTwo = thisScreen;
            thisScreen = newScreen;
            ActivateScreen();
        }
        else
        {
            Debug.Log("Error in ScreenActivation - reached else block");
        }

    }
    

    void ActivateScreen()
    {
        thisScreen.SetActive(true);
        screenOne.SetActive(false);
        screenTwo.SetActive(false);
    }
}

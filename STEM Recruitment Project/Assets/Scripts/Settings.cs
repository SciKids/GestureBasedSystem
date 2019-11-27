using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Button quitBtn;
    public Slider sensSlider;
    public Text ValText, UserMessage;
    //public Button lowBtn, medBtn, highBtn;
    private float sensVal;
    private float newSensVal;
    // Start is called before the first frame update
    void Start()
    {
        // Deactivate and hide quit button.
        quitBtn.interactable = false;
        quitBtn.gameObject.SetActive(false);

        // Display current sensitivity
        ValText.text = "Sensitivity: " + Math.Round(Mathf.Abs(sensSlider.value - 6), 
            2, MidpointRounding.AwayFromZero);
        //Debug.Log("Sensitivity: " + sensSlider.value);

        // Save previous sensitivity value.
        SensitivitySettings sensSet = GetComponent<SensitivitySettings>();
        sensVal = sensSet.GetSensitivityVal();

        // Adjust slider to current value
        sensSlider.value = sensVal;
    }
    
    void Update()
    {
        ValText.text = "Sensitivity: " + Math.Round(Mathf.Abs(sensSlider.value - 6),
             2, MidpointRounding.AwayFromZero);
        //Debug.Log("Sensitivity: " + sensSlider.value);
    }

    public void UpdateSensitivity()
    {
        newSensVal = sensSlider.value;

        DBManager dbManage = GetComponent<DBManager>();

        if (sensVal != newSensVal)
        {
            // Show quit button.
            quitBtn.interactable = true;
            quitBtn.gameObject.SetActive(true);

            // Write new sensitivity in sensitivity file.
            SensitivitySettings sensSet = GetComponent<SensitivitySettings>();
            sensSet.ChangeSensitivity(newSensVal);

            UserMessage.text = "Settings have changed. Please restart the program.";

            // Add in / edit sensitivity value in database
            if(dbManage.getStatus())
            {
                int userID = dbManage.getID();

                dbManage.editUserSensitivityVal(userID, newSensVal);
            }
            // Restart nuitrack to apply changes.
            //NuitrackManager nuitrack = GetComponent<NuitrackManager>();
            //nuitrack.NuitrackInit();

            // Restart scene
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void increaseSlider()
    {
        sensSlider.value += 0.1f;
    }

    public void decreaseSlider()
    {
        sensSlider.value -= 0.1f;
    }
    // Maybe move to different file? Here temporarily.
    public void QuitGame()
    {
        Application.Quit();
    }

}

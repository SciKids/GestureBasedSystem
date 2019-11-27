using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SensitivitySettings : MonoBehaviour
{
    public Text userMessage = null;

    public void ChangeSensitivity(float val)
    {
        string filePath = GetFilePath();
        
        try
        {
            // WriteAllText creates new file, write empty string to file, and closes file.
            // CHANGE LATER if more info is put into same file.
            File.WriteAllText(filePath, string.Empty);
            
            // Open file to write in
            TextWriter writer = new StreamWriter(filePath);

            // Writes sensitivity value to file
            writer.Write(val);

            // close file
            writer.Close();

            // Print out message to user.
            DisplayMessage("Settings have successfully changed. Please restart the program to apply the changes.");
        }

        // Print error if occurs
        catch(Exception e)
        {
            Debug.Log("Could not read/write settings file. ");
            Debug.Log(e);
            DisplayMessage("Error: Could not change settings.");
        }
        
    } // end ChangeSensitivity
    
    public float GetSensitivityVal()
    {
        DBManager dbManage = GetComponent<DBManager>();
        
        if(dbManage.getStatus())
        {
            int userID = dbManage.getID();

            return dbManage.getUserSensitivity(userID);
        }
        else
        {
            return 3.0f;

            /*
            // Read value from file
            try
            {
                using (TextReader reader = File.OpenText(GetFilePath()))
                {
                    float val = float.Parse(reader.ReadLine());
                    return val;
                }
            }
            // Print out error message if error occurs.
            catch (Exception e)
            {
                Debug.Log("Could not read sensitivity file. Sensitivity set to default");
                Debug.Log(e);
                // Return default value
                return 3f;
            }*/
        }
        
    }

    void DisplayMessage(string message)
    {
        if(userMessage == null)
        {
            return;
        }
        userMessage.text = message;
    }

    string GetFilePath()
    {
        if (Application.isEditor)
        {
            return Application.dataPath + "/Plugins/sensitivity.txt";
        }
        else
        {
            return Application.persistentDataPath + "/sensitivity.txt";
        }
    }

    
}

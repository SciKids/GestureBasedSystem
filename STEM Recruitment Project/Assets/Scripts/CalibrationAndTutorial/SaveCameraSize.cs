using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveCameraSize : MonoBehaviour
{
    public Camera camera;
    public Text userMessage;
    public GameObject rightHand, leftHand;

    private float originalSize;

    private void Start()
    {
        originalSize = camera.orthographicSize;
        Debug.Log("Original size: " + originalSize);
    }

    public void SaveSize()
    {
        string filePath = GetFilePath();

        try
        {
            File.WriteAllText(filePath, string.Empty);

            // Open file to write in
            TextWriter writer = new StreamWriter(filePath);

            // Save camera size to file
            writer.Write(camera.orthographicSize);

            Debug.Log("New size " + camera.orthographicSize);
            // Separate by commas
            writer.Write(",");

            // original_size * scale = new_size
            float scale = camera.orthographicSize / originalSize;

            // Save scale to file
            writer.Write(scale);

            Debug.Log("Scale: " + scale);
           
            // close file
            writer.Close();
            
            float newHandSize = rightHand.transform.localScale.x * scale;

            Debug.Log("New hand size: " + newHandSize);

           // rightHand.transform.localScale = new Vector3(newHandSize, newHandSize, newHandSize);

           // leftHand.transform.localScale = new Vector3(newHandSize, newHandSize, newHandSize);
        }

        catch (Exception e)
        {
            Debug.Log("Could not read/write settings file. ");
            Debug.Log(e);
            userMessage.text = "Error: could not change settings.";
        }


    }
    
    string GetFilePath()
    {
        if (Application.isEditor)
        {
            return Application.dataPath + "/Plugins/camera_size.txt";
        }
        else
        {
            return Application.persistentDataPath + "/camera_size.txt";
        }
    }

}

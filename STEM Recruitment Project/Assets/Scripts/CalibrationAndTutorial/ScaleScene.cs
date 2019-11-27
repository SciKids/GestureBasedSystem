using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ScaleScene : MonoBehaviour
{
    public Camera cam;
    public GameObject rightHand, leftHand, star;
    public GameObject[] otherObjectsNotOnCanvas;
    //public GameObject starAnchor = null;
    public bool StarOnCanvas = true;
    public bool alreadyScaled = false;

    private float[] allOtherSizes;
    private float originalHandSize, originalStarSize, originalCamSize;

    // Start is called before the first frame update
    void Start()
    {
        originalHandSize = rightHand.transform.localScale.x;
        originalCamSize = cam.orthographicSize;

        string filePath = GetFilePath();

        try
        {
            StreamReader reader = File.OpenText(filePath);

            string infoStr = reader.ReadLine(); // Should only be 1 line

            string[] strList = infoStr.Split(','); // Split line by commas

            reader.Close();

            float[] floatList = new float[strList.Length]; // array to hold all values

            for(int i = 0; i < strList.Length; i++)
            {
                floatList[i] = float.Parse(strList[i]); // convert each string into float
            }

            float newHandSize = rightHand.transform.localScale.x * floatList[1];

            cam.orthographicSize = floatList[0];
            
            if(!alreadyScaled)
            {
                rightHand.transform.localScale = new Vector3(newHandSize, newHandSize, newHandSize);

                leftHand.transform.localScale = new Vector3(newHandSize, newHandSize, newHandSize);
            }

            // If the star is on the canvas, it will automatically resize stay in place.
            if(!StarOnCanvas)
            {
                originalStarSize = star.transform.localScale.x;

                Debug.Log("Original star size: " + star.transform.localScale.x);

                float newStarSize = star.transform.localScale.x * floatList[1];

                Debug.Log("New star size: " + newStarSize);

                star.transform.localScale = new Vector3(newStarSize, newStarSize, newStarSize);

                RepositionStar(newStarSize, floatList[0]);
            }
           
        }

        catch(Exception e)
        {
            Debug.Log("Could not read/write settings file. ");
            Debug.Log(e);
        }
    }

    // TEMPORARY - I'm worried that the scale will continuously decrease, so I'm resetting 
    // everything when the scene is destroyed.
    private void OnDestroy()
    {
       /* rightHand.transform.localScale = new Vector3(originalHandSize, originalHandSize, originalHandSize);

        leftHand.transform.localScale = new Vector3(originalHandSize, originalHandSize, originalHandSize);

        if (!StarOnCanvas)
        {
            star.transform.localScale = new Vector3(originalStarSize, originalStarSize, originalStarSize);
        }

        cam.orthographicSize = originalCamSize;*/
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

    void RepositionStar(float newScale, float camSize)
    {
        /*float height = 2f * camSize;
        float width = height * cam.aspect;

        Debug.Log("Width: " + width +", height: " + height);

        Vector3 newPos = new Vector3(0, 0, star.transform.position.z);

        //newPos.x = Screen.width / 2;
        Debug.Log(newPos);
        Debug.Log("Screen width: " + Screen.width + ", Screen height: " + Screen.height);
        Debug.Log("Upper left: " + cam.ScreenToWorldPoint(new Vector3(0, Screen.height, star.transform.position.z)));
        //star.transform.position = newPos;
        */

        float screenAspect = (float)Screen.width / (float)Screen.height;
        float camHalfHeight = camSize;
        Debug.Log("Orthographic size: " + camSize);
        float camHalfWidth = screenAspect * camHalfHeight;
        float camWidth = 2.0f * camHalfWidth;
        float camHeight = camHalfHeight * 2.0f;

        Debug.Log("Cam width " + camWidth + ", cam height: " + camHeight);
        Debug.Log("CamHalfWidth " + camHalfWidth + " CamHalfHeight: " + camHalfHeight);

        Vector3 newPos = new Vector3(-(camHalfWidth - camSize), camHalfHeight, star.transform.position.z);
        star.transform.position = newPos;
        //Vector3 newPos = starAnchor.transform.position;
    }
}

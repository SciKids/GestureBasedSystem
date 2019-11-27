using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScreenV2 : MonoBehaviour
{
    public Camera cam;

    private Vector3 screen1, screen2;
    private float min, max;
    private float t = 0.0f;
    private bool okToChange = false;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        if(okToChange)
        {
            // animate the position of the game object...
            cam.transform.position = new Vector3(Mathf.Lerp(min, max, t), screen1.y, screen1.z);

            // .. and increase the t interpolater
            t += 0.5f * Time.deltaTime;

            if (t > 1.0f)
            {
                t = 0.0f;

                okToChange = false;

               // Debug.Log("Camera changed to: " + cam.transform.position);
            }
            
        }
        
    }

    // Changes the value of screen2.
    public void MoveScreen(float screen2x)
    {
        screen1 = cam.transform.position;

        screen2 = cam.transform.position;

        //Debug.Log("Screen1: " + screen1);

        screen2.x = screen2x;

        //Debug.Log("Screen2 changed to: " + screen2);

        min = screen1.x;

        max = screen2x;

        okToChange = true;
        
    }

    public void SetReadyToLoad(GameObject loadingScreen)
    {
        loadingScreen.SendMessage("ReadyToLoad", true);
    }


}

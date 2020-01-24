using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script Summary ////////////////////////////////////////////////////////////
/*
 * Plays the loading circle, which is activated when the next button is clicked on
 * the worker screen.
 * NOTE: This is from a downloaded asset. See Assets/Downloaded Assets/loadingBar/scripts
 */

public class LoadingScreenV3 : MonoBehaviour
{
    // Speed colors load in color wheel.
    public float speed = 0.0f;
    
    // For the color wheel
    private RectTransform rectComponent;
    private Image imageComp;
    

    // Use this for initialization
    void Start()
    {
        // Stuff for color wheel
        rectComponent = GetComponent<RectTransform>();
        imageComp = rectComponent.GetComponent<Image>();
        imageComp.fillAmount = 0.0f;
        
    }

    void Update()
    {
        // If the loading circle is activated, start the animation.
        if (GameObject.Find("/WorkerCanvas/LoadingCanvas").activeSelf)
        {
            StartCoroutine(GoBack());

            // Fills up the circle
            if (imageComp.fillAmount != 1f)
            {
                imageComp.fillAmount = imageComp.fillAmount + Time.deltaTime * speed;

            }

            else
            {
                imageComp.fillAmount = 0.0f;

            }
        }
    }// end Update

    // Sends message to the MoveScreenV2 script to move screen back to worker screen.
    IEnumerator GoBack()
    {
        yield return new WaitForSeconds(5);

        GameObject.Find("/WorkerCanvas/WorkerScreen").SetActive(true);
        GameObject.Find("/WorkerCanvas/LoadingCanvas").SetActive(false);

    }// end GoBack
    
}

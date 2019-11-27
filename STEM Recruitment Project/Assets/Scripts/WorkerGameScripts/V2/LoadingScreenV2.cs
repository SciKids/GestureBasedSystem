using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenV2 : MonoBehaviour
{
    // Speed colors load in color wheel.
    public float speed = 0.0f;
    
    // Variables needed in editor to move screen from loading screen to 
    // worker screen
    public float workerScreenX;
    public Camera camera;
    public GameObject loadingScreen;

    // For the color wheel
    private RectTransform rectComponent;
    private Image imageComp;

    // For the camera movement
    private float min, max;
    private float t = 0.0f;
    private float y, z;
    private bool readyToLoad;

    // Use this for initialization
    void Start()
    {
        // Stuff for color wheel
        rectComponent = GetComponent<RectTransform>();
        imageComp = rectComponent.GetComponent<Image>();
        imageComp.fillAmount = 0.0f;
        readyToLoad = false;

        // y and z positions always stay the same, since the camera only
        // moves its x position.
        y = camera.transform.position.y;
        z = camera.transform.position.z;
    }
    
    void Update()
    {
        if(readyToLoad)
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
    }

    // Sends message to the MoveScreenV2 script to move screen back to worker screen.
    IEnumerator GoBack()
    {
        yield return new WaitForSeconds(5);

        loadingScreen.SendMessage("MoveScreen", workerScreenX);

        readyToLoad = false; // Stop the wheel.

    }
    
    public IEnumerator ReadyToLoad(bool status)
    {
        yield return new WaitForSeconds(2.0f);

        readyToLoad = status;
    }
}

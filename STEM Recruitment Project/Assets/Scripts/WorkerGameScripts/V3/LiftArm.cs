using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script Summary ////////////////////////////////////////////////////////////
/*
 * Disables down arm image and enables up arm image if a user hand has entered
 * the collision box.
 */
public class LiftArm : MonoBehaviour
{
    public string handCanLift;

    private GameObject up, down;
    private bool okToLift = true;
    
    // Find the up and down arm images, and disable the up image.
    void Start()
    {
        up = this.gameObject.transform.Find("Up").gameObject;
        down = this.gameObject.transform.Find("Down").gameObject;

        up.SetActive(false);
    }// end start
    
    // Move arm up if user hand enters collision box.
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Hand" && okToLift && other.name == handCanLift)
        {
            down.SetActive(false);
            up.SetActive(true);
        }
    }// end OnTriggerStay
    
    // Blocks the up arm image from being enabled. This is called on by other classes.
    public void SetOkToLift(bool status)
    {
       // Debug.Log("Messaged received: " + status);
        okToLift = status;
    }
    
}

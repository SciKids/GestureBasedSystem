using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HalfStarCollider2D : MonoBehaviour
{// Script Summary ////////////////////////////////////////////////////////////
 /*
  * This is attached to each half of the selection star. If a user hand collides, 
  * the image is enabled. 
  */
    private SpriteRenderer halfStarImage;

    // Disables the half star image
    void Start()
    {
        //this.enabled = false;
        halfStarImage = GetComponent<SpriteRenderer>();
        halfStarImage.enabled = false;

    }// end Start

    // If a user hand collides with the half-star, enable the image
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hand")
        {
            //Debug.Log("Hand collided");
            halfStarImage.enabled = true;
        }
    }// end OnTriggerEnter
    
}// end HalfStarCollider2D

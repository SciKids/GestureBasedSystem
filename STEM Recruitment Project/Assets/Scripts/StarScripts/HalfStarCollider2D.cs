using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HalfStarCollider2D : MonoBehaviour
{
    private SpriteRenderer halfStarImage;
    
    // Disable image
    void Start()
    {
        halfStarImage = GetComponent<SpriteRenderer>();
        halfStarImage.enabled = false;

    }

    // If left hand collides with left half, or if right hand collides with right half, set image to true.
    private void OnTriggerEnter(Collider other)
    {
        if(this.gameObject.name == "LeftHalfStar" && other.gameObject.name == "LeftHand")
        {
            halfStarImage.enabled = true;
        }
        if(this.gameObject.name == "RightHalfStar" && other.gameObject.name == "RightHand")
        {
            halfStarImage.enabled = true;
        }
    }
    
}

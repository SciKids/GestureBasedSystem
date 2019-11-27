using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HalfStarCollider2D : MonoBehaviour
{
    private SpriteRenderer halfStarImage;

    // Start is called before the first frame update
    void Start()
    {
        //this.enabled = false;
        halfStarImage = GetComponent<SpriteRenderer>();
        halfStarImage.enabled = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hand")
        {
            //Debug.Log("Hand collided");
            halfStarImage.enabled = true;
        }
    }
    
}

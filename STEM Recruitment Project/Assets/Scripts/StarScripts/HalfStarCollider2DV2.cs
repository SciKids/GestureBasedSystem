using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfStarCollider2DV2 : MonoBehaviour
{
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hand")
        {
            sprite.enabled = true;
        }
    }
}

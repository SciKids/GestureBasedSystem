using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInsideBorder : MonoBehaviour
{
    void Update()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        //this.transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

}

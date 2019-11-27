using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class dropHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform playerBtn = transform as RectTransform;  

        if (!RectTransformUtility.RectangleContainsScreenPoint(playerBtn, Input.mousePosition))
        {
            Debug.Log("DropItem"); 
        }
    }
}

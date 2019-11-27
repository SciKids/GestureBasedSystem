using UnityEngine;
using System.Collections;

public class CustomCursor : MonoBehaviour
{
    private float zPos = 10f;
    //private Vector3 mousePosition;

    // Use this for initialization
    void Start()
    {
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = Input.mousePosition;
        
        temp.z = zPos;

        Debug.Log(temp);

        this.transform.position = Input.mousePosition;
    }
}
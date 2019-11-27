using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackToInitPos : MonoBehaviour
{
    private Vector3 initPos;

    // Start is called before the first frame update
    void Start()
    {
        initPos = this.transform.position;    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Floor")
        {
            Debug.Log("MoveBackToInitPos - Returning to initial position");
            this.transform.position = initPos;
        }
    }
}

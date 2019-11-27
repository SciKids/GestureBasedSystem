using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayWorkerTitle : MonoBehaviour
{
    public Text title;
    public GameObject feedbackPic;
    // Start is called before the first frame update
    void Start()
    {
        title.enabled = false;
        feedbackPic.SetActive(false);
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Hand")
        {
            title.enabled = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Hand")
        {
            title.enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftArm : MonoBehaviour
{
    public string handCanLift;

    private GameObject up, down;
    private bool okToLift = true;
    
    // Start is called before the first frame update
    void Start()
    {
        up = this.gameObject.transform.Find("Up").gameObject;
        down = this.gameObject.transform.Find("Down").gameObject;

        up.SetActive(false);
    }
    // Move arm up.
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Hand" && okToLift && other.name == handCanLift)
        {
            down.SetActive(false);
            up.SetActive(true);
        }
    }
    
    public void SetOkToLift(bool status)
    {
       // Debug.Log("Messaged received: " + status);
        okToLift = status;
    }
    
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandles : MonoBehaviour
{
    //public Material greenMaterial, blackMaterial;
    //private Material currentMaterial, otherMat;
    // public GameObject otherSeg;

    public GameObject otherBar;
    private GameObject thisSeg1, thisSeg2, otherSeg1, otherSeg2, knob1, knob2;
    private Material thisMat1, thisMat2, otherMat1, otherMat2, knobMat1, knobMat2;

    public void Start()
    {
        // Get main bars
        thisSeg1 = this.transform.Find("Seg1").gameObject;
        thisSeg2 = this.transform.Find("Seg2").gameObject;
        otherSeg1 = otherBar.transform.Find("Seg1").gameObject;
        otherSeg2 = otherBar.transform.Find("Seg2").gameObject;

        // Get materials for main bars
        thisMat1 = thisSeg1.GetComponent<Renderer>().material;
        thisMat2 = thisSeg2.GetComponent<Renderer>().material;
        otherMat1 = otherSeg1.GetComponent<Renderer>().material;
        otherMat2 = otherSeg2.GetComponent<Renderer>().material;

        // Turn main bars to black
        thisMat1.color = Color.black;
        thisMat2.color = Color.black;

        //this.currentMaterial = GetComponent<Renderer>().material;
        // this.currentMaterial.color = Color.black;

        //otherMat = otherSeg.GetComponent<Renderer>().material;
        //otherMat.color = Color.black;

        // Get knob objects, materials, and turn to black.
        knob1 = thisSeg2.transform.Find("Knob1").gameObject;
        knob2 = thisSeg2.transform.Find("Knob2").gameObject;
        knobMat1 = knob1.GetComponent<Renderer>().material;
        knobMat2 = knob2.GetComponent<Renderer>().material;
        knobMat1.color = Color.black;
        knobMat2.color = Color.black;


    }
    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Hand")
        {
            thisMat1.color = Color.green;
            thisMat2.color = Color.green;
            knobMat1.color = Color.green;
            knobMat2.color = Color.green;
            //this.currentMaterial.color = Color.green;
            //otherMat.color = Color.green;
        }

    }

    public void OnCollisionExit(Collision collision)
    {
        /*if(otherMat1.color == Color.green)
        {
            otherMat1.color = Color.black;
            otherMat2.color = Color.black;
        }
        */
        /*if (otherMat1.color == Color.black)
        {
            thisMat1.color = Color.black;
            thisMat2.color = Color.black;
        }*/
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfStarCollider : MonoBehaviour
{
    private List<GameObject> segments;
    // Start is called before the first frame update
    void Start()
    {
        segments = new List<GameObject>();

        FillList();
        TurnAllToBlack();
    }

    
    void FillList()
    {
        //b1seg1 = bar1.transform.Find("Seg1").gameObject;
        //There are 5 segments to a half of the star.
        for (int i = 1; i <= 5; i++)
        {
            string segName = "Capsule" + i.ToString();

            GameObject currentSeg = this.transform.Find(segName).gameObject;

            segments.Add(currentSeg);
        }
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Hand")
        {
            TurnAllToGreen();
        }
    }

    void TurnAllToBlack()
    {
        for(int i = 0; i < 5; i++)
        {
            Material currentMat = segments[i].GetComponent<Renderer>().material;
            currentMat.color = Color.black;
        }
    }

    void TurnAllToGreen()
    {
        for (int i = 0; i < 5; i++)
        {
            Material currentMat = segments[i].GetComponent<Renderer>().material;
            currentMat.color = Color.green;
        }
    }
}

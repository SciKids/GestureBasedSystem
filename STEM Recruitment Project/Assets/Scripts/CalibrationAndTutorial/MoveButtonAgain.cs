using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButtonAgain : MonoBehaviour
{
    public GameObject canvas;
    private int canvasWidth, canvasHeight;

    private void Start()
    {
        canvasWidth = (int)canvas.GetComponent<RectTransform>().rect.width / 2;
        canvasHeight = (int)canvas.GetComponent<RectTransform>().rect.height / 2;

    }
    private void OnTriggerEnter(Collider other)
    {
        // If the button spawned on another button, re-randomize its position.
        if(other.gameObject.tag == "Button")
        {
            System.Random rand = new System.Random();

            int newX = rand.Next(-(canvasWidth - 20), canvasWidth - 20);
            int newY = rand.Next(-(canvasHeight - 20), canvasHeight - 20);

            Debug.Log("New position: (" + newX + "," + newY + ")");
            this.transform.localPosition = new Vector3(newX, newY, this.transform.localPosition.z);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddToScore : MonoBehaviour
{
    //public TextMeshPro feedback;
    public Text scoreText;
    public GameObject picture; // outline's corresponding picture
   
    // Other needed values.
    private int score;
    private Collider picCollider;

    // Start is called before the first frame update
    void Start()
    {
        score = int.Parse(scoreText.text);

        picCollider = picture.GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COLLISION ENTER");
        if(collision.collider == picCollider)
        {
            score += 5;
            scoreText.text = score.ToString();
        }
    }

}


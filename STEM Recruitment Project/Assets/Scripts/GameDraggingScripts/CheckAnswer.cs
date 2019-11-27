using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckAnswer : MonoBehaviour
{
    public GameObject answer;
    public TextMeshPro feedback;
    public Text scoreText;
    private int score;
    private bool isFirstTime;
    private Vector3 answerPos;
    private RigidbodyConstraints constraints;
    private Material outlineMaterial;

    // Start is called before the first frame update
    void Start()
    {
        answerPos = answer.transform.position;
        score = 0;
        scoreText.text = "0";
        isFirstTime = true;
        outlineMaterial = answer.GetComponent<Renderer>().material;

        //constraints = this.GetComponent<RigidbodyConstraints>();
    }
    // New: 462, 57 || 389, 132
    // OG: 462, 132
    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = this.transform.position;

        score = int.Parse(scoreText.text);

        // If the current is close enough to the answer
        if(currentPos.x <= answerPos.x + 80 || currentPos.x <= answerPos.x - 80)
        {
            if(currentPos.y < answerPos.y + 80 || currentPos.y <= answerPos.y - 80)
            {
                if (isFirstTime)
                {
                    score += 5;
                    scoreText.text = score.ToString();
                    isFirstTime = false;
                }
                // Change outline to green
                outlineMaterial.color = Color.green;

                // Set current and answer to the same position.
                this.transform.position = answerPos;
                
                // Freeze current position
                constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;

                // Set answer's outline to green.
                answer.GetComponent<Outline>().effectColor = Color.green;
                
                //Debug.Log("Current: " + currentPos + ", Answer: " + answerPos);
            }
        }
    }
}

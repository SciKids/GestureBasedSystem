using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestGameScript : MonoBehaviour
{
    public Button button;
    public int timeToClick;
    public Text feedbackText, timerText, feedback2Text;
    public int scoreVal = 5;

    private int canvasWidth, canvasHeight;
    private bool buttonClicked = false;
    private bool runTimer = true;
    private int score;
    private int timer;

    // Copies of coroutines needed throughout script.
    private IEnumerator timerCoroutine, clickCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        System.Random rand = new System.Random();

        canvasWidth = (int) this.GetComponent<RectTransform>().rect.width / 2;
        canvasHeight = (int) this.GetComponent<RectTransform>().rect.height / 2;
        
        int newX = rand.Next(-(canvasWidth - 20), canvasWidth - 20);
        int newY = rand.Next(-(canvasHeight - 20), canvasHeight - 20);

        button.transform.localPosition = new Vector3(newX, newY, button.transform.localPosition.z);

        timer = timeToClick;
    }

    private void Update()
    {
        if(runTimer)
        {
            runTimer = false; // block this from rerunning. Unblocked when DetermineClick() is completed.

            clickCoroutine = DetermineClick();
            timerCoroutine = ShowTimer();

            StartCoroutine(clickCoroutine);
            StartCoroutine(timerCoroutine);
        }
    }

    void MoveButton() // Parameter is for send message
    {
        System.Random rand = new System.Random();

        int newX = rand.Next(-(canvasWidth - 20), canvasWidth - 20);
        int newY = rand.Next(-(canvasHeight - 20), canvasHeight - 20);

        Debug.Log("New position: (" + newX + "," + newY + ")");
        button.transform.localPosition = new Vector3(newX, newY, button.transform.localPosition.z);
    }

    // If the button is clicked, can
    public void ClickButton()
    {
        buttonClicked = true;

        StopCoroutine(clickCoroutine);
        StopCoroutine(timerCoroutine);

        score += scoreVal;

        MoveButton();

        feedbackText.text = "Score: " + score.ToString();

        if(score % 10 == 0 && score != 0 && timer != 2)
        {
            feedback2Text.text = "Faster!";
            timer--;
        }
        else
        {
            feedback2Text.text = "";
        }

        runTimer = true;
        
    }

    // After a certain amount of time, move button if not clicked.
    // This coroutine will be cancelled if button is clicked
    IEnumerator DetermineClick()
    {
        yield return new WaitForSeconds(timer);

        feedback2Text.text = "";

        score -= scoreVal;

        timer = timeToClick;

        feedback2Text.text = "Timer reset to " + timer.ToString() + " seconds!";

        feedbackText.text = "Score: " + score.ToString();

        MoveButton();

        runTimer = true; // Rerun timer.
    }

    IEnumerator ShowTimer()
    {
        int i = timer;

        while(i >= 0)
        {
            timerText.text = "Time Left: " + i.ToString();

            yield return new WaitForSeconds(1);

            i--;
        }
    }
}

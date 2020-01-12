using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script Summary ////////////////////////////////////////////////////////////
/*
 * Attached to worker objects. If a user hand hovers over a worker, show the 
 * worker's title and desecription
 */

public class ShowTitleAndDescriptionV2 : MonoBehaviour
{
    public Text feedback;
    public GameObject feedbackPic;
    public Text title;

    private string workerDescription;
    private string workerFeedback;
    private int timesHovered = 0;
    private string textToShow;
    private GameObject speechbubble;
    private bool oneIsWrong = false;
    private bool setTextOnce = false;
    private bool descriptionIsShowing = false;
    private bool workersWereChecked = false;
    
    // hide feedbackpic, title, & speechbubble, assign the textToShow to workerDescription
    void Start()
    {
        //title = this.GetComponentInChildren<Text>();
        textToShow = workerDescription;
        feedbackPic.SetActive(false);
        title.enabled = false;
        speechbubble = GameObject.Find("/WorkerCanvas/WorkerScreen/Speechbubble");
        speechbubble.SetActive(false);

    }// end Start

    // Enables certain messages
    private void Update()
    {
        // Making sure timesHovered is 0 if the worker title is disabled.
        if (title.enabled == false)
        {
            timesHovered = 0;
        }

        // Shows feedback after evaluation. Feedback is dependent on if the user has at least one
        // wrong answer present.
        if (setTextOnce)
        {
            // If at least one of the workers are wrong, show the "Oops!..." feedback
            if (oneIsWrong)
            {
                feedback.text = "Oops! Looks like at least one worker here doesn't belong. Hover over" +
                    " each worker to see why, and press the reset button when you're ready to try again.";
            }

            // Otherwise, show the "Yay!..." feedback
            else
            {
                feedback.text = "Yay! Everyone here plays an important role in this project. Click the X to " +
                    "close out of the game.";
            }

            setTextOnce = false;

        }// end if setTextOnce

        if (descriptionIsShowing)
        {
            feedback.text = "";

        }

    }// end Update

    // If a user hand collides with the worker, show the worker's title first. Afterwards, show the worker's
    // description.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hand")
        {
            // The first time a player hovers over a worker, only the worker's title
            // appears. Title is permanently enabled afterwards.
            if (timesHovered == 0)
            {
                // I want to shortly delay the title. This is so that titles don't pop on and off while the
                // the user is moving their hands around.
                StartCoroutine(DelayTitle());
            }

            // If the title is already enabled, show the worker description.
            if (timesHovered == 1)
            {
                // If the right/wrong pictures are active, the worker has been evaluated so the feedback can be shown.
                if (feedbackPic.activeSelf)
                {
                    textToShow = workerFeedback;
                }

                // Otherwise, just show the description
                else
                {
                    textToShow = workerDescription;
                }

                // I want to shortly delay the feedback, so it's not popping in and out while the user is moving their
                // hands around.
                StartCoroutine(DelayFeedback());
            }
        }// end if other==hand

    }// end OnTriggerEnter

    // If a hand leaves the worker, hide the worker's description. If the user has been evaluated, show the 
    // original feedbakc,
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Hand")
        {
            StopAllCoroutines();
            // Worker description disappears when hand leaves worker.
            if (timesHovered == 1)
            {
                speechbubble.SetActive(false);

                // If the user has been evaluated, show the evaluation's feedback text.
                if (workersWereChecked)
                {
                    if (oneIsWrong)
                    {
                        feedback.text = "Oops! Looks like at least one worker here doesn't belong. Hover over" +
                                        " each worker to see why, and press the reset button when you're ready to try again.";
                    }

                    else
                    {
                        feedback.text = "Yay! Everyone here plays an important role in this project. Click the X to " +
                                        "close out of the game.";
                    }
                }// end if workersWereChecked

                // Otherwise, don't show anything
                else
                {
                    feedback.text = "";
                }

            }// end if timesHovered==1

        }// end if other==hand

    }// end OnTriggerExit

    // If the user has been evaluated, change the worker's description to the worker's feedback.
    public void ChangeFeedback(bool readyToCheck)
    {
        if (readyToCheck)
        {
            textToShow = workerFeedback;
        }
        else
        {
            textToShow = workerDescription;
        }

    }// end ChangeFeedback

    // Delays the worker title for half a second. This is to prevent titles popping up quickly 
    // if the user moves their hands around without intending to show the title.
    IEnumerator DelayTitle()
    {
        yield return new WaitForSeconds(0.5f);

        title.enabled = true;

        timesHovered++;
    }// end DelayTitle

    // Delays the worker description for half a second. This is to prevent text popping in 
    // quickly if the user moves their hands without intending to show the description.
    IEnumerator DelayFeedback()
    {
        yield return new WaitForSeconds(0.5f);

        feedback.text = textToShow;

        speechbubble.SetActive(true);
    }// end DelayFeedback

    ///// Next 4 functions receive values from LoadGameInfo, which reads Project#.txt ///////////
    public void ReceiveTitle(string newTitle)
    {
        title.text = newTitle;
    }// end ReceiveTitle

    public void ReceiveDescription(string newDescript)
    {
        workerDescription = newDescript;
    }// end ReceiveDescription

    public void ReceiveFeedback(string newFB)
    {
        workerFeedback = newFB;
    }// end ReceiveFeedback

    public void ReceiveStatus(bool isCorrect)
    {
        if (isCorrect)
        {
            feedbackPic.GetComponent<SpriteRenderer>().sprite = Resources.Load("Images/WorkerGameImages/Right", typeof(Sprite)) as Sprite;
        }
        else
        {
            feedbackPic.GetComponent<SpriteRenderer>().sprite = Resources.Load("Images/WorkerGameImages/Wrong", typeof(Sprite)) as Sprite;
        }
    }// end ReceiveStatus0
    //////////////// end receiver functions for LoadGameInfo /////////////////////////////////
    
    //////////////// Functions gets called from CheckWorkersV2 //////////////////////////////
    public void ReceiveCorrectness(bool status)
    {
        oneIsWrong = !status;
    }// end ReceiveCorrectness

    public void ShowOverallFeedback(bool status)
    {
        if (feedbackPic.activeSelf)
        {
            setTextOnce = status;
        }
    }// end ShowOverallFeedback

    public void SetWorkersAreChecked(bool status)
    {
        workersWereChecked = status;
    }// end SetWorkersAreChecked

    // Function gets called from ShowJobDescription
    public void ReceiveDescriptBubbleStatus(bool status)
    {
        descriptionIsShowing = status;
    }// end ReceiveDescriptBubbleStatus
    /////////////// End functions called from CheckWorkersV2 ////////////////////////////////
    
}// end ShowTitleAndDescriptionV2

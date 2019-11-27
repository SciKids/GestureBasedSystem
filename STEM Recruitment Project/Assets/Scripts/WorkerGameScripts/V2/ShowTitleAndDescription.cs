using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTitleAndDescription : MonoBehaviour
{
    public Text feedback;
    public GameObject feedbackPic;
    public Text title;

    private string workerDescription;
    private string workerFeedback;
   
    private int timesHovered = 0;
   // private Text title;
    private string textToShow;
    private GameObject speechbubble;
    //private bool drawLine = false;
    private bool oneIsWrong = false;
    private bool setTextOnce = false;
    private bool descriptionIsShowing = false;
    private bool workersWereChecked = false;
   // private bool resetPressed = false;
    // Start is called before the first frame update
    void Start()
    {
        //title = this.GetComponentInChildren<Text>();
        textToShow = workerDescription;
        feedbackPic.SetActive(false);
        title.enabled = false;
        speechbubble = GameObject.Find("/WholeGame/WorkerScreen/Speechbubble");
        speechbubble.SetActive(false);
        
    }

    private void Update()
    {
        if(title.enabled == false)
        {
            timesHovered = 0;
        }
        
        if(setTextOnce)
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

            setTextOnce = false;
        }

        if(descriptionIsShowing)
        {
            feedback.text = "";
            
        }
        
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hand")
        {
            // The first time a player hovers over a worker, only the worker's title
            // appears. Title is permanently enabled afterwards.
            if(timesHovered == 0)
            {
                StartCoroutine(DelayTitle());
            }

            // If the title is already enabled, show the worker description.
            if(timesHovered == 1)
            {
                // If the right/wrong pictures are active, the worker has been evaluated so the feedback can be shown.
                if(feedbackPic.activeSelf)
                {
                    textToShow = workerFeedback;
                }

                // Otherwise, just show the description
                else
                {
                    textToShow = workerDescription;
                }

                StartCoroutine(DelayFeedback());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Hand")
        {
            StopAllCoroutines();
            // Worker description disappears when hand leaves worker.
            if (timesHovered == 1)
            {
                speechbubble.SetActive(false);
                
                if(workersWereChecked)
                {
                    if(oneIsWrong)
                    {
                        feedback.text = "Oops! Looks like at least one worker here doesn't belong. Hover over" +
                            " each worker to see why, and press the reset button when you're ready to try again.";
                    }

                    else
                    {
                        feedback.text = "Yay! Everyone here plays an important role in this project. Click the X to " +
                    "close out of the game.";
                    }
                }
                else
                {
                    feedback.text = "";
                }

            }

        }
    }
    public void ChangeFeedback(bool readyToCheck)
    {
        if(readyToCheck)
        {
            textToShow = workerFeedback;
        }
        else
        {
            textToShow = workerDescription;
        }
    }

    IEnumerator DelayTitle()
    {
        yield return new WaitForSeconds(0.5f);

        title.enabled = true;

        timesHovered++;
    }

    IEnumerator DelayFeedback()
    {
        yield return new WaitForSeconds(0.5f);

        feedback.text = textToShow;

        speechbubble.SetActive(true);
    }
    
    // Next 4 functions receive values from LoadGameInfo, which reads Project#.txt
    public void ReceiveTitle(string newTitle)
    {
        title.text = newTitle;
    }
    public void ReceiveDescription(string newDescript)
    {
        workerDescription = newDescript;
    }

    public void ReceiveFeedback(string newFB)
    {
        workerFeedback = newFB;
    }

    public void ReceiveStatus(bool isCorrect)
    {
        if(isCorrect)
        {
            feedbackPic.GetComponent<SpriteRenderer>().sprite = Resources.Load("Images/WorkerGameImages/Right", typeof(Sprite)) as Sprite;
        }
        else
        {
            feedbackPic.GetComponent<SpriteRenderer>().sprite = Resources.Load("Images/WorkerGameImages/Wrong", typeof(Sprite)) as Sprite;
        }
    }

    // Functions gets called from CheckWorkersV2
    public void ReceiveCorrectness(bool status)
    {
        oneIsWrong = !status;
    }
    public void ShowOverallFeedback(bool status)
    {
        if(feedbackPic.activeSelf)
        {
            setTextOnce = status;
        }
    }
    public void SetWorkersAreChecked(bool status)
    {
        workersWereChecked = status;
    }

    // Function gets called from ShowJobDescription
    public void ReceiveDescriptBubbleStatus(bool status)
    {
        descriptionIsShowing = status;
    }
    
}

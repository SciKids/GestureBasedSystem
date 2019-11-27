using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDialog : MonoBehaviour
{
    public Text dialog;
    public float typingSpeed;
    public string[] sentences;
//private GameObject nextButton;
    private int index = 0;
    private bool isFirstTime;
    private Text timeCounter;
    private SphereCollider collider;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        isFirstTime = true;
        collider = this.GetComponent<SphereCollider>();
        sprite = this.GetComponent<SpriteRenderer>();

        //this.gameObject.SetActive(false);
        StartCoroutine(Type());
        timeCounter = this.transform.Find("CounterText").gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActiveAndEnabled)
        {
            Debug.Log("NOT ACTIVE");
        }
        // If we reach the end of a sentence, show the next button.
        if (dialog.text == sentences[index])
        {
            if(index != sentences.Length - 1)
            {
                ShowButton();
            }
            else
            {
                HideButton();
            }
        }
        else
        {
            HideButton();
        }

        if(!isFirstTime)
        {
            dialog.text = "Did you figure it out?";
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Hand")
        {
            StartCoroutine(WaitForNextSentence());
            StartCoroutine(ShowTime());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Hand")
        {
            StopAllCoroutines();
            timeCounter.text = "";
        }
    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            dialog.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
       // this.gameObject.SetActive(true);
        if (index < sentences.Length - 1)
        {
            index++;
            dialog.text = "";
            StartCoroutine(Type());
        }

        else
        {
            dialog.text = "";
        }
    }

    void HideButton()
    {
        collider.isTrigger = false;
        sprite.enabled = false;
    }

    void ShowButton()
    {
        collider.isTrigger = true;
        sprite.enabled = true;
    }

    IEnumerator ShowTime()
    {
        int timeLeft = 3;
        while (timeLeft >= 0)
        {
            Debug.Log(timeLeft);

            timeCounter.text = timeLeft.ToString();

            yield return new WaitForSeconds(1);

            timeLeft--;
        }
        timeCounter.text = "";
    }

    IEnumerator WaitForNextSentence()
    {
        yield return new WaitForSeconds(3);
        NextSentence();
    }
  
}
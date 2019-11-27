using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour
{
    public int timeToClick = 3;
    public bool hoverToClick = false;
    private bool okToClick = true;
    private string originalText;

    // This script goes on both the right and left hands. Allows the hands to
    // determine if they just collided with a button or not.

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Button" && hoverToClick)
        {
            Button button = other.GetComponent<Button>();
            Text buttonText = button.GetComponentInChildren<Text>();
            
            originalText = buttonText.text;

            StartCoroutine(Click(button));

            StartCoroutine(ShowTime(buttonText));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Button" && hoverToClick)
        {
            StopAllCoroutines();
            
            Button button = other.GetComponent<Button>();
            Text buttonText = button.GetComponentInChildren<Text>();

            buttonText.text = originalText;
        }
        
    }

    IEnumerator Click(Button button)
    {
        yield return new WaitForSeconds(timeToClick);
        
        button.onClick.Invoke();
            
    }

    IEnumerator ShowTime(Text buttonText)
    {
        int timeLeft = timeToClick;

        while (timeLeft >= 0)
        {
            buttonText.text = originalText + " (" + timeLeft.ToString() + ")";

            yield return new WaitForSeconds(1);

            timeLeft--;
        }

        buttonText.text = originalText;
    }
}

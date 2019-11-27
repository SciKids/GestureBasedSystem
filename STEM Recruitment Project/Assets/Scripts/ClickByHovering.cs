using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickByHovering : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button myButton;
    private string originalButtonText;
    public int time = 3;
    private int timeLeft; 

    public void OnPointerEnter(PointerEventData eventData)
    {
        clickButton(myButton);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        stop();
    }
    // Call this method on EventTrigger -> PointerEnter.
    public void clickButton(Button thisButton)
    {
        timeLeft = time;

        myButton = thisButton;
        
        StartCoroutine(wait(timeLeft)); 
        
        if(isTextMeshProUGUI())
        {
            originalButtonText = myButton.GetComponentInChildren<TextMeshProUGUI>().text;
        }
        else
        {
            originalButtonText = myButton.GetComponentInChildren<Text>().text;
        }

        StartCoroutine(showTime());
    }

    // Method waits for 3 seconds then invokes a click.
    IEnumerator wait(int seconds)
    {
        yield return new WaitForSeconds(seconds);
      
        myButton.onClick.Invoke();
    }
    
    // Method adds countdown to button text
    IEnumerator showTime()
    {
        while (timeLeft>=0)
        {
            string newText = originalButtonText + " (" + timeLeft + ")";

            updateButtonText(newText);
            
            yield return new WaitForSeconds(1);

            timeLeft--;
        }

        updateButtonText(originalButtonText);
    }

    // Stops everything and changes button text to original. Call when pointer exits button.
    public void stop()
    {
        StopAllCoroutines();

        updateButtonText(originalButtonText);

       // Debug.Log(originalButtonText);
    }

    
    private bool isTextMeshProUGUI()
    {
        if(myButton.GetComponentInChildren<TextMeshProUGUI>() == null)
        {
            return false;
        }
        return true;
    }
    
    private void updateButtonText(string newText)
    {
        if(isTextMeshProUGUI())
        {
            myButton.GetComponentInChildren<TextMeshProUGUI>().text = newText;
        }
        else
        {
            myButton.GetComponentInChildren<Text>().text = newText;
        }
    }
}
   

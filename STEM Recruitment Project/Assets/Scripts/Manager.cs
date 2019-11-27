using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/* Drag and Drop system for the matching game.*/

public class Manager : MonoBehaviour
{
    public GameObject microscope, calc, microscope_outline, calc_outline;
    Vector2 microscopeInitialPos, calcInitialPos;
    public Outline outline_color;
    public int scoreValue = 10;
    public AudioSource source;
    public AudioClip correct;
    public AudioClip incorrect;



    public void Start()
    {
        microscopeInitialPos = microscope.transform.position;
        calcInitialPos = calc.transform.position;

    }

    public void DragMicro()
    {

        microscope.transform.position = Input.mousePosition;
    }

    public void DragCalc()
    {
        calc.transform.position = Input.mousePosition;
    }

    public void DropMicro()
    {
        float Distance = Vector3.Distance(microscope.transform.position, microscope_outline.transform.position);
        if (Distance < 50)
        {
            outline_color = microscope_outline.GetComponent<Outline>();
            microscope.transform.position = microscope_outline.transform.position;
            outline_color.effectColor = Color.green;
            GameProgress.instance.AddScore(scoreValue);
            source.clip = correct;
            source.Play();
        }
        else
        {
            microscope.transform.position = microscopeInitialPos;
            source.clip = incorrect;
            source.Play();
        }
    }

    public void DropCalc()
    {
        float Distance = Vector3.Distance(calc.transform.position, calc_outline.transform.position);
        if (Distance < 50)
        {
            outline_color = calc_outline.GetComponent<Outline>();
            calc.transform.position = calc_outline.transform.position;
            outline_color.effectColor = Color.green;
            GameProgress.instance.AddScore(scoreValue);
            source.clip = correct;
            source.Play();
        }
        else
        {
            calc.transform.position = calcInitialPos;
            source.clip = incorrect;
            source.Play();
        }
    }
}

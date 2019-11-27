using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckMatchingPictures : MonoBehaviour
{

    public GameObject picture, picture_outline;
    private Outline outline_color;
    private int scoreValue = 10;
    bool firstTime = true;
    Vector2 initialPos;
    public AudioSource source;
    public AudioClip correct;

    void Start()
    {
        initialPos = picture.transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        float Distance = Vector3.Distance(picture.transform.position, picture_outline.transform.position);

        if(Distance < 50)
        {
            outline_color = picture_outline.GetComponent<Outline>();

            picture.transform.position = picture_outline.transform.position;

            outline_color.effectColor = Color.green;
            
            if(firstTime)
            {
                GameProgress.instance.AddScore(scoreValue);
                firstTime = false;
                source.clip = correct;
                source.Play();
            }
        }

        else
        {
            firstTime = true;
            picture.transform.position = initialPos;

        }
    }
}

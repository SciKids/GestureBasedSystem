using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LoadSprites : MonoBehaviour
{
    public GameObject[] workers;
    public int numOfSprites; // number of available sprites to randomly load.

    // Start is called before the first frame update
    void Start()
    {
        System.Random rand = new System.Random();

        int[] unavailableSprites = new int[numOfSprites];

        int i = 0;
        
        while(i < workers.Length)
        {
            // Randomly selecting sprite design - all are labeled "Sprite2Torso" or "Sprite2Arm"
            int spriteNum = rand.Next(1, numOfSprites+1);

            if(!SpriteWasUsed(unavailableSprites, spriteNum))
            {
                // Getting full name of randomly selected sprite
                string torsoSpriteStr = "Worker" + spriteNum.ToString() + "Torso";
                string armSpriteStr = "Worker" + spriteNum.ToString() + "Arm";

                // Path that sprite designs are located in.
                string path = "Images/WorkerGameImages/Workers/";

                Sprite torsoSprite = Resources.Load(path + torsoSpriteStr, typeof(Sprite)) as Sprite;
                Sprite armSprite = Resources.Load(path + armSpriteStr, typeof(Sprite)) as Sprite;

                GameObject workerTorso = workers[i].transform.Find("Torso").gameObject;

                GameObject workerRightArm = workers[i].transform.Find("RightArm").gameObject;
                GameObject workerRightUp = workerRightArm.transform.Find("Up").gameObject;
                GameObject workerRightDown = workerRightArm.transform.Find("Down").gameObject;

                GameObject workerLeftArm = workers[i].transform.Find("LeftArm").gameObject;
                GameObject workerLeftUp = workerLeftArm.transform.Find("Up").gameObject;
                GameObject workerLeftDown = workerLeftArm.transform.Find("Down").gameObject;

                workerTorso.GetComponent<SpriteRenderer>().sprite = torsoSprite;

               // workerRightUp.GetComponent<SpriteRenderer>().sprite = armSprite;
                workerRightDown.GetComponent<SpriteRenderer>().sprite = armSprite;

                //workerLeftUp.GetComponent<SpriteRenderer>().sprite = armSprite;
                workerLeftDown.GetComponent<SpriteRenderer>().sprite = armSprite;

                //Debug.Log(path + torsoSpriteStr);
                //Debug.Log(path + armSpriteStr);

                unavailableSprites[i] = spriteNum;
               
                i++;
            }
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    bool SpriteWasUsed(int[] numOfSprites, int sprite)
    {
        if(numOfSprites.Contains(sprite))
        {
            return true;
        }
        return false;
    }
    
}
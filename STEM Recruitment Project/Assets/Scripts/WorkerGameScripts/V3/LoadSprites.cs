using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

// Script Summary ////////////////////////////////////////////////////////////
/*
 * Randomly loads a sprite to each of the workers. 
 */
public class LoadSprites : MonoBehaviour
{
    public GameObject[] workers;
    public int numOfSprites; // number of available sprites to randomly load.

    // Start is called before the first frame update
    void Start()
    {
        System.Random rand = new System.Random();

        // This temp array makes sure that a sprite isn't used twice. 
        int[] unavailableSprites = new int[numOfSprites];

        int i = 0;
        
        // Go through each of the workers on screen.
        while(i < workers.Length)
        {
            // Randomly selecting sprite design - all are labeled "Sprite2Torso" or "Sprite2Arm"
            int spriteNum = rand.Next(1, numOfSprites+1);

            // If a sprite is not present in unavailableSprites
            if(!SpriteWasUsed(unavailableSprites, spriteNum))
            {
                // Getting full name of randomly selected torso and arm sprite.
                string torsoSpriteStr = "Worker" + spriteNum.ToString() + "Torso";
                string armSpriteStr = "Worker" + spriteNum.ToString() + "Arm";

                // Path that sprite designs are located in.
                string path = "Images/WorkerGameImages/Workers/";

                // Declare torso and arm sprite, then load the corresponding images. 
                Sprite torsoSprite = Resources.Load(path + torsoSpriteStr, typeof(Sprite)) as Sprite;
                Sprite armSprite = Resources.Load(path + armSpriteStr, typeof(Sprite)) as Sprite;

                // Get the dummy torso
                GameObject workerTorso = workers[i].transform.Find("Torso").gameObject;

                // Get the dummy right arm
                GameObject workerRightArm = workers[i].transform.Find("RightArm").gameObject;
                GameObject workerRightUp = workerRightArm.transform.Find("Up").gameObject;
                GameObject workerRightDown = workerRightArm.transform.Find("Down").gameObject;

                // Get the dummy left arm
                GameObject workerLeftArm = workers[i].transform.Find("LeftArm").gameObject;
                GameObject workerLeftUp = workerLeftArm.transform.Find("Up").gameObject;
                GameObject workerLeftDown = workerLeftArm.transform.Find("Down").gameObject;

                // Set the dummy torso to the loaded torso sprite
                workerTorso.GetComponent<SpriteRenderer>().sprite = torsoSprite;
                
                // Set the dummy right arm to the loaded right arm sprite
                workerRightDown.GetComponent<SpriteRenderer>().sprite = armSprite;
                
                // Set the dummy left arm to the loaded left arm sprite
                workerLeftDown.GetComponent<SpriteRenderer>().sprite = armSprite;
                
                // Add the loaded sprite num to unavailableSprites
                unavailableSprites[i] = spriteNum;
               
                // Go to next sprite
                i++;
            }// end if

        }// end while

    }// end Start

    // Restarts the current string. This is called through the restart button
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }// end ReloadScene

    // If a given int is in the array, return true. Otherwise, return false
    bool SpriteWasUsed(int[] numOfSprites, int sprite)
    {
        if(numOfSprites.Contains(sprite))
        {
            return true;
        }
        return false;
    }// end SpriteWasUsed
    
}// end LoadSprites
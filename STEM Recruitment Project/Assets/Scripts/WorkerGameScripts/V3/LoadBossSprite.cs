using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script Summary ////////////////////////////////////////////////////////////
/*
 * Adds and removes workers to and from array, checks each workers correctness, 
 * and can reset scene
 */
public class LoadBossSprite : MonoBehaviour
{
    // Calls LoadSprite on startup
    void Start()
    {
        LoadSprite();
    }
    
    // Randomly chooses and loads a boss sprite. This can be called from other classes.
    public void LoadSprite()
    {
        // Get file path of boss sprites
        string path = "Images/WorkerGameImages/Bosses/";

        // Load all possible boss sprites.
        Sprite[] allSprites = Resources.LoadAll<Sprite>(path);

        Debug.Log("AllSprites length: " + allSprites.Length);
        
        System.Random rand = new System.Random();

        // Randomly get an index number
        int randSprite = rand.Next(0, allSprites.Length);

        // Get the spriterenderer component of the boss object
        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();

        // Set the sprite to the randomly chosen sprite.
        sprite.sprite = allSprites[randSprite];
    }

}// end LoadBossSprite

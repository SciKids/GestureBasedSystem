using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBossSprite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LoadSprite();
    }
    
    public void LoadSprite()
    {
        string path = "Images/WorkerGameImages/Bosses/";

        Sprite[] allSprites = Resources.LoadAll<Sprite>(path);

        Debug.Log("AllSprites length: " + allSprites.Length);
        System.Random rand = new System.Random();

        int randSprite = rand.Next(0, allSprites.Length);

        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();

        sprite.sprite = allSprites[randSprite];
    }
}

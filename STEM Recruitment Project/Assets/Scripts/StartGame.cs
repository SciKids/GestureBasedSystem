using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{

    public float timeLeft = 20f;
    public Text startText;


    void Update()
    {
        timeLeft -= Time.deltaTime;
        startText.text = "Count Down: "+(timeLeft).ToString("0");
        if (timeLeft < 0)
        {
            startText.text = "GAME OVER."; 
        }
    }
}
 
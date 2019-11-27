using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ExitWithBckSpce : MonoBehaviour
{
    public string scene;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("backspace"))
        {
            SceneManager.LoadScene(scene);
        }
    }
}

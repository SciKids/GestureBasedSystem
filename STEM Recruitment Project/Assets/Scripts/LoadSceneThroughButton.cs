using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneThroughButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void changeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

}

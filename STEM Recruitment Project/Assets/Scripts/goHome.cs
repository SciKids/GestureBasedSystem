using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class goHome : MonoBehaviour
{
    public Button backBtn;
    // Start is called before the first frame update
    void Start()
    {
        backBtn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene("Home");
    }

}

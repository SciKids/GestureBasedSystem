using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingSplashScreen : MonoBehaviour
{
    public TextMeshProUGUI instructions;
    public string newScene;

    IEnumerator Start()
    {
        instructions.canvasRenderer.SetAlpha(0.0f);
        FadeIn();
        yield return new WaitForSeconds(2.5f);
        FadeOut();
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(newScene);
    }

    void FadeIn()
    {
        instructions.CrossFadeAlpha(1.0f, 1.5f, false);

    }

    void FadeOut()
    {
        instructions.CrossFadeAlpha(0.0f, 2.5f, false);

    }
}

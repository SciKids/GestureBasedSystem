using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SplashScene : MonoBehaviour
{
    public Image sciLogo, nauLogo ;
    public string newScene;

    IEnumerator Start()
    {
        sciLogo.canvasRenderer.SetAlpha(0.0f);
        nauLogo.canvasRenderer.SetAlpha(0.0f);


        FadeIn();
        yield return new WaitForSeconds(2.5f);
        FadeOut();
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(newScene);
    }

    void FadeIn()
    {
        sciLogo.CrossFadeAlpha(1.0f, 1.5f, false);
        nauLogo.CrossFadeAlpha(1.0f, 1.5f, false);

    }
    void FadeOut()
    {
        sciLogo.CrossFadeAlpha(0.0f, 2.5f, false);
        nauLogo.CrossFadeAlpha(0.0f, 2.5f, false);

    }
}



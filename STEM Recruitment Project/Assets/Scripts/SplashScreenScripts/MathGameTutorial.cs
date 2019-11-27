using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MathGameTutorial : MonoBehaviour
{
    public Image gamePicture;
    public TextMeshProUGUI message;
    public string newScene;

    IEnumerator Start()
    {
        gamePicture.canvasRenderer.SetAlpha(0.0f);
        message.canvasRenderer.SetAlpha(0.0f);


        FadeIn();
        yield return new WaitForSeconds(5.5f);
        FadeOut();
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(newScene);
    }

    void FadeIn()
    {
        gamePicture.CrossFadeAlpha(1.0f, 1.5f, false);
        message.CrossFadeAlpha(1.0f, 1.5f, false);

    }
    void FadeOut()
    {
        gamePicture.CrossFadeAlpha(0.0f, 2.5f, false);
        message.CrossFadeAlpha(0.0f, 2.5f, false);

    }
}

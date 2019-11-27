using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashTutorialScene : MonoBehaviour
{
    public Image tutorialPic, rightHandPic;
    public TextMeshProUGUI description;
    public string newScene;

    IEnumerator Start()
    {
        tutorialPic.canvasRenderer.SetAlpha(0.0f);
        rightHandPic.canvasRenderer.SetAlpha(0.0f);
        description.canvasRenderer.SetAlpha(0.0f);

        FadeIn();
        yield return new WaitForSeconds(5.5f);
        FadeOut();
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(newScene);
    }

    void FadeIn()
    {
        tutorialPic.CrossFadeAlpha(1.0f, 1.5f, false);
        rightHandPic.CrossFadeAlpha(1.0f, 1.5f, false);
        description.CrossFadeAlpha(1.0f, 1.5f, false);

    }
    void FadeOut()
    {
        tutorialPic.CrossFadeAlpha(0.0f, 2.5f, false);
        rightHandPic.CrossFadeAlpha(0.0f, 2.5f, false);
        description.CrossFadeAlpha(0.0f, 2.5f, false);

    }
}

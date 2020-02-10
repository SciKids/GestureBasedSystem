using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterviewSplashScene : MonoBehaviour
{
    public Image pic1, pic2;
    public string newScene;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        pic1.canvasRenderer.SetAlpha(0.0f);
        pic2.canvasRenderer.SetAlpha(0.0f);
        
        FadeIn(pic1);
        yield return new WaitForSeconds(8.0f);
        FadeOut(pic1);

        yield return new WaitForSeconds(2.0f);

        FadeIn(pic2);
        yield return new WaitForSeconds(8.0f);
        FadeOut(pic2);
        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene(newScene);
    }
    
    void FadeIn(Image pic)
    {
        pic.CrossFadeAlpha(1.0f, 1.5f, false);
    }

    void FadeOut(Image pic)
    {
        pic.CrossFadeAlpha(0.0f, 2.5f, false);
    }
}

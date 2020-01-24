using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorkerGameSplash : MonoBehaviour
{
    public CanvasGroup canvas1, canvas2;
    public string sceneToLoad;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        FadeIn(canvas1);
        yield return new WaitForSeconds(8f);
        FadeOut(canvas1);
        yield return new WaitForSeconds(2f);

        FadeIn(canvas2);
        yield return new WaitForSeconds(8f);
        FadeOut(canvas2);
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(sceneToLoad);
    }

    void FadeIn(CanvasGroup canvas)
    {
        StartCoroutine(FadeCanvasGroup(canvas, canvas.alpha, 1));
    }

    void FadeOut(CanvasGroup canvas)
    {
        StartCoroutine(FadeCanvasGroup(canvas, canvas.alpha, 0));
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 0.5f)
    {
        float timeStartLerp = Time.time;
        float timeSinceStart = Time.time - timeStartLerp;
        float percentComplete = timeSinceStart / lerpTime;

        while(true) // while(true) and WaitForEndOfFrame act as an update function
        {
            timeSinceStart = Time.time - timeStartLerp;
            percentComplete = timeSinceStart / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentComplete);

            cg.alpha = currentValue;

            if(percentComplete >= 1)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Done");
    }

    // Allows users to skip tutorial
    public void SkipTutorial()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}

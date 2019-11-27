using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackCollider : MonoBehaviour
{
    bool active = true;
    Text display_time;
    int sec = 3;
    public string scene;
    public Collider2D backBtn;

    private void OnTriggerEnter(Collider collision)
    {
        if (!active)
            return;
        active = false;

        if(collision.transform.tag == "BackCollider")
        {
            StartCoroutine(GoBack());
            StartCoroutine(ShowTime());
        }
    }

    IEnumerator GoBack()
    {
        yield return new WaitForSeconds(sec);

        SceneManager.LoadScene(scene);
    }
    IEnumerator ShowTime()
    {
        int time_left = sec;

        while(time_left >= 0)
        {
            string time = time_left.ToString();
            display_time.text = time;

            yield return new WaitForSeconds(1);

            time_left--;
        }
        display_time.text = " ";
    }
}

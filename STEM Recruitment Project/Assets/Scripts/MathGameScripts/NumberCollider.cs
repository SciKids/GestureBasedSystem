using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberCollider : MonoBehaviour
{
    //public GameObject spriteAnim;
    public GameObject correct, wrong;
    public GameObject[] otherNumbers = new GameObject[3]; // TEMPORARY
    int num = 0;
    GameObject mainCamera;

    // bool active = true;
    bool isCorrectAnswer;
    bool blocked = false;

    private void Awake()
    {
        mainCamera = GameObject.Find("/Main Camera");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!blocked)
        {
            StartCoroutine(PlayAnim());
        }

    }

    public void SetNum(int newNum)
    {
        num = newNum;
        Debug.Log(this.name + " number set to: " + num);
    }

    public void SetCorrect(bool status)
    {
        isCorrectAnswer = status;
    }

    IEnumerator PlayAnim()
    {
        if (isCorrectAnswer)
        {
            correct.SetActive(true);
            Vector3 initPos = correct.transform.position;
            correct.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, initPos.z);
            BlockOtherNumbers(true);
            correct.GetComponent<Animator>().Play("Grow");
            yield return new WaitForSeconds(1.0f);
            BlockOtherNumbers(false);
            correct.transform.position = initPos;
            correct.SetActive(false);
            mainCamera.SendMessage("PressAnswer", num);
        }
        else
        {
            wrong.SetActive(true);
            Vector3 initPos = wrong.transform.position;
            wrong.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, initPos.z);
            BlockOtherNumbers(true);
            wrong.GetComponent<Animator>().Play("Grow");
            yield return new WaitForSeconds(1.0f);
            BlockOtherNumbers(false);
            wrong.transform.position = initPos;
            wrong.SetActive(false);
            mainCamera.SendMessage("PressAnswer", num);
        }
    }

    void BlockOtherNumbers(bool status)
    {
        for (int i = 0; i < 3; i++)
        {
            otherNumbers[i].SendMessage("Block", status);
        }
    }
    public void Block(bool status)
    {
        blocked = status;
    }
}

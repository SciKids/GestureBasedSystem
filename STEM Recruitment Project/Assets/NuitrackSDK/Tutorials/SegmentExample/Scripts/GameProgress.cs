using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameProgress : MonoBehaviour
{
    public static GameProgress instance = null;

    [SerializeField]
    Text scoreText;

    int currentScore = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void UpdateScoreText()
    {
        scoreText.text = "Your score: " + currentScore;
    }

    public void AddScore(int val)
    {
        DBManager dbManage = GetComponent<DBManager>();
        currentScore += val;
        UpdateScoreText();

        int sceneID = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(sceneID);
        dbManage.addScoreToDB(currentScore, dbManage.getID(), sceneID);

    }

    public void RemoveScore(int val)
    {
        currentScore -= val;
        UpdateScoreText();
    }

}

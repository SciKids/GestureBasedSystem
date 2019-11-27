using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MathGameProgress : MonoBehaviour
{
    public static MathGameProgress instance = null;

    [SerializeField]
    Text scoreText;

    [SerializeField]
    Text responseText;

    [SerializeField]
    Text currentScore;

    [SerializeField]
    Text highScore;

    int firstNum;
    int secondNum;
    int ans;
    int givenAns;
    int score = 0;
    string strAns;

    string[] myObjectsNames = new string[] { "button_score_1(Clone)",
                                             "button_score_2(Clone)",
                                             "button_score_3(Clone)",
                                             "button_score_4(Clone)"};

    private void Start()
    {
        // Make cursor invisible
        Cursor.visible = false;
        DBManager dbManage = GetComponent<DBManager>();

        if (dbManage.getStatus())
        {
            int userID = dbManage.getID();
            int gameID = SceneManager.GetActiveScene().buildIndex;
            highScore.text = DisplayHighScore(userID, gameID);

        }
        CreateEquation();
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void OnDestroy()
    {
        Cursor.visible = true;
    }

    void CreateEquation()
    {
        RandomEquation();
        scoreText.text = firstNum.ToString() + "+" + secondNum.ToString() + "=";

    }

    void RandomEquation()
    {
        firstNum = Random.Range(0, 3);
        secondNum = Random.Range(0, 3);
        ans = firstNum + secondNum;
        if (ans == 0)
        {
            RandomEquation();
        }
    }

    void UpdateScoreText()
    {
        DBManager dbManage = GetComponent<DBManager>();
        scoreText.text += strAns;


        if (givenAns == ans)
        {

            responseText.text = "CORRECT";
            score += 5;
            currentScore.text = "Your Score: " + score.ToString();
            //// Adding score to database ///////
            // If the user is logged in...
           /* if (dbManage.getStatus())
            {
                int userID = dbManage.getID();
                int gameID = SceneManager.GetActiveScene().buildIndex;

                // If the score is higher than the one saved in the database...
                if (dbManage.getHighScore(userID, gameID) < score)
                {
                    // Set new high score
                    dbManage.addScoreToDB(score, userID, gameID);
                    // Display new high score
                    highScore.text = DisplayHighScore(userID, gameID);
                }
            }*/
            StartCoroutine(TimeStop());
            //responseText.text = "";
            //CreateEquation();

        }
        else
        {
            responseText.text = "INCORRECT, NEXT";
            score -= 5;
            currentScore.text = "Your Score: " + score.ToString();
            StartCoroutine(TimeStop());
            //responseText.text = "";
            //CreateEquation();
        }

    }

    public void AddNumber(string strVal, int intVal)
    {
        strAns = strVal;
        givenAns = intVal;
        if (scoreText.text.Length < 5)
        {
            UpdateScoreText();
        }

    }

    IEnumerator TimeStop()
    {
        yield return new WaitForSeconds(3f);
        DeleteAll();
        responseText.text = "";
        CreateEquation();
    }

    public void DeleteAll()
    {
        foreach (string name in myObjectsNames)
        {
            GameObject go = GameObject.Find(name);
            //if the tree exist then destroy it
            if (go)
                Destroy(go.gameObject);
        }
    }

    private string DisplayHighScore(int userID, int gameID)
    {
        DBManager dbManage = GetComponent<DBManager>();
        int score = dbManage.getHighScore(userID, gameID);
        string display = "High Score: " + score;
        return display;
    }

}

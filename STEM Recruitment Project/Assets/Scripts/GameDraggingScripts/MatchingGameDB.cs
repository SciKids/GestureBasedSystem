using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchingGameDB : MonoBehaviour
{
    public Text username_text;
    // Start is called before the first frame update
    void Start()
    {
        DBManager dbManage = GetComponent<DBManager>();
        // If user is logged in, print username
        if (dbManage.getStatus())
        {
            int id = dbManage.getID();
            int gameID = SceneManager.GetActiveScene().buildIndex;
            username_text.text = "User: " + dbManage.getUsername() + ", High Score: " + dbManage.getHighScore(id, gameID);
        }
    }
    
}

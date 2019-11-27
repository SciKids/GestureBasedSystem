using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public UnityEngine.UI.Button logInButton; 
    public TextMeshProUGUI salutation; 

    // Use this for initialization
    void Start()
    {
        // Create the database on startup if it does not yet exist.
        DBManager dbManage = GetComponent<DBManager>();
        Debug.Log(dbManage.getFilePath());

        if (!File.Exists(dbManage.getFilePath()))
        {
            dbManage.makeDatabase(dbManage.getFilePath());
        }

        if (dbManage.getStatus())
        {
            salutation.text = "Welcome " + dbManage.getUsername();

            logInButton.GetComponentInChildren<TextMeshProUGUI>().text = "Log Out";
        }

    }
    // Functions to load new scenes 
    public void Modules ()
    {
        // Call to new scene
        SceneManager.LoadScene("Modules");

    }

    public void LogIn()
    {
        DBManager dbManage = GetComponent<DBManager>();

        if(!dbManage.getStatus())
        {
            SceneManager.LoadScene("LogIn");
        }
        // Call to new scene
        else
        {
            dbManage.logOut();

            // Reload scene
            Scene thisScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(thisScene.name);
        }
    
    }

    public void Register()
    {
        // Call to new scene
        SceneManager.LoadScene("Registration");

    }

    public void AboutUs()
    {
        // Call to new scene
        SceneManager.LoadScene("AboutUs");

    }

    public void Settings()
    {
        // Call to new scene
        SceneManager.LoadScene("Settings");

    }

    public void Back()
    {
        // Call to new scene
        SceneManager.LoadScene("Home");

    }

    // Exits game 
    public void quitGame()
    {
        // Displays message in console of Unity
        Debug.Log("Succesful Quit");

        // Log out of account
        DBManager dbManage = GetComponent<DBManager>();
        dbManage.logOut();

        // Quits game
        Application.Quit(); 

    }

}

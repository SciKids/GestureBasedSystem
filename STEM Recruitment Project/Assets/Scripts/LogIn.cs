using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;
using System;
using System.Data;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogIn : MonoBehaviour
{

    public InputField nameField;
    public InputField passwordField;
    public Text errorDisplay;
    public Button submitButton, registerButton;
    private DBManager dbManage;
    private int id; // ID for future use.


    // Use this for initialization
    void Start()
    {
        dbManage = GetComponent<DBManager>();

        dbTest();
        // If user is already logged in: 
        if (dbManage.getStatus())
        {
            // Display username
            errorDisplay.text = "Welcome " + dbManage.getUsername();
            
            // Deactivate fields    
            nameField.DeactivateInputField();
            passwordField.DeactivateInputField();
            submitButton.GetComponent<Button>().interactable = false;
            registerButton.GetComponent<Button>().interactable = false;

        }
        else
        {
            // Make password show up as astrisks
            passwordField.contentType = InputField.ContentType.Password;

            // Go to TaskOnClick when button is pressed
            submitButton.onClick.AddListener(TaskOnClick);
            
        }
      
    }

   
    void dbTest()
    {
        dbManage = GetComponent<DBManager>();
        int id = dbManage.getID();

        Debug.Log(id);
        Debug.Log(dbManage.getUsername());
    }

    void TaskOnClick()
    {
        dbManage = GetComponent<DBManager>();

        string conn = LoadConnectionString(); // Once button is clicked, open database

        Debug.Log(conn);

        IDbConnection dbconn = new SqliteConnection(conn);

        dbconn.Open();
        
        if (nameField.text == null || passwordField.text == null) // Make sure there is an entry for username and password
        {
            errorDisplay.text = "Please enter a username or password.";
        }
        else
        {
            if (accountExists(nameField.text, passwordField.text, dbconn))
            {
                //errorDisplay.text = "Login Successful. Welcome back " + nameField.text;

                id = getID(nameField.text, dbconn);
           
                dbManage.writeID(id);

                SceneManager.LoadScene("Home");
            }
            else
            {
                Debug.Log("Couldn't find account");
            }
        }
    }

    bool accountExists(string username, string password, IDbConnection dbconn)
    {
        // Counting all instances of given username
        IDbCommand checkUName = dbconn.CreateCommand();

        checkUName.CommandText = "SELECT count(*) FROM user WHERE username = '" + username + "';";

        int UCount = Convert.ToInt32(checkUName.ExecuteScalar());

        // Counting all instances of given password
        IDbCommand checkPass = dbconn.CreateCommand();

        checkPass.CommandText = "SELECT count(*) FROM user WHERE password = '" + password + "';";

        int PCount = Convert.ToInt32(checkPass.ExecuteScalar());

        if (UCount == 0 || PCount == 0) // If username or password is not found, return false
        {
            errorDisplay.text = "Incorrect username or password";

            return false;
        }

        return true;
    }

    int getID(string username, IDbConnection dbconn)
    {
        IDbCommand idQuery = dbconn.CreateCommand();

        idQuery.CommandText = "SELECT id FROM user WHERE username = '" + username + "';";

        int id = Convert.ToInt32(idQuery.ExecuteScalar());

        return id;
    }
    string LoadConnectionString()
    {
        if (Application.isEditor)
        {
            return "URI=file:" + Application.dataPath + "/Plugins/prototypedb.s3db;";
        }
        else
        {
            return "URI=file:" + Application.persistentDataPath + "/prototypedb.s3db;";
        }

    } // end LoadConnectionString()

    //////// This method is just for testing purposes. It iterates through database and prints everything in it. //////
    void returnAll(IDbConnection dbconn)
    {
        IDataReader reader;

        IDbCommand command = dbconn.CreateCommand();

        command.CommandText = "SELECT * FROM user";

        reader = command.ExecuteReader();

        while (reader.Read())
        {
            Debug.Log("id: " + reader[0].ToString() + " username: " + reader[1].ToString() + " password: " + reader[2].ToString());

        }
    }// end returnAll
}

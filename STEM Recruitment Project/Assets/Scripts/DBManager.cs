using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;
using System;
using System.Data;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DBManager : MonoBehaviour
{
    private bool isLoggedIn;

    private int playerID;

    public bool getStatus()
    {
        string path = getTxtPath();

        if (File.Exists(path))
        {
            return true;
        }

        return false;
    }

    public void setStatus(bool status)
    {
        isLoggedIn = status;
    }


    public int getID()
    {
        if (!File.Exists(getTxtPath()))
        {
            Debug.Log("Error: File does not exist (DBManager.getID)");
            return -1;
        }

        else
        {
            using (TextReader reader = File.OpenText(getTxtPath()))
            {
                int id = int.Parse(reader.ReadLine());
                return id;
            }
        }
    }

    public string getUsername()
    {
        string conn = loadConnectionString();

        string returnUsername = "";

        int id = getID();

        SqliteConnection dbconn = new SqliteConnection(conn);

        dbconn.Open();

        SqliteCommand command = new SqliteCommand(dbconn);

        command.CommandText = "SELECT username FROM user WHERE id = " + id + ";";

        SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            returnUsername += reader["username"];
        }

        Debug.Log(returnUsername);
        dbconn.Close();
        return returnUsername;

    }

    string loadConnectionString()
    {
        if (Application.isEditor)
        {
            return "URI=file:" + Application.dataPath + "/Plugins/prototypedb.s3db;";
        }
        else
        {
            return "URI=file:" + Application.persistentDataPath + "/prototypedb.s3db;";
        }

    }

    // Same as loadConnectionString() but without "URI=file:" -- for other files to use.
    public string getFilePath()
    {
        if (Application.isEditor)
        {
            return Application.dataPath + "/Plugins/prototypedb.s3db;";
        }
        else
        {
            return Application.persistentDataPath + "/prototypedb.s3db;";
        }
    }
    // When user logs in/registers, a temporary file is made 
    public void writeID(int userID)
    {
        string path = getTxtPath();

        if (File.Exists(path))
        {
            Debug.Log("Error: file already exists");

            logOut();

        }

        File.Create(path).Dispose();

        TextWriter writer = new StreamWriter(path);

        // Writes userID to file
        writer.Write(userID);

        writer.Close();
    }

    static void readStatus()
    {
        string path = getTxtPath();

        Debug.Log(path);

        StreamReader reader = new StreamReader(path);

        string status = reader.ReadToEnd();

        Debug.Log(status);

        reader.Close();
    }

    public void logOut()
    {
        string path = getTxtPath();

        File.Delete(path);
    }

    static string getTxtPath()
    {
        if (Application.isEditor)
        {
            return Application.dataPath + "/Plugins/status.txt";
        }
        else
        {
            return Application.persistentDataPath + "/status.txt";
        }
    }

    public void editUserSensitivityVal(int userID, float value)
    {
        string path = loadConnectionString();

        IDbConnection dbconn = new SqliteConnection(path);

        dbconn.Open();

        // Check if user is already in table
        IDbCommand checkUser = dbconn.CreateCommand();
        checkUser.CommandText = "SELECT userID FROM settings WHERE userID = " + userID + ";";
        int check = Convert.ToInt32(checkUser.ExecuteScalar());

        // Edit current setting
        if(check > 0)
        {
            IDbCommand editSetting = dbconn.CreateCommand();

            editSetting.CommandText = "UPDATE settings SET sensitivityValue = " + value + 
                " WHERE userID = " + userID + ";";

            Debug.Log(editSetting.CommandText);

            editSetting.ExecuteNonQuery();
        }
        // Add new setting
        else
        {
            IDbCommand addNewSetting = dbconn.CreateCommand();

            addNewSetting.CommandText = "INSERT INTO settings(userID, sensitivityValue) VALUES (" + userID +
                ", " + value + ");";

            Debug.Log(addNewSetting.CommandText);

            addNewSetting.ExecuteNonQuery();
            
        }
    }

    public float getUserSensitivity(int userID)
    {
        // If no one is logged in, return a default value.
        if(getStatus() == false)
        {
            return 3.0f;
        }
        
        // If a user is logged in, pull their custom sensitivity from the database and return value.
        string path = loadConnectionString();

        IDbConnection dbconn = new SqliteConnection(path);

        dbconn.Open();

        IDbCommand getValue = dbconn.CreateCommand();

        getValue.CommandText = "SELECT sensitivityValue FROM settings WHERE userID = " + userID + ";";

        float val = (float) Convert.ToDecimal(getValue.ExecuteScalar());

        return val;
    }

    public void addScoreToDB(int score, int userID, int gameID)
    {
        if(getID() < 0)
        {
            return;
        }

        string path = loadConnectionString();

        IDbConnection dbconn = new SqliteConnection(path);

        dbconn.Open();

        // Check to see if user has already played game before
        IDbCommand checkUser = dbconn.CreateCommand();
        checkUser.CommandText = "SELECT userID FROM score WHERE userID = " + userID + ";";
        Debug.Log(checkUser.CommandText);
        int check = Convert.ToInt32(checkUser.ExecuteScalar());

        // Check if score is higher or lower than score already in database
        IDbCommand checkScore = dbconn.CreateCommand();
        checkScore.CommandText = "SELECT userScore FROM score WHERE userID = " + userID + ";";
        Debug.Log(checkScore.CommandText);
        int check2 = Convert.ToInt32(checkScore.ExecuteScalar());

        // If user is already in table...
        if (check > 0)
        {
            // If score is lower than new score...
            if(check2<score)
            {
                // Add to table
                IDbCommand updateScore = dbconn.CreateCommand();

                updateScore.CommandText = "UPDATE score SET userScore = " + score + " WHERE userID = " + userID + " AND gameID = " + gameID + ";";

                updateScore.ExecuteNonQuery();
            }
        }


        // If user is not in table ...
        else
        {
            IDbCommand addNewUser = dbconn.CreateCommand();

            addNewUser.CommandText = "INSERT INTO score(gameID, userID, userScore) VALUES (" + gameID +
                ", " + userID + ", " + score + ");";

            Debug.Log(addNewUser.CommandText);

            addNewUser.ExecuteNonQuery();

            Debug.Log("Score of " + score + " was added.");
        }
    }

    public int getHighScore(int userID, int gameID)
    {
        if(getStatus() == false)
        {
            return -1;
        }

        string path = loadConnectionString();

        IDbConnection dbconn = new SqliteConnection(path);

        dbconn.Open();

        IDbCommand getScoreCmd = dbconn.CreateCommand();

        getScoreCmd.CommandText = "SELECT userScore FROM score WHERE userID = " + userID + ";";

        int score = Convert.ToInt32(getScoreCmd.ExecuteScalar());

        return score;


    }

    // Creates the database if doesn't exist
    public void makeDatabase(string filePath)
    {
        SqliteConnection.CreateFile(filePath);

        SqliteConnection dbconn = new SqliteConnection("URI=file:" + filePath);

        dbconn.Open();

        // Create 1st table - user
        string command = "CREATE TABLE IF NOT EXISTS user(id INTEGER PRIMARY KEY, " +
            "username VARCHAR(30) NOT NULL, " +
            "password VARCHAR(30) NOT NULL);";
        Debug.Log(command);
        SqliteCommand makeTableQuery1 = dbconn.CreateCommand();
        makeTableQuery1.CommandText = command;
        Debug.Log(makeTableQuery1.CommandText);
        makeTableQuery1.ExecuteNonQuery();

        // Create 2nd table - score
        string command2 = "CREATE TABLE IF NOT EXISTS score(" +
            "gameID INTEGER," +
            "userID INTEGER," +
            "userScore INTEGER," +
            "FOREIGN KEY(userID) REFERENCES user(id));";

        SqliteCommand makeTableQuery2 = dbconn.CreateCommand();
        makeTableQuery2.CommandText = command2;
        makeTableQuery2.ExecuteNonQuery();

        // Create 3rd table - module
        string command3 = "CREATE TABLE IF NOT EXISTS module(" +
             "modID INTEGER PRIMARY KEY," +
             "userID INTEGER," +
             "gameID INTEGER," +
             "FOREIGN KEY(userID) REFERENCES user(id)," +
             "FOREIGN KEY(gameID) REFERENCES score(gameID));";

        SqliteCommand makeTableQuery3 = dbconn.CreateCommand();
        makeTableQuery3.CommandText = command3;
        makeTableQuery3.ExecuteNonQuery();

        // Create 4th table - settings
        string command4 = "CREATE TABLE IF NOT EXISTS settings(" +
            "userID INTEGER PRIMARY KEY," +
            "sensitivityValue FLOAT," +
            "FOREIGN KEY(userID) REFERENCES user(id));";
        SqliteCommand makeTableQuery4 = dbconn.CreateCommand();
        makeTableQuery4.CommandText = command4;
        makeTableQuery4.ExecuteNonQuery();


        Debug.Log(command);

        dbconn.Close();
    } // end makeDatabase()
}

using System;
using System.IO;
using UnityEngine;

/*
Notes for Pinball Assignment:
-our example saves when you click the button
-but
-your Pinball game will save whenever a game is complete, and the current score is higher than the high score

also
-our example saves the entire GameData class, including current score
-your Pinball game should set current score to zero before saving

*/
//BE CAREFUL
//This script must be a component of a game object that is tagged with "GameState"
//in the OPENING scene of the game

[Serializable]
public class GameData
{
    public int _score = 0;
    public int _highScore = 0;
}
public class GameState : MonoBehaviour
{
    private GameData _gameData;
    public int Score { get => _gameData._score; set => _gameData._score = value; }
    public int HighScore { get => _gameData._highScore; set => _gameData._highScore = value; }
    public int BallsRemaining = 3;

    void Awake()
    {
        _gameData = new GameData();
        LoadFromDisk();

        GameObject [] objs = GameObject.FindGameObjectsWithTag("GameState");
        if(objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void SaveToDisk()
    {
        if (_gameData._score > _gameData._highScore)
        {
            _gameData._highScore = _gameData._score;
        }
        _gameData._score = 0;//reset score 

        string dataPath = Path.Combine(Application.persistentDataPath, "PinballScores.txt");
        string jsonString = JsonUtility.ToJson(_gameData);
        Debug.Log("Saving score to " + Application.persistentDataPath);

        using(StreamWriter streamWriter = File.CreateText(dataPath))
        {
            streamWriter.Write(jsonString);
            Debug.Log(dataPath);
        }
    }
    public void LoadFromDisk()
    {
        string dataPath = Path.Combine(Application.persistentDataPath, "PinballScores.txt");

        if(File.Exists(dataPath))
        {
            using (StreamReader streamReader = File.OpenText(dataPath))
            {
                //get the string that we wrote to the file
                string jsonString = streamReader.ReadToEnd();

                //convert the string to an object
                JsonUtility.FromJsonOverwrite(jsonString, _gameData);
            }
        }
        else
        {
            Debug.Log("No save file found at " + dataPath + " Starting Fresh !");
            _gameData = new GameData();
        }

    }
}

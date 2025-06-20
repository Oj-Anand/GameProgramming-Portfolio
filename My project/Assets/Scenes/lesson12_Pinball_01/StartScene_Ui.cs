using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class StartScene_Ui : MonoBehaviour
{
    private GameState _gameState;
    private Label _scoreLabel;
    private Label _highScoreLabel;
    void Start()
    {
        _gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();

        VisualElement _root = GetComponent<UIDocument>().rootVisualElement;
        
        _root.Q<Button>("LoadLevel01Button").clicked += LoadLevel01;
        _root.Q<Button>("SaveButton").clicked += Save;
        _root.Q<Button>("LoadButton").clicked += Load;

        _scoreLabel = _root.Q<Label>("ScoreLabel");
        _highScoreLabel = _root.Q<Label>("HighScoreLabel");
        if(_gameState.Score > _gameState.HighScore)
        {
            _gameState.HighScore = _gameState.Score;
        }
        PopulateScores();
    }
    private void PopulateScores()
    {
        _scoreLabel.text = $"{_gameState.Score}";
        _highScoreLabel.text = $"{_gameState.HighScore}";
    }
    private void Save()
    {
        _gameState.SaveToDisk();
    }
    private void Load()
    {
        _gameState.LoadFromDisk();
        _gameState.Score = 0;
        PopulateScores();
    }
    private void LoadLevel01()
    {
            _gameState.Score = 0;
        _gameState.BallsRemaining = 3;
        SceneManager.LoadScene("pinball");
        //_gameState.LoadFromDisk();
    }
}

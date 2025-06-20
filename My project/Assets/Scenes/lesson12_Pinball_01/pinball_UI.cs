using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class Pinball_UI : MonoBehaviour
{
    private GameState _gameState;
    private Label _scoreLabel;
    private Label _ballsLabel;
    private int _lastDisplayedScore = -1;
    void Start()
    {
        _gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        _scoreLabel = root.Q<Label>("ScoreLabel");
        _ballsLabel = root.Q<Label>("BallsLabel");

        _scoreLabel.text = $"Score: {_gameState.Score}";
        _ballsLabel.text = $"Balls: {_gameState.BallsRemaining}";
    }
    void Update()
    {
        if (_lastDisplayedScore != _gameState.Score)
        {
            _scoreLabel.text = $"{_gameState.Score}";
            _lastDisplayedScore = _gameState.Score;
        }
        _ballsLabel.text = $"Balls: {_gameState.BallsRemaining}";
    }
}

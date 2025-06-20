using UnityEngine;

public class GameState : MonoBehaviour
{
    private int _score = 0;        // Tracks player score
    private int _playerHP = 3;     // Player starts with 3 HP
    private int _enemyCount = 0;   // Number of enemies left
    private int _highScore = 0;  // Tracks the highest score
    private int _currentLevel = 1;  // Track current level

    // Public properties to access these values
    public int Score { get => _score; set => _score = value; }
    public int PlayerHP { get => _playerHP; set => _playerHP = value; }
    public int EnemyCount { get => _enemyCount; set => _enemyCount = value; }
    public int HighScore { get => _highScore; set => _highScore = value; }
    public int CurrentLevel 
    { 
        get => _currentLevel; 
        set
        {
            _currentLevel = value;
            if (_currentLevel == 2)
            {
                PlayerHP = 3; // Reset HP when transitioning to Level 2
            }
            ResetEnemyCount();
        }
    }

    void Awake()
    {
        GameObject [] objs = GameObject.FindGameObjectsWithTag("GameState");
        if(objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void UpdateHighScore()
    {
        if (_score > _highScore)
        {
            _highScore = _score;
        }
    }

    public void ResetEnemyCount()
    {
        EnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("Resetting EnemyCount to: " + EnemyCount);
    }
}

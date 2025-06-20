using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOver_UI : MonoBehaviour
{
    private Label finalScoreLabel;
    private Label highScoreLabel;
    private void OnEnable()
    {
        GameState gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
        // Get UI root
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        // Get UI elements
        Label finalScoreLabel = root.Q<Label>("FinalScoreLabel");
        Label highScoreLabel = root.Q<Label>("HighScoreLabel");
        Button playAgainButton = root.Q<Button>("PlayAgainButton");

        // Load the final score (we will later update this via a StateManager)
        finalScoreLabel.text = "Your Score: " + gameState.Score ;
        highScoreLabel.text = "High Score: " + gameState.HighScore;

        // Assign button functionality
        if (playAgainButton != null)
            playAgainButton.clicked += RestartGame;
    }

    private void RestartGame()
    {
        GameState gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
        gameState.Score = 0;
        SceneManager.LoadScene("StartMenu");  // Restart the game
    }
}

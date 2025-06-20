using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    private GameState _gameState;
    void Start()
    {
        _gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO: put a tag on your bumper and your drop target (for example)
        //check the tag here and award points based on what the ball hit
        _gameState.Score++;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        //TODO: 
        // -number of balls should be stored in GameState
        //- since a ball just hit the DeathZone, decrease the number of balls by one 
        // -is this the last ball? load the start scene
        // - OPTIONAL: to make the transition more smooth:
        //              -change a label in this scene to "Game Over"
        //              -then wait 5 seconds, then load StartScene
        //SceneManager.LoadScene("StartScene");
        if (collider.CompareTag("DeathZone"))
        {
            _gameState.BallsRemaining--;

            if (_gameState.BallsRemaining <= 0)
            {
                _gameState.SaveToDisk(); //auto save
                SceneManager.LoadScene("StartScene"); // or GameOverScene if you have one
            }
        }
    }
}
